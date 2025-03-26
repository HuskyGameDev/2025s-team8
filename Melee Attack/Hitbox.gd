class_name Hitbox
extends Area2D

#make variable editable, exported variable damage of type int, with a default value of 1
@export var damage: int = 1: set = set_damage, get = get_damage

# set collision layer.
func _init() -> void:
	#Only objects whose collision masks include layer 2 can interact with this hitbox.
	collision_layer = 2
	collision_mask = 0


func set_damage(value: int):
	damage = value
	
func get_damage() -> int:
	return damage
	
