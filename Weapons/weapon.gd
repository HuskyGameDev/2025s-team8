class_name Weapon extends Node2D


var attacking: bool = false
var image: Image
@export var cooldown : float = 0.25
@export var freeze_duration : float = 0.1

# meant to be overridden
func attack() -> void:
	pass
