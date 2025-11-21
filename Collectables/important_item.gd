extends Area2D

var map_gen

func _on_body_entered(body: Node2D) -> void:
	if(body is Player):
		
		if(map_gen):
			map_gen.create_exit()
		else:
			print("no map gen on item")
		queue_free()
