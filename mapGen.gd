extends Node2D



var map = [];
var length: int = 500;
@onready var tilemap = $"../Dungeon01"

var lastWanderDir: Vector2 = Vector2(1,1)

func _ready():
	for y in range(length):
		var row = []
		for x in range(length):
			row.append(1)
		map.append(row);
	
	
	var startPos = Vector2(2,2)
	var endPos = Vector2(490,200)
	
	map[startPos.y][startPos.x] = 5
	map[endPos.y][endPos.x] = 5
	
	var pointer = startPos
	var approach_timer: int = 0
	
	while(pointer != endPos):
		if(approach_timer > 0):
			pointer += wander()
			approach_timer -= 1
		else:
			pointer += approach(pointer,endPos)
			approach_timer = 9

		
		if(pointer != endPos):
			if(pointer.y >= length):
				pointer.y -= 1
			if(pointer.y < 0):
				pointer.y += 1
			if(pointer.x >= length):
				pointer.x -= 1
			if(pointer.x < 0):
				pointer.x += 1
			
			# paint a 5x5 of air
			for y in range(5):
				for x in range(5):
					try_carve(pointer + Vector2(x-3,y-3))
	
	# print the matrix
	#for y in range(length):
	#	print(map[y])
		
	for y in range(length):
		for x in range(length):
			if(map[y][x] == 1):
				tilemap.set_cell(Vector2i(x, y), 0, Vector2i(1, 6))
			else:
				tilemap.set_cell(Vector2i(x, y), 0, Vector2i(2, 2))
		
func try_carve(pos: Vector2):
	if(pos.x >= length or pos.x < 0):
		return
	if(pos.y >= length or pos.y < 0):
		return
	map[pos.y][pos.x] = 0
	
	
func approach(pos: Vector2, endPos: Vector2):
	var dif = endPos-pos;
	dif = Vector2(clamp(dif.x, -1,1), clamp(dif.y, -1,1));
	return dif
	
func wander():
	var dir;
	if(randi_range(0,1) == 0): # chance to have momentum
		dir = lastWanderDir
	else:
		dir = Vector2(randi_range(-1,1), randi_range(-1,1))
		lastWanderDir = dir
		
		
	return dir
	
	
	
