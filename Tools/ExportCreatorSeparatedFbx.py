import bpy

OUTPUTS = [
    {
        "path": r"C:\unity\Portfolio-Filling\Assets\Resource\Model\Weapon\CreatorSeparated\Creator_Arms.fbx",
        "rig": "Arms_Rig",
        "objects": {"Arms_Rig", "arms", "FP_Camera"},
        "action_prefix": "FP_",
    },
    {
        "path": r"C:\unity\Portfolio-Filling\Assets\Resource\Model\Weapon\CreatorSeparated\Creator_Gun.fbx",
        "rig": "Gun_Rig",
        "objects": {"Gun_Rig", "gun"},
        "action_prefix": "GUN_",
    },
]


def prepare_rig_actions(rig_name, action_prefix):
    rig = bpy.data.objects.get(rig_name)
    if rig is None:
        raise RuntimeError(f"Missing rig: {rig_name}")

    rig.animation_data_create()
    rig.animation_data.action = None
    for track in list(rig.animation_data.nla_tracks):
        rig.animation_data.nla_tracks.remove(track)

    for action in bpy.data.actions:
        if not action.name.startswith(action_prefix):
            continue

        take_name = action.name[len(action_prefix):]
        if take_name == "":
            take_name = action.name

        track = rig.animation_data.nla_tracks.new()
        track.name = take_name
        strip = track.strips.new(take_name, 0, action)
        strip.name = take_name
        strip.action_frame_start = action.frame_range[0]
        strip.action_frame_end = action.frame_range[1]
        strip.frame_start = 0
        strip.frame_end = action.frame_range[1] - action.frame_range[0]
        strip.scale = 1


for output in OUTPUTS:
    prepare_rig_actions(output["rig"], output["action_prefix"])

    for obj in bpy.context.scene.objects:
        obj.select_set(obj.name in output["objects"])

    bpy.ops.export_scene.fbx(
        filepath=output["path"],
        use_selection=True,
        add_leaf_bones=False,
        bake_anim=True,
        bake_anim_use_all_actions=False,
        bake_anim_use_nla_strips=True,
        bake_anim_step=1.0,
        bake_anim_simplify_factor=0.0,
        object_types={"ARMATURE", "MESH", "CAMERA"},
        path_mode="AUTO",
    )
