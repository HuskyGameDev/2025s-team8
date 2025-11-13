class_name MapGenerator extends Node2D

var map = [];
var length: int = 350;
@onready var tilemap = $"../Dungeon01"

var lastWanderDir: Vector2 = Vector2(1,1)

@export var enemyPool : Array[PackedScene] = [];

@export var important_item_scene : PackedScene;
@export var portal_scene : PackedScene; #Not used yet, but will be spawned upon important item collection

@export var player : Node2D;

var currentEnemies : Array[Enemy] = [];
var timer = 1;

func _physics_process(delta: float):
	
	for e in currentEnemies:
		if(e.dead):
			e.global_position = player.global_position + Vector2(0,-900)
	if timer <= 0:
		spawn_enemy_near_player();
		spawn_enemy_near_player();
		timer = 0.5
	else:
		timer -= delta;

func _ready():
	
	await get_tree().create_timer(0.1).timeout
	# get enemies spawned before map to prevent lag
	initialize_enemies()
	
	await get_tree().create_timer(0.1).timeout
	
	
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
				row.append(2)  # this used to append 0, but now it appends 2. 2 is cave air. Cave air might be replaced. 
			else:	
				row.append(1)
		map.append(row);

	var startPos = Vector2(64,150)
	var endPos = Vector2(300, randi_range(100,250))

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

	#print the matrix
	#for y in range(length):
	#	print(map[y])	
	
	for y in range(length):
		for x in range(length):
			cave_air_shift(Vector2(x,y))
	
	cellular_automatize()
	
	player.global_position = startPos * 16;
	
	spawn_tiles();
	
	spawn_important_item(endPos);

func try_carve(pos: Vector2):
	if(pos.x >= length or pos.x < 0):
		return
	if(pos.y >= length or pos.y < 0):
		return
	map[pos.y][pos.x] = 0

func spawn_tiles():
	for y in range(length):
		for x in range(length):
			if(map[y][x] == 0):
				tilemap.set_cell(Vector2i(x, y), 0, Vector2i(2, 2)) #floor
			else:
				var tileTypeVal : int = 0;
				if(floor_here(Vector2(x+1,y))):
					tileTypeVal += 1;
				if(floor_here(Vector2(x-1,y))):
					tileTypeVal += 2;
				if(floor_here(Vector2(x,y+1))):
					tileTypeVal += 4;
				if(floor_here(Vector2(x,y-1))):
					tileTypeVal += 8;

				# this code is kinda bulky, but idk how else to do this
				match tileTypeVal:
					0:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(1, 6)) # blank wall
					1:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(2, 6)) #wall
					2:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(0, 6)) #wall
					3:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(4, 2)) #wall
					4:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(1, 7)) #wall
					5:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(2, 7)) #wall
					6:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(0, 7)) #wall
					7:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(4, 6)) # down end
					8:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(1, 5)) #wall
					9:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(2, 5)) #wall
					10:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(0, 5)) #wall
					11:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(4, 1)) # up end
					12:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(6, 4)) #wall
					13:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(7, 4)) # right end
					14:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(2, 4)) # left end
					15:
						tilemap.set_cell(Vector2i(x, y), 0, Vector2i(4, 4)) # walls

func floor_here(pos: Vector2) -> bool:
	
	if(pos.x >= length or pos.x < 0):
		return false;
	if(pos.y >= length or pos.y < 0):
		return false;
	if(map[pos.y][pos.x] == 0):
		return true
	return false

func nearby_enemies() -> int:
	var count = 0;	
	for e in currentEnemies:
		if(!e): continue;
		if e.global_position.distance_to(player.global_position) < 800:
			count += 1;		
	return count;
	
	
func initialize_enemies():
	for i in range(enemyPool.size()):
		for k in range(5): # a maximum of 5 of each enemy can be on the screen
			spawn_enemy(i, Vector2(200,200), true)
	
func spawn_enemy_near_player():
	if(nearby_enemies() < 50):
		var rand_pos = (player.global_position as Vector2i) + Vector2i(randi_range(-1000,1000), randi_range(-1000,1000));
		var map_rand_pos = Vector2i(rand_pos.x / 16, rand_pos.y / 16);
		
		if(floor_here(map_rand_pos)):
			print("num of currentEnemies: ", currentEnemies.size())
			for e in currentEnemies: # find the first faraway enemy and brings it here
				if(!e):
					print("e null")
					continue;
				if e.global_position.distance_to(player.global_position) > 800:
					e.global_position = map_rand_pos * 16
					#e.global_position = player.global_position
					e.Revive()
					print("revived enemy near player")
					return
	return
		
func spawn_enemy(index: int, pos: Vector2, isDead: bool):
	var enemy_scene = enemyPool[index];
	var new_enemy = enemy_scene.instantiate() as Enemy
	if(new_enemy != null):
		get_parent().get_parent().add_child(new_enemy) # adds the enemy under the root node
		var newPos = pos * 16
		new_enemy.global_position = newPos
		new_enemy.dead = isDead;
		if(!new_enemy.get_parent()):
			print("NULL PARENT ", new_enemy.get_parent()) # this is always printing for some reason
			return;
		#print("spawned enemy at pos", newPos)
		currentEnemies.append(new_enemy);

func spawn_important_item(pos: Vector2):
	var important_item = important_item_scene.instantiate() as Node2D
	if(important_item != null):
		add_child(important_item)
		var newPos = pos * 16
		important_item.global_position = newPos

# shifts a cave air tile to air if there's adjacent air, wall otherwise
func cave_air_shift(pos: Vector2, power: int = 20):
	var x = pos.x;
	var y = pos.y;

	if(power <= 0):
		return
	if(x >= length-1 or x <= 0):
		return
	if(y >= length-1 or y <= 0):
		return
	if(map[y][x] != 2):
		return

	if(map[y-1][x] == 0):
		map[y][x] = 0
		cave_air_shift(Vector2(x,y+1), power-1);
		cave_air_shift(Vector2(x-1,y), power-1);
		cave_air_shift(Vector2(x+1,y), power-1);
		return;

	if(map[y+1][x] == 0):
		map[y][x] = 0
		cave_air_shift(Vector2(x,y-1), power-1);
		cave_air_shift(Vector2(x-1,y), power-1);
		cave_air_shift(Vector2(x+1,y), power-1);
		return;

	if(map[y][x+1] == 0):
		map[y][x] = 0
		cave_air_shift(Vector2(x,y+1), power-1);
		cave_air_shift(Vector2(x-1,y), power-1);
		cave_air_shift(Vector2(x,y-1), power-1);
		return;

	if(map[y][x-1] == 0):
		map[y][x] = 0
		cave_air_shift(Vector2(x,y+1), power-1);
		cave_air_shift(Vector2(x,y-1), power-1);
		cave_air_shift(Vector2(x+1,y), power-1);
		return;
		
	map[y][x] = 1; # no adjacent air, so it becomes wall
		

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
	var x = pos.x;
	var y = pos.y;
	
	if(x >= length-1 or x <= 0):
		#map[y][x] = 1
		return
	if(y >= length-1 or y <= 0):
		#map[y][x] = 1
		return
	
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
