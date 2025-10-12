class_name Hurtbox extends Area2D


signal received_damage(damage: int) #any time damage is recieved
@export var damage : int = 1

#@export var health: Health

<<<<<<< HEAD
func _init() -> void:
	pass
=======
#func _init() -> void:
#	collision_layer = 0
#	collision_mask = 3
>>>>>>> origin/origin/Derek
	
	
# .
func _ready() -> void:
	area_entered.connect(_on_area_entered)

# .
func _on_area_entered(a : Area2D) -> void:
	if a is Hitbox:
<<<<<<< HEAD
		a.TakeDamage(self)

		#health.health -= hitbox.damage
=======
		a.TakeDamage(damage)
		print("Did " + str(damage) + " to " + a.name)
>>>>>>> origin/origin/Derek
		
