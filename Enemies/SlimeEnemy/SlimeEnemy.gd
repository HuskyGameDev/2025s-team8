class_name SlimeEnemy extends Enemy


var move_speed : float = 100
var wander_dir_timer : float = 0
var randAngle = randf() * TAU

@onready var animation_player : AnimationPlayer = $AnimationPlayer
@onready var sprite : Sprite2D = $Sprite2D


func _physics_process(delta: float):
	if chase_player:
		chase(player.position - position)
	else:
		wander(delta)
		
	move_and_slide()

func wander(delta : float):
	if wander_dir_timer <= 0:
		randAngle = randf() * TAU
		wander_dir_timer = 1
	else:
		wander_dir_timer -= delta
		
	var randDir = Vector2.RIGHT.rotated(randAngle)
	var t = Time.get_ticks_msec() / 1000.0
	velocity = randDir * move_speed * 0.5 * randf() * (1 + 1 * sin(t * 0.00000001))


func chase(dif) -> void:
	var distance = dif.length()
	if(distance > 50):
		velocity = dif.normalized() * move_speed
	elif(distance < 40): # too close, back up!
		velocity = -dif.normalized() * move_speed
	else:
		velocity = Vector2.ZERO
		

func _on_detection_area_body_entered(_body: Node2D) -> void:
	if(player == null and _body is Player):
		player = _body
		chase_player = true
		
		
func _on_detection_area_body_exited(_body: Node2D) -> void:
	if(_body is Player):
		player = null
		chase_player = false	

#function to change update the animation
func UpdateAnimation(state: String) -> void:
	animation_player.play("idle")
	#print(state + "_" + AnimDirection() )
	pass
