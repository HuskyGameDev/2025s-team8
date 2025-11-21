class_name SlimeEnemy extends Enemy


var move_speed : float = 50
var wander_dir_timer : float = 0
var randAngle = randf() * TAU
var leap_timer: float = 0
@onready var animation_player : AnimationPlayer = $AnimationPlayer
@onready var nav: NavigationAgent2D = $NavigationAgent2D

@export var hurt_box: Hurtbox




func _physics_process(delta: float):
	if dead: return;
	if player_found:
		chase(player.position - position, delta)
		pass
	else:
		wander(delta)
		
	move_and_slide()

func wander(delta : float):
	hurt_box.monitoring = false
	
	if wander_dir_timer <= 0:
		randAngle = randf() * TAU
		wander_dir_timer = 1
	else:
		wander_dir_timer -= delta
		
	var randDir = Vector2.RIGHT.rotated(randAngle)
	var t = Time.get_ticks_msec() / 1000.0
	velocity = randDir * move_speed * 0.5 * randf() * (1 + 1 * sin(t * 0.00000001))


func chase(dif, delta) -> void:
	var distance = dif.length()
	var leap_force = 1
	
	leap_timer -= delta
	
	if(leap_timer < 1):
		leap_force = clampf(5 * (1 - leap_timer), 0, 4) # stop and then speed up, but stay at 4
		position += Vector2(randf_range(-1,1),randf_range(-1,1)) # shake
		hurt_box.monitoring = true

		if(leap_timer <= 0): # stop leaping
			leap_timer = 3 + (randf() * 2) # wait 3 to 5 seconds
			leap_force = 1
			hurt_box.monitoring = false

	if(distance > 50 or leap_force > 1):
		velocity = dif.normalized() * move_speed * leap_force
		nav.target_position = player.position
		if(nav.is_navigation_finished() == false):
			var navDir = (nav.get_next_path_position() - position).normalized()
			velocity = navDir * move_speed * leap_force
	elif(distance < 40): # too close, back up!
		velocity = -dif.normalized() * move_speed
	else:
		velocity = Vector2.ZERO
		

func _on_detection_area_body_entered(_body: Node2D) -> void:
	if(player == null and _body is Player):
		player = _body
		player_found = true
		
		
#func _on_detection_area_body_exited(_body: Node2D) -> void:
	#if(_body is Player):
		#player = null
		#chase_player = false	

#function to change update the animation
func UpdateAnimation(state: String) -> void:
	animation_player.play("idle")
	#print(state + "_" + AnimDirection() )
	pass
