extends Control

#The array of text for each scene
@export var texts: Array[String] = []
var cur_ind = 0
var waiting = false

@onready var label = $Panel/RichTextLabel


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	hide()
	start_text(texts)


func start_text(newText: Array[String]):
	texts = newText
	cur_ind = 0
	if texts.size() > 0:
		show()
		show_text(newText[0])
	else:
		hide()

func show_text(text: String):
	label.text = text
	waiting = true

func _input(event: InputEvent):
	if waiting and event.is_action_released("ui_accept"):
		cur_ind += 1
		if cur_ind < texts.size():
			show_text(texts[cur_ind])
		else:
			hide()
			waiting = false

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
