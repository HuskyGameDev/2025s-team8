class_name Hurtbox extends Area2D


signal received_damage(damage: int) #any time damage is recieved
@export var damage : int = 1

#@export var health: Health

func _init() -> void:
	pass
	
	
# .
func _ready() -> void:
	area_entered.connect(_on_area_entered)

# .
func _on_area_entered(a : Area2D) -> void:
	if a is Hitbox:
		a.TakeDamage(self)

		#health.health -= hitbox.damage
		
