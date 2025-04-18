class_name Health
extends Node

signal max_health_changed(diff: int)
signal health_changed(diff: int)
#emitted when health reaches zero
signal health_depleted

@export var max_health: int =3 : set = set_max_health, get = get_max_health
@export var invincible: bool = false : set = set_invincible, get = get_invincible

var invincible_timer: Timer = null

@onready var health: int = max_health : set = set_health, get = get_health

func set_max_health(value: int):
	var clamped_value = 1 if value <= 0 else value
	
	if not clamped_value == max_health:
		var difference = clamped_value - max_health
		max_health = value
		max_health_changed.emit(difference)
		
		if health > max_health:
			health = max_health
	
func get_max_health() -> int:
	return max_health
	
func set_invincible(value: bool):
	invincible = value
	
func get_invincible() -> bool:
	return invincible
	
func set_health(value: int):
	if value < health and invincible:
		return
		
	var clamped_value = clampi(value, 0, max_health)
	
	if clamped_value != health:
		var difference = clamped_value - health
		health = value
		health_changed.emit(difference)
		
		if health == 0:
			health_depleted.emit()

func get_health():
	return health
