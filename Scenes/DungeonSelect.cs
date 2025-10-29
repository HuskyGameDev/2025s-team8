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
	
	const string DUNGEON1 = "res://Scenes/TemporaryTestingScene/Tutorial.tscn";
	const string DUNGEON2 = "res://Scenes/TestingGround.tscn";
	const string DUNGEON3 = ""; // Will become path for the 3rd dungeon
	const string HOME = "res://Scenes/TemporaryTestingScene/Tutorial.tscn";

	private Node UI_ROOT;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);

		// += relate to functions, they are the name of functions
		if (HomeButton != null)
		{
			GD.Print("Home button not null");
			ResourceLoader.LoadThreadedRequest(HOME);
			HomeButton.Pressed += ToMainMenu;
		}
		if (Dung1Button != null)
		{
			ResourceLoader.LoadThreadedRequest(DUNGEON1);
			//newgameButton.Pressed += BeginNewGame;
		}
		if (Dung2Button != null)
		{
			ResourceLoader.LoadThreadedRequest(DUNGEON2);
			//settingsButton.Pressed += ToSettings;
		}
		if (Dung3Button != null)
		{
			ResourceLoader.LoadThreadedRequest(DUNGEON3);
			//settingsButton.Pressed += ToSettings;
		}

		UI_ROOT = this.GetTree().GetNodesInGroup("UI_ROOT")[0];
	}
	
	// Not adding path to tree, not going to the scene
	// Technically no UI Root, this may be issue
	private void ToMainMenu() {
		// Get copies of Transition and SaveSelect
		PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene savesScene = (PackedScene) ResourceLoader.LoadThreadedGet(HOME);
		Node sNode = savesScene.Instantiate();

		// Add into scene (UI Root -> Transition -> SaveSelect)
		UI_ROOT.AddChild(tNode);

		// Begin the transition
		((Transition)tNode).BeginTransition(sNode, this, Transition.Mode.topLeft);
		GD.Print("main menu part ending");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
