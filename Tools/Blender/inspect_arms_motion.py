import argparse
import os
import re
import sys

import bpy


def parse_args():
    argv = sys.argv
    if "--" in argv:
        argv = argv[argv.index("--") + 1:]
    else:
        argv = []

    parser = argparse.ArgumentParser()
    parser.add_argument("--path", required=True)
    parser.add_argument("--fbx", action="store_true")
    return parser.parse_args(argv)


def load(path, is_fbx):
    if is_fbx:
        bpy.ops.object.delete()
        bpy.ops.import_scene.fbx(filepath=path)
    else:
        bpy.ops.wm.open_mainfile(filepath=path)


def keyed_bones(action):
    result = []
    for curve in action.fcurves:
        match = re.search(r'pose\.bones\["(.+?)"\]', curve.data_path)
        if match and match.group(1) not in result:
            result.append(match.group(1))
    return result


def sample_action(armature, action):
    armature.animation_data_create()
    armature.animation_data.action = action
    start, end = action.frame_range
    frames = [int(start), int((start + end) / 2), int(end)]
    bone_names = keyed_bones(action)[:12]

    print("ACTION", action.name, tuple(round(v, 3) for v in action.frame_range), "curves", len(action.fcurves))
    print("KEYED_BONES", bone_names)
    for frame in frames:
        bpy.context.scene.frame_set(frame)
        bpy.context.view_layer.update()
        values = []
        for bone_name in bone_names[:8]:
            pose_bone = armature.pose.bones.get(bone_name)
            if pose_bone is None:
                continue
            values.append((bone_name, tuple(round(v, 4) for v in pose_bone.matrix.translation)))
        print("FRAME", frame, values)


def main():
    args = parse_args()
    load(os.path.abspath(args.path), args.fbx)

    armature = bpy.data.objects.get("Arms_Rig")
    if armature is None:
        raise RuntimeError("Missing Arms_Rig")

    print("BONES", [bone.name for bone in armature.pose.bones])
    for action in bpy.data.actions:
        if "FP_fire" in action.name or "FP_reload" in action.name:
            sample_action(armature, action)


if __name__ == "__main__":
    main()
