class_name ItemData
extends Resource

enum Type {ARMOR, WEAPON, RANDOM, MAIN}
#Treat each rarity as an index in an array for now as that is what they return when used
enum Rarity {Common, Uncommon, Rare, Legendary}

@export var type: Type
@export var name: String
@export var damage: int
@export var defense: int
@export var rarity: Rarity
@export_multiline var description: String
@export var texture: Texture2D
