class_name Projectile extends Area2D


@export var damage: float
var direction: Vector2 = Vector2.RIGHT
@export var speed: float

	
	
	
func _process(delta: float) -> void:
	position += direction * speed * delta
	

func _on_body_entered(a):
	queue_free()


func _on_area_entered(area):
	if area is Hitbox:
		area.TakeDamage(damage)
		print("Damaged a hitbox.")
		queue_free()
