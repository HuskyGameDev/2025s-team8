class_name Knight extends Enemy

@onready var animation_player: AnimationPlayer = $AnimationPlayer
@onready var hit_box: Hitbox = $hit_box
@onready var hurt_box: Hurtbox = $hurt_box


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	move_and_slide()
	pass
