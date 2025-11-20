class_name state_stun extends State


@export var knockback_speed: float = 100.0
@export var decelerate_speed: float = 10.0
@export var invulnerable_duration: float = 1.0


#the hurtbox right here is an old reference and probably needs to be updated
var hurt_box: Hurtbox
var direction: Vector2

@onready var idle : State = $"../Idle"

#@onready var animation_player: AnimationPlayer = $"../../AnimationPlayer"

var next_state : State = null

func init() -> void:
	player.player_damaged.connect(playerDamaged) #connected to the script player and do player damaged

func Enter() -> void:
	player.invulnerable = true;
	player.animation_player.animation_finished.connect(animationFinished)
	#note after updating the animaton, the player does not know how to leave
	#this is regardless of the animation playerd
	player.UpdateAnimation("stun")

	#knockback here
	direction = player.global_position.direction_to(hurt_box.global_position)
	player.velocity = direction *-knockback_speed
	player.SetDirection()
	
	#Invincible state
	player.MakeInvulnerable(invulnerable_duration)
	#await get_tree().create_timer(1.0).timeout
	#player.effect_player.play("damaged")
	#animation_player.animation_finished.disconnect(animationFinished)
	#player.animation_player.animation_finished.disconnect(animationFinished)
	player.invulnerable = false;
	pass
	
func Exit() -> void:
	
	next_state = null
	player.animation_player.animation_finished.disconnect(animationFinished)
	print("animation has finished")
	pass


func Process(_delta : float) -> State:
	
	#if player.invulnerable == true:
	#	return idle
		
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
	print("I am notified that the animation has started")
	next_state = idle
	
	
