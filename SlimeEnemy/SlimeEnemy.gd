class_name SlimeEnemy extends Enemy


var move_speed : float = 100

@onready var animation_player : AnimationPlayer = $AnimationPlayer
@onready var sprite : Sprite2D = $Sprite2D

# Called when the node enters the scene tree for the first time.
var hp: int = 3

func _physics_process(_delta: float):
	if chase_player:
		chase(player.position - position)
		
	move_and_slide()

func chase(dif) -> void:
	var distance = dif.length()
	if(distance > 50):
		velocity = dif.normalized() * move_speed
	elif(distance < 40): # too close, back up!
		velocity = -dif.normalized() * move_speed

func _on_detection_area_body_entered(_body: Node2D) -> void:
	if(player == null and _body is Player):
		player = _body
		chase_player = true

#function to change update the animation
func UpdateAnimation(state: String) -> void:
	animation_player.play("idle")
	#print(state + "_" + AnimDirection() )
	pass
	
func _on_detection_area_body_exited(_body: Node2D) -> void:
	if(_body is Player):
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
