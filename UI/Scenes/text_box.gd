extends Control

# NOTE: Still need to create solution for text larger than box (reduce font size, scrolling, or others)
# NOTE: Player cannot attack while text is up for some reason.
#NOTE: This will include code specifically for the tutorial pacing, removing this scripts ability to be reused
# There is most likely a way to change that if the script is needed later.

@export var texts: Array[String] = []
@export var speed := 0.1 # seconds per character
@export var delay := 3 # Determines how many seconds after the scene starts, the text box will appear

var cur_ind = 0			# The current string in the array
var waiting = false		# If waiting for user input
var typing = false		# If tween animation is running
var full_text = ""		# The string currently being displayed
var char_index = 0		# Index within the displaying string

@onready var door1 = %door1
@onready var door2 = %door2

@onready var label = $Panel/RichTextLabel
@onready var tween = create_tween()  # Create a tween dynamically
# Tween just actually gradually increases char_index so process knows how much to show

# Hide is there only for if there is no text
func _ready() -> void:
	hide()
	# Ensures the textbox fill the top of the screen no matter screen size (Broken due to player)
	size = get_viewport_rect().size
	await get_tree().create_timer(delay).timeout
	start_text(texts)

func start_text(new_text: Array[String]):
	# Need to double check but may just be able to use new_text rather than a new var
	texts = new_text
	cur_ind = 0
	if texts.size() > 0:
		show()
		show_text(texts[0])
	else:
		hide()

func show_text(text: String):
	# Stop any active tween
	if tween:
		tween.kill()
	
	if cur_ind == 2:
		door1.queue_free()
	
	if cur_ind == 11:
		door2.queue_free()

	full_text = text
	char_index = 0
	typing = true
	waiting = false
	label.text = ""

	# Tween char_index from 0 to full_text.length
	tween = create_tween()
	tween.tween_property(self, "char_index", full_text.length(), full_text.length() * speed)
	tween.set_trans(Tween.TRANS_LINEAR).set_ease(Tween.EASE_IN_OUT)
	tween.connect("finished", Callable(self, "_on_tween_finished"))

func _on_tween_finished():
	typing = false
	waiting = true
	label.text = full_text  # Ensure full text shown at the end

func _process(delta):
	if typing:
		# Update label text to current char_index
		var chars_to_show = int(char_index)
		label.text = full_text.substr(0, chars_to_show)

func _input(event: InputEvent):
	# Event can be changed from ui_accept (Enter) if needed
	if event.is_action_released("ui_accept"):
		if typing:
			# Skip typing effect
			tween.kill()
			label.text = full_text
			typing = false
			waiting = true
		elif waiting:
			cur_ind += 1
			if cur_ind < texts.size():
				show_text(texts[cur_ind])
			else:
				hide()
				waiting = false
