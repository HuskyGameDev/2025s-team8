class_name state_attack extends State

#tutorial is using animatino player
#consider switching to time

var attacking: bool = false
var coolingDown: bool = false
@onready var animation_player: AnimationPlayer = $"../../AnimationPlayer"

@onready var idle: State = $"../Idle"
@onready var walk: State = $"../Walk"

@onready var weapons = $"../../interactions/Weapons".get_children()


@export var held_weaponId : int = 0

func Enter() -> void:
	if(coolingDown == true or attacking == true):
		return
	player.UpdateAnimation("attack")
	attacking = true
	
	var held_weapon = weapons[held_weaponId]
	held_weapon.attack()
	
	await get_tree().create_timer(held_weapon.freeze_duration).timeout		
	coolingDown = true;
	EndAttack()
	await get_tree().create_timer(held_weapon.cooldown).timeout # stops player from spamming weapon
	coolingDown = false;
	pass
	
	
func init() ->void:
	pass
	
func Exit() -> void:
	animation_player.animation_finished.disconnect(EndAttack)
	attacking = false
	pass
	
func Process(_delta : float) -> State:
	player.velocity = Vector2.ZERO
	
	if attacking == false:
		if player.direction == Vector2.ZERO:
			return idle
		else:
			return walk
	
	return null
	
func Physics(_delta: float) -> State:
	return null
	
func HandleInput(_event: InputEvent) -> State:
	return null
	
func EndAttack() -> void:
	attacking = false

	
