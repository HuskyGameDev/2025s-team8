class_name ItemData
extends Resource

enum Type {ARMOR, WEAPON, RANDOM, MAIN}

@export var type: Type
@export var name: String
@export var damage: int
@export var defense: int
@export_multiline var description: String
@export var texture: Texture2D
