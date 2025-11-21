extends NavigationRegion2D



func _ready():
	await get_tree().create_timer(3).timeout # wait a bit for map to generate
	bake_navigation_polygon()
