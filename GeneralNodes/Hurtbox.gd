class_name Hurtbox
extends Area2D

#signal that emits an integer damage when the hurtbox takes damage
signal received_damage(damage: int)

@export var health: Health

func _init() -> void:
	collision_layer = 0
	#Only areas or physics bodies in layer 2 will trigger the area_entered signal.
	
	collision_mask = 10
	
	
# .
func _ready() -> void:
	connect("area_entered", _on_area_entered)


# .
func _on_area_entered(hitbox: Hitbox) -> void:
	if hitbox != null:
		#health.health -= hitbox.damage
		received_damage.emit(hitbox.damage)
