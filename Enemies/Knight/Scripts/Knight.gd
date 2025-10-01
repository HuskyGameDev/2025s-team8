class_name Knight extends Enemy

var move_speed : float = 100
var wander_dir_timer : float = 0
var randAngle = randf() * TAU
var chase_player : bool = false
var direction : Vector2 = Vector2.ZERO
const DIR_4 = [Vector2.RIGHT, Vector2.DOWN, Vector2.LEFT, Vector2.UP]
var cardinal_direction : Vector2 = Vector2.RIGHT

var knockback : float = 10

@export var hurt_box: Hurtbox
#@onready var mc: Player = $"."

signal direction_changed( new_direction : Vector2)
signal slime_damaged(hurt_box: Hurtbox)

var invulnerable : bool = false


@onready var animation_player : AnimationPlayer = $AnimationPlayer
@onready var sprite : Sprite2D = $Sprite2D

@onready var knight_state_machine: KnightStateMachine = $KnightStateMachine



# Called when the node enters the scene tree for the first time.


func _physics_process(_delta: float):
	move_and_slide()

#version of set direction but it doesnt include up or down
func set_direction(_new_direction: Vector2) -> bool:
	direction = _new_direction
	var direction_id : int = int(round(
		(direction + cardinal_direction * 0.1).angle()
		/ TAU * DIR_4.size()
	))
	var new_dir = DIR_4[direction_id]
	#if new_dir == Vector2.UP:
	#	return false
	#if new_dir == Vector2.DOWN:
	#	return false
	if new_dir == cardinal_direction:
		return false
		
	cardinal_direction = new_dir
	direction_changed.emit(new_dir)
	sprite.scale.x = -1 if cardinal_direction == Vector2.LEFT else 1
	return true


func _on_detection_area_body_entered(_body: Node2D) -> void:
	player = _body
	chase_player = true
	pass # Replace with function body.

#function to change update the animation
func UpdateAnimation(state: String) -> void: 
	animation_player.play(state)
	#print(state + "_" + AnimDirection() )
	pass

func anim_direction() -> String:
	return "side"
	
func _on_detection_area_body_exited(_body: Node2D) -> void:
	player = null
	chase_player = false

func _ready():
	knight_state_machine.initialize(self)
	$HitBox.Damaged.connect( TakeDamage )
	
#
#func TakeDamage(hurt_box:Hurtbox) -> void:
#	hp = hp - hurt_box.damage #underscore means optional
#	#queue_free()
#	if hp > 0:
#		slime_damaged.emit(hurt_box)
#	else:
#		queue_free() #just get rid of the slime if below 0
#	pass
#
