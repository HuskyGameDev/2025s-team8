extends TextureProgressBar

@export var health: Health

func _ready():
	health.health_changed.connect(update)
	update()



func update():
	value = health.get_health()
