class_name state_stun extends State


@export var knockback_speed: float = 20.0
@export var decelerate_speed: float = 10.0
@export var invulnerable_duration: float = 1.0

var hurt_box: Hurtbox
var direction: Vector2

@onready var idle : State = $"../Idle"

var next_state : State = null

func init() -> void:
	player.player_damaged.connect(playerDamaged) #connected to the script player and do player damaged

func Enter() -> void:
	player.UpdateAnimation("stun")
	player.animation_player.animation_finished.connect(animationFinished)
	
	#knockback here
	direction = player.global_position.direction_to(hurt_box.global_position)
	player.velocity = direction *-knockback_speed
	player.SetDirection()
	
	#Invincible state
	player.MakeInvulnerable(invulnerable_duration)
	#player.effect_player.play("damaged")
	
	pass
	
func Exit() -> void:
	next_state = null
	player.animation_player.animation_finished.disconnect(animationFinished)
	pass


func Process(_delta : float) -> State:
	
	return next_state


func Physics(_delta: float) -> State:
	return null


func HandleInput(_event: InputEvent) -> State:
	#repeat code from idle for attack

	return null

func playerDamaged(_hurt_box: Hurtbox) -> void:
	hurt_box = _hurt_box
	state_machine.ChangeState(self)
	pass
	
func animationFinished(_a: String) -> void:
	next_state = idle
	
