using Godot;
using System;

public partial class DungeonSelect : Node2D
{
	[Export]
	Button HomeButton;
	[Export]
	Button Dung1Button;
	[Export]
	Button Dung2Button;
	[Export]
	Button Dung3Button;
	
	// These are currently copied from MainMenu.cs, needs to be changed
	// Additional things will be copied over within the functions
	// NOTE FOR SELF: Button actions can be seen in MainMenu.cs, look there to see how buttons operate
	const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
	const string SAVE_SELECT_PATH = "res://UI/Scenes/SaveSelect.tscn";
	const string SETTINGS_SCENE_PATH = "res://UI/Scenes/SettingsMenu.tscn";
	const string NEW_GAME_PATH = "res://UI/Scenes/NewGame.tscn";

	private Node UI_ROOT;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
