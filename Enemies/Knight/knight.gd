class_name Knight extends Enemy

var move_speed : float = 20
var wander_dir_timer : float = 0
var randAngle = randf() * TAU
var leap_timer: float = 0

#direciton stuff
var cardinal_direction : Vector2 = Vector2.DOWN
var direction : Vector2 = Vector2.ZERO

var player_ref : Player

@onready var animation_player : AnimationPlayer = $AnimationPlayer
@onready var sprite : Sprite2D = $Sprite2D
@onready var state_machine: knight_state_machine = $state_machine
@onready var nav: NavigationAgent2D = $NavigationAgent2D

@export var hurt_box: Hurtbox

signal DirectionChanged( _new_direction: Vector2)

func _ready():
	state_machine.initialize(self)
	#reference to the player loaded in may be needed, omitted for now as tracking exists
	
func _process(float) ->void:
	
	if(velocity.x > 0):
		direction.x = 1
	else:
		direction.x = -1
	
	if(velocity.y < 0):
		direction.y = 1
	else:
		direction.y = -1
	direction = direction.normalized();
	pass
	
func _physics_process(delta: float):
	
	if player_found:
		chase(player.position - position, delta)
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
		#velocity = dif.normalized * move_speed * leap_force
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
		
		
#consider uncommenting this
#func _on_detection_area_body_exited(_body: Node2D) -> void:
	#if(_body is Player):
		#player = null
		#chase_player = false	

#ripped from the player in which it will flip the sprite based on direction
func SetDirection() -> bool:
	var new_dir: Vector2 = cardinal_direction
	if direction == Vector2.ZERO:
		return false
	
	if direction.y == 0:
		new_dir = Vector2.LEFT if direction.x < 0 else Vector2.RIGHT
	elif direction.x == 0:
		new_dir = Vector2.UP if direction.y < 0 else Vector2.DOWN

		
	if new_dir == cardinal_direction:
		return false
	
	cardinal_direction = new_dir
	DirectionChanged.emit(new_dir)
	#this swithces the way the player is facing rather than create a new sprite animation
	sprite.scale.x = -1 if cardinal_direction == Vector2.LEFT else 1
	return true

#function to change update the animation
func updateAnimation(state: String) -> void:
	animation_player.play(state + "_" + AnimDirection())
	#print(state + "_" + AnimDirection() )
	pass

func AnimDirection() -> String:
	if cardinal_direction == Vector2.DOWN:
		return "down"
	elif cardinal_direction == Vector2.UP:
		return "up"
	else:
		return "side"
