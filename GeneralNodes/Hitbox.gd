class_name Hitbox
extends Area2D


@export var damage: int = 1: set = set_damage, get = get_damage
# note by bryson, the collisions of the walls have been painted to the id of 2. 
# change this collision layer or repaint the wall to avoid further issues
# set collision layer.
func _init() -> void:
	collision_layer = 2
	collision_mask = 0


func set_damage(value: int):
	damage = value
	
func get_damage() -> int:
	return damage
	
