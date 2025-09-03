extends Label

func _process(_delta):
	text = "Damage: %d\nDefense: %d" % [Player_Stats.dam, Player_Stats.def]
