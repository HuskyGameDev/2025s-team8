extends Node


var current_tilemap_bounds: Array[Vector2]
signal tilemap_bound_changed(bounds : Array[Vector2] )

func change_tilemap_bounds(bounds : Array[Vector2] )->void:
	current_tilemap_bounds = bounds
	tilemap_bound_changed.emit(bounds)
	
	# need to update to godot 4.3
	

#
