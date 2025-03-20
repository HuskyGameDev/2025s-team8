class_name ItemData
extends Resource

enum Type {ARMOR, WEAPON, RANDOM, MAIN}
enum Rarity {Common, Uncommon, Rare, Legendary}

@export var type: Type
@export var name: String
@export var damage: int
@export var defense: int
@export var rarity: Rarity
@export var slot_type: Type
@export_multiline var description: String
@export var texture: Texture2D
