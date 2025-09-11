class_name Projectile extends Area2D


@export var damage: float
var direction: Vector2 = Vector2.RIGHT
@export var speed: float

	
	
	
func _process(delta: float) -> void:
	position += direction * speed * delta
	

func _on_body_entered(a):
	queue_free()
