class_name Hurtbox
extends Area2D


signal received_damage(damage: int)
@export var damage : int = 1
#@export var health: Health

func _init() -> void:
	collision_layer = 0
	collision_mask = 3
	
	
# .
func _ready() -> void:
	area_entered.connect(_on_area_entered)


# .
func _on_area_entered(a : Area2D) -> void:
	if a is Hitbox:
		a.TakeDamage(damage)
		#health.health -= hitbox.damage
		
