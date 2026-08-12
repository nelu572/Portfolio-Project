import argparse
import os
import sys

import bpy


def parse_args():
    argv = sys.argv
    if "--" in argv:
        argv = argv[argv.index("--") + 1:]
    else:
        argv = []

    parser = argparse.ArgumentParser()
    parser.add_argument("--blend", required=True)
    parser.add_argument("--outdir", required=True)
    return parser.parse_args(argv)


def keep_actions(prefix):
    for action in list(bpy.data.actions):
        if not action.name.startswith(prefix):
            bpy.data.actions.remove(action)


def remove_constraints(armature):
    for constraint in list(armature.constraints):
        armature.constraints.remove(constraint)

    for pose_bone in armature.pose.bones:
        for constraint in list(pose_bone.constraints):
            pose_bone.constraints.remove(constraint)


def serialize_action(action, name):
    curves = []
    for curve in action.fcurves:
        keyframes = []
        for point in curve.keyframe_points:
            keyframes.append(
                {
                    "co": (point.co.x, point.co.y),
                    "interpolation": point.interpolation,
                    "easing": point.easing,
                }
            )

        curves.append(
            {
                "data_path": curve.data_path,
                "array_index": curve.array_index,
                "keyframes": keyframes,
            }
        )

    return {
        "name": name,
        "curves": curves,
    }


def recreate_actions(action_data):
    created = []
    for data in action_data:
        action = bpy.data.actions.new(data["name"])
        for curve_data in data["curves"]:
            curve = action.fcurves.new(curve_data["data_path"], index=curve_data["array_index"])
            curve.keyframe_points.add(len(curve_data["keyframes"]) - 1)
            for point, point_data in zip(curve.keyframe_points, curve_data["keyframes"]):
                point.co = point_data["co"]
                point.interpolation = point_data["interpolation"]
                point.easing = point_data["easing"]
            curve.update()
        created.append(action)

    return created


def bake_single_action(blend_path, object_names, armature_name, action_name):
    bpy.ops.wm.open_mainfile(filepath=blend_path)
    keep_objects(object_names)

    armature = bpy.data.objects.get(armature_name)
    if armature is None:
        raise RuntimeError(f"Missing armature: {armature_name}")

    source_action = bpy.data.actions.get(action_name)
    if source_action is None:
        raise RuntimeError(f"Missing action: {action_name}")

    bpy.context.view_layer.objects.active = armature
    armature.select_set(True)
    bpy.ops.object.mode_set(mode="POSE")
    for pose_bone in armature.pose.bones:
        pose_bone.bone.select = True

    start, end = source_action.frame_range
    armature.animation_data_create()
    armature.animation_data.action = source_action
    bpy.context.scene.frame_set(int(start))
    bpy.context.view_layer.update()

    bpy.ops.nla.bake(
        frame_start=int(start),
        frame_end=int(end),
        step=1,
        only_selected=False,
        visual_keying=True,
        clear_constraints=True,
        clear_parents=False,
        use_current_action=False,
        clean_curves=False,
        bake_types={"POSE"},
    )
    bpy.ops.object.mode_set(mode="OBJECT")

    baked_action = armature.animation_data.action
    return serialize_action(baked_action, action_name)


def action_names_for_prefix(blend_path, action_prefix):
    bpy.ops.wm.open_mainfile(filepath=blend_path)
    return [action.name for action in bpy.data.actions if action.name.startswith(action_prefix)]


def bake_action_set(blend_path, object_names, armature_name, action_prefix):
    action_names = action_names_for_prefix(blend_path, action_prefix)
    if not action_names:
        raise RuntimeError(f"Missing action prefix: {action_prefix}")

    return [
        bake_single_action(blend_path, object_names, armature_name, action_name)
        for action_name in action_names
    ]


def select_only(names):
    for obj in bpy.data.objects:
        obj.select_set(False)

    selected = []
    for name in names:
        obj = bpy.data.objects.get(name)
        if obj is None:
            raise RuntimeError(f"Missing object: {name}")
        obj.select_set(True)
        selected.append(obj)

    bpy.context.view_layer.objects.active = selected[0]


def keep_objects(names):
    keep = set(names)
    for obj in list(bpy.data.objects):
        if obj.name not in keep:
            bpy.data.objects.remove(obj, do_unlink=True)


def export_fbx(filepath):
    bpy.ops.export_scene.fbx(
        filepath=filepath,
        use_selection=False,
        object_types={"ARMATURE", "MESH"},
        axis_forward="-Z",
        axis_up="Y",
        global_scale=1.0,
        apply_unit_scale=True,
        apply_scale_options="FBX_SCALE_ALL",
        add_leaf_bones=False,
        bake_anim=True,
        bake_anim_use_all_bones=True,
        bake_anim_use_nla_strips=False,
        bake_anim_use_all_actions=True,
        bake_anim_force_startend_keying=True,
        bake_anim_step=1.0,
        bake_anim_simplify_factor=0.0,
        use_mesh_modifiers=True,
        mesh_smooth_type="OFF",
        use_custom_props=False,
    )


def sanitize_filename(value):
    return "".join(character if character.isalnum() or character in "_-" else "_" for character in value)


def export_single_baked_action(blend_path, object_names, armature_name, action_name, filepath):
    bpy.ops.wm.open_mainfile(filepath=blend_path)
    keep_objects(object_names)

    armature = bpy.data.objects.get(armature_name)
    if armature is None:
        raise RuntimeError(f"Missing armature: {armature_name}")

    source_action = bpy.data.actions.get(action_name)
    if source_action is None:
        raise RuntimeError(f"Missing action: {action_name}")

    armature.animation_data_create()
    armature.animation_data.action = source_action

    for obj in bpy.data.objects:
        obj.select_set(False)
    armature.select_set(True)
    bpy.context.view_layer.objects.active = armature

    bpy.ops.object.mode_set(mode="POSE")
    for pose_bone in armature.pose.bones:
        pose_bone.bone.select = True

    start, end = source_action.frame_range
    bpy.context.scene.frame_set(int(start))
    bpy.context.view_layer.update()
    bpy.ops.nla.bake(
        frame_start=int(start),
        frame_end=int(end),
        step=1,
        only_selected=False,
        visual_keying=True,
        clear_constraints=True,
        clear_parents=False,
        use_current_action=False,
        clean_curves=False,
        bake_types={"POSE"},
    )
    bpy.ops.object.mode_set(mode="OBJECT")

    baked_action = armature.animation_data.action
    baked_action.name = action_name
    for action in list(bpy.data.actions):
        if action != baked_action:
            bpy.data.actions.remove(action)

    select_only(object_names)
    export_fbx(filepath)


def export_action_files(blend_path, action_prefix, object_names, armature_name, base_name, outdir):
    action_names = action_names_for_prefix(blend_path, action_prefix)
    if not action_names:
        raise RuntimeError(f"Missing action prefix: {action_prefix}")

    base_action = next((name for name in action_names if "idle_pose" in name.lower()), action_names[0])
    export_single_baked_action(
        blend_path,
        object_names,
        armature_name,
        base_action,
        os.path.join(outdir, f"{base_name}.fbx"),
    )

    for action_name in action_names:
        export_single_baked_action(
            blend_path,
            object_names,
            armature_name,
            action_name,
            os.path.join(outdir, f"{base_name}_{sanitize_filename(action_name)}.fbx"),
        )


def assign_first_action(armature_name, action_prefix):
    armature = bpy.data.objects.get(armature_name)
    if armature is None:
        raise RuntimeError(f"Missing armature: {armature_name}")

    action = next((candidate for candidate in bpy.data.actions if candidate.name.startswith(action_prefix)), None)
    if action is None:
        raise RuntimeError(f"Missing action prefix: {action_prefix}")

    armature.animation_data_create()
    armature.animation_data.action = action


def export_set(blend_path, action_prefix, object_names, armature_name, filepath):
    baked_action_data = bake_action_set(blend_path, object_names, armature_name, action_prefix)

    bpy.ops.wm.open_mainfile(filepath=blend_path)
    keep_objects(object_names)
    for action in list(bpy.data.actions):
        bpy.data.actions.remove(action)

    baked_actions = recreate_actions(baked_action_data)
    armature = bpy.data.objects[armature_name]
    remove_constraints(armature)
    armature.animation_data_create()
    armature.animation_data.action = baked_actions[0]
    select_only(object_names)
    export_fbx(filepath)


def main():
    args = parse_args()
    blend_path = os.path.abspath(args.blend)
    outdir = os.path.abspath(args.outdir)
    os.makedirs(outdir, exist_ok=True)

    export_action_files(
        blend_path,
        "FP_",
        ["Arms_Rig", "arms"],
        "Arms_Rig",
        "CreatorClean_Arms",
        outdir,
    )
    export_action_files(
        blend_path,
        "GUN_",
        ["Gun_Rig", "gun"],
        "Gun_Rig",
        "CreatorClean_Gun",
        outdir,
    )


if __name__ == "__main__":
    main()
