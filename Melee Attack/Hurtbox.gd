class_name Hurtbox
extends Area2D


signal received_damage(damage: int)

#@export var health: Health

func _init() -> void:
	collision_layer = 0
	collision_mask = 2
	
	
# .
func _ready() -> void:
	connect("area_entered", _on_area_entered)


# .
func _on_area_entered(hitbox: Hitbox) -> void:
	if hitbox != null:
		#health.health -= hitbox.damage
		received_damage.emit(hitbox.damage)
