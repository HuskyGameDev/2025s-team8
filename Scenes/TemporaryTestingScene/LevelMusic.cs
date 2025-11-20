using Godot;
using System;

public partial class LevelMusic : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MusicManager.Instance.PlayTrack("res://Sounds/Music/Dungeon3_Temp.mp3");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
