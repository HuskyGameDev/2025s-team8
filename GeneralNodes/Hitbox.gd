class_name Hitbox extends Area2D

signal Damaged(hurt_box : Hurtbox)

#DO NOT CHANGE DAMAGE IN THE CODE USE THE INSPECTOR PROPERTY PLEASE
@export var damage: int = 1: set = set_damage, get = get_damage
# note by bryson, the collisions of the walls have been painted to the id of 2. 
# change this collision layer or repaint the wall to avoid further issues
# set collision layer.
func _init() -> void:
	pass


func set_damage(value: int):
	damage = value
	
func get_damage() -> int:
	return damage
	
func TakeDamage( hurt_box : Hurtbox) -> void:
	Damaged.emit(hurt_box)
