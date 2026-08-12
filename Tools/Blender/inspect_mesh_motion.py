import argparse
import os
import sys

import bpy
from mathutils import Vector


def parse_args():
    argv = sys.argv
    if "--" in argv:
        argv = argv[argv.index("--") + 1:]
    else:
        argv = []

    parser = argparse.ArgumentParser()
    parser.add_argument("--path", required=True)
    parser.add_argument("--fbx", action="store_true")
    parser.add_argument("--armature", required=True)
    parser.add_argument("--mesh", required=True)
    parser.add_argument("--actions", nargs="+", required=True)
    return parser.parse_args(argv)


def load(path, is_fbx):
    if is_fbx:
        bpy.ops.object.delete()
        bpy.ops.import_scene.fbx(filepath=path)
    else:
        bpy.ops.wm.open_mainfile(filepath=path)


def find_action(names):
    for action in bpy.data.actions:
        if any(name in action.name for name in names):
            return action
    return None


def mesh_bounds(mesh_object):
    depsgraph = bpy.context.evaluated_depsgraph_get()
    evaluated = mesh_object.evaluated_get(depsgraph)
    mesh = evaluated.to_mesh()
    try:
        points = [evaluated.matrix_world @ vertex.co for vertex in mesh.vertices]
    finally:
        evaluated.to_mesh_clear()

    minimum = Vector((min(p.x for p in points), min(p.y for p in points), min(p.z for p in points)))
    maximum = Vector((max(p.x for p in points), max(p.y for p in points), max(p.z for p in points)))
    center = (minimum + maximum) * 0.5
    size = maximum - minimum
    return center, size


def main():
    args = parse_args()
    load(os.path.abspath(args.path), args.fbx)

    armature = bpy.data.objects[args.armature]
    mesh = bpy.data.objects[args.mesh]
    action = find_action(args.actions)
    if action is None:
        raise RuntimeError(f"Missing action: {args.actions}")

    armature.animation_data_create()
    armature.animation_data.action = action
    start, end = action.frame_range
    print("ACTION", action.name, tuple(round(v, 3) for v in action.frame_range), "curves", len(action.fcurves))
    for frame in [int(start), int((start + end) / 4), int((start + end) / 2), int((start + end) * 3 / 4), int(end)]:
        bpy.context.scene.frame_set(frame)
        bpy.context.view_layer.update()
        center, size = mesh_bounds(mesh)
        print(
            "FRAME",
            frame,
            "center",
            tuple(round(v, 4) for v in center),
            "size",
            tuple(round(v, 4) for v in size),
        )


if __name__ == "__main__":
    main()
