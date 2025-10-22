extends Node2D



var map = [];
var length: int = 300;
@onready var tilemap = $"../Dungeon01"

var lastWanderDir: Vector2 = Vector2(1,1)

@export var enemyPool : Array[PackedScene] = [];


func _ready():
	
	# set up perlin noise thing
	var noise = FastNoiseLite.new()
	noise.noise_type = FastNoiseLite.TYPE_PERLIN
	noise.seed = randi()
	noise.frequency = 0.04

	# slap a bunch of ones and zeroes into the area based on noise
	for y in range(length):
		var row = []
		for x in range(length):
			var noise_val = (noise.get_noise_2d(x,y) + 1) * 0.5 # gives a value 0-1
			if(noise_val < 0.4):
				row.append(0)
			else:	
				row.append(1)
		map.append(row);
	
	
	var startPos = Vector2(2,2)
	var endPos = Vector2(250,250)
	
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
					
			if(randi_range(0,1000) == -1): # not working currently
				spawn_rand_enemy(Vector2(pointer.x,pointer.y))
	
	
	cellular_automatize()
	
	
	#print the matrix
	#for y in range(length):
	#	print(map[y])	
	
		
	for y in range(length):
		for x in range(length):
			if(map[y][x] == 1):
				tilemap.set_cell(Vector2i(x, y), 0, Vector2i(1, 6)) #wall
			else:
				tilemap.set_cell(Vector2i(x, y), 0, Vector2i(2, 2)) #floor
		
func try_carve(pos: Vector2):
	if(pos.x >= length or pos.x < 0):
		return
	if(pos.y >= length or pos.y < 0):
		return
	map[pos.y][pos.x] = 0
	
	
func spawn_rand_enemy(pos: Vector2):
	var enemy_scene = enemyPool[randi_range(0,enemyPool.size()-1)]
	var new_enemy = enemy_scene.instantiate() as Enemy
	if(new_enemy != null):
		add_child(new_enemy)
		var newPos = pos * 16
		new_enemy.global_position = newPos
		print("spawned enemy at pos", newPos)
	else:
		print("enemy was null")

	
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
	
	
func cellular_automatize():
	for y in range(length):
		for x in range(length):
			try_cell_smooth(Vector2(x,y))
			
			
func try_cell_smooth(pos: Vector2):
	if(pos.x >= length-1 or pos.x <= 0):
		return
	if(pos.y >= length-1 or pos.y <= 0):
		return
	var x = pos.x;
	var y = pos.y;
	var currentVal = map[y][x];
	var unequalCount = 0;
	
	for i in range(3):
		for k in range(3):
			if map[pos.y + i - 1][pos.x + k - 1] != currentVal:
				unequalCount += 1;
			
	if(unequalCount > 4):
		if(currentVal == 0):
			map[y][x] = 1
		else:
			map[y][x] = 0
	
			

	
	
