class_name SlimeEnemy extends Node2D

var move_speed : float = 80
var chase_player : bool = false
var player: Node2D = null

# Called when the node enters the scene tree for the first time.
var hp: int = 3

func _physics_process(_delta: float):
	if chase_player:
		position += (player.position - position)/move_speed


func _on_detection_area_body_entered(_body: Node2D) -> void:
	player = _body
	chase_player = true
	pass # Replace with function body.


func _on_detection_area_body_exited(_body: Node2D) -> void:
	player = null
	chase_player = false
	
	
func _ready():
	$HitBox.Damaged.connect( TakeDamage )
	
func TakeDamage(_damage:int) -> void:
	hp = hp - _damage#underscore means optional
	print("took damage")
	#queue_free()
	if hp <= 0:
		queue_free()
	pass
