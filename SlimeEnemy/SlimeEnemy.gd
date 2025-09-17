class_name SlimeEnemy extends CharacterBody2D

var move_speed : float = 80
var chase_player : bool = false
var player: Node2D = null
var direction : Vector2 = Vector2.ZERO
var knockback : float = 10
var leap_timer: float = 0
@export var hurt_box: Hurtbox
#@onready var mc: Player = $"."

var hp: int = 3

signal slime_damaged(hurt_box: Hurtbox)

@onready var animation_player : AnimationPlayer = $AnimationPlayer
@onready var sprite : Sprite2D = $Sprite2D
@onready var nav: NavigationAgent2D = $NavigationAgent2D
# Called when the node enters the scene tree for the first time.

#This code is not mine it was made by Derek
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



func _physics_process(_delta: float):
	if chase_player:
		position +=(player.position - position)/move_speed
	move_and_slide()

#	if player_found:
#		chase(player.position - position, delta)
#	else:
#		wander(delta)
#	move_and_slide()




func _on_detection_area_body_entered(_body: Node2D) -> void:
	player = _body
	chase_player = true
	pass # Replace with function body.

#function to change update the animation
func UpdateAnimation(state: String) -> void: 
	animation_player.play("idle")
	#print(state + "_" + AnimDirection() )
	pass
	
func _on_detection_area_body_exited(_body: Node2D) -> void:
	player = null
	chase_player = false
	
	
func _ready():
	$HitBox.Damaged.connect( TakeDamage )
	
	
func TakeDamage(hurt_box:Hurtbox) -> void:
	hp = hp - hurt_box.damage #underscore means optional
	#queue_free()
	if hp > 0:
		slime_damaged.emit(hurt_box)
	else:
		queue_free() #just get rid of the slime if below 0
	pass
