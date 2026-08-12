import bpy

SOURCE_ACTIONS = {
    "Idle": {
        "Arms_Rig": "FP_idle_pose",
        "Gun_Rig": "GUN_idle",
    },
    "Fire": {
        "Arms_Rig": "FP_fire",
        "Gun_Rig": "GUN_fire",
    },
    "Reload": {
        "Arms_Rig": "FP_reload",
        "Gun_Rig": "GUN_reload",
    },
    "Walk": {
        "Arms_Rig": "FP_idle_pose",
        "Gun_Rig": "GUN_walk",
    },
    "RefPose": {
        "Arms_Rig": "FP_ref_pose",
        "Gun_Rig": "GUN_ref_pose",
    },
}

EXPORT_PATH = r"C:\unity\Portfolio-Filling\Assets\Resource\Model\Weapon\CreatorBakedTest\Creator_Baked.fbx"


def clear_animation_data(armature):
    armature.animation_data_create()
    armature.animation_data.action = None
    for track in list(armature.animation_data.nla_tracks):
        armature.animation_data.nla_tracks.remove(track)


def add_strip(armature, take_name, action_name):
    action = bpy.data.actions.get(action_name)
    if action is None:
        raise RuntimeError(f"Missing action: {action_name}")

    track = armature.animation_data.nla_tracks.new()
    track.name = take_name
    strip = track.strips.new(take_name, 0, action)
    strip.name = take_name
    strip.action_frame_start = action.frame_range[0]
    strip.action_frame_end = action.frame_range[1]
    strip.frame_start = 0
    strip.frame_end = action.frame_range[1] - action.frame_range[0]
    strip.use_animated_time = False
    strip.scale = 1
    return strip


for rig_name in ("Arms_Rig", "Gun_Rig"):
    rig = bpy.data.objects.get(rig_name)
    if rig is None:
        raise RuntimeError(f"Missing rig: {rig_name}")

    clear_animation_data(rig)
    for take_name, rig_actions in SOURCE_ACTIONS.items():
        add_strip(rig, take_name, rig_actions[rig_name])

for obj in bpy.context.scene.objects:
    obj.select_set(obj.type in {"ARMATURE", "MESH", "EMPTY", "CAMERA"})

bpy.ops.export_scene.fbx(
    filepath=EXPORT_PATH,
    use_selection=True,
    add_leaf_bones=False,
    bake_anim=True,
    bake_anim_use_all_actions=False,
    bake_anim_use_nla_strips=True,
    bake_anim_step=1.0,
    bake_anim_simplify_factor=0.0,
    object_types={"ARMATURE", "MESH", "EMPTY", "CAMERA"},
    path_mode="AUTO",
)
