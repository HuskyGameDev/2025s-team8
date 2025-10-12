extends Enemy


var move_speed : float = 40
var wander_dir_timer : float = 0
var randAngle = randf() * TAU

@export var projectile_scene: PackedScene
@export var shoot_delay: float = 1.5
var shoot_timer: float = shoot_delay

@onready var nav: NavigationAgent2D = $NavigationAgent2D


func _physics_process(delta: float):
	if player_found:
		chase(player.position - position)
		try_shoot(delta)
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
	if(distance > 120):
		#velocity = dif.normalized() * move_speed
		nav.target_position = player.position
		if(nav.is_navigation_finished() == false):
			var navDir = (nav.get_next_path_position() - position).normalized()
			velocity = navDir * move_speed
	elif(distance < 90): # too close, back up!
		velocity = -dif.normalized() * move_speed
	else:
		velocity = Vector2.ZERO
		
func try_shoot(delta: float):
	if(shoot_timer <= 0):
		shoot_timer = shoot_delay
		shoot()
	else:
		shoot_timer -= delta
		
		
func shoot():
	var projectile = projectile_scene.instantiate() as Projectile
	if(projectile != null):
		get_parent().add_child(projectile)
	
	var dir = (player.position - position).normalized()
	projectile.global_position = global_position + dir
	projectile.global_rotation = global_rotation
	projectile.direction = dir
	
	
	
func _on_detection_area_body_entered(_body: Node2D) -> void:
	if(player == null and _body is Player):
		player = _body
		player_found = true
		
		
#func _on_detection_area_body_exited(_body: Node2D) -> void:
	#if(_body is Player):
		#player = null
		#chase_player = false	
