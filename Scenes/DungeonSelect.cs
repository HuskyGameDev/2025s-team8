using Godot;
using System;

// Should be fully working
public partial class DungeonSelect : Node2D, ITransitionOnDeath
{
	[Export]
	Button HomeButton;
	[Export]
	Button Dung1Button;
	[Export]
	Button Dung2Button;
	[Export]
	Button Dung3Button;
	
	// Direct paths to scenes, dungeon paths will be changed as they are added
	const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
	const string LOAD_SCENE_PATH = "res://UI/Scenes/LoadScreen.tscn";
	const string PAUSE_MENU_PATH = "res://UI/Scenes/PauseMenu.tscn";
	const string DUNGEON1 = "res://Scenes/TestingGround.tscn";
	const string DUNGEON2 = "res://Scenes/level3.tscn";
	const string DUNGEON3 = "res://Scenes/level2.tscn"; 
	const string HOME = "res://UI/Scenes/MainMenu.tscn";

	private Node UI_ROOT;
	private Node GAME_ROOT;
	private int onDeathMode = 0;
	// 0 to start as no dungeon selected, value set by corresponding button press
	private int DungNum = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);

		// += relate to functions, they are the name of functions
		if (HomeButton != null)
		{
			ResourceLoader.LoadThreadedRequest(HOME);
			HomeButton.Pressed += ToMainMenu;
		}
		if (Dung1Button != null)
		{
			ResourceLoader.LoadThreadedRequest(DUNGEON1);
			Dung1Button.Pressed += ToDungeon1;
		}
		if (Dung2Button != null)
		{
			ResourceLoader.LoadThreadedRequest(DUNGEON2);
			Dung2Button.Pressed += ToDungeon2;
		}
		if (Dung3Button != null)
		{
			ResourceLoader.LoadThreadedRequest(DUNGEON3);
			Dung3Button.Pressed += ToDungeon3;
		}

		UI_ROOT = this.GetTree().GetNodesInGroup("UI_ROOT")[0];
		GAME_ROOT = this.GetTree().GetNodesInGroup("GAME_ROOT")[0];
		
		MusicManager.Instance.PlayTrack("res://Sounds/Music/Dungeon1_Temp.mp3");
	}
	
	private void ToMainMenu() {
		// Get copies of Transition and MainMenu
		PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene savesScene = (PackedScene) ResourceLoader.LoadThreadedGet(HOME);
		Node sNode = savesScene.Instantiate();

		// Add into scene (UI Root -> Transition -> MainMenu)
		UI_ROOT.GetNode("PauseMenu").QueueFree();
		UI_ROOT.AddChild(tNode);

		// Begin the transition
		((Transition)tNode).BeginTransition(sNode, this, Transition.Mode.topLeft);
	}
	
	private void ToDungeon1() {
		// Get copies of Transition and first dungeon
		PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene loadScene = (PackedScene)ResourceLoader.LoadThreadedGet(LOAD_SCENE_PATH);
		Node lNode = loadScene.Instantiate();

		// Remove old pause menu to prevent doubling and transition to dungeon
		UI_ROOT.GetNode("PauseMenu").QueueFree();
		UI_ROOT.AddChild(tNode);
		DungNum = 1;
		onDeathMode = 1;
		
		// Begin the transition
		((Transition)tNode).BeginTransition(lNode, this, Transition.Mode.topRight);
	}
	
	private void ToDungeon2() {
		// Get copies of Transition and second dungeon
		PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene loadScene = (PackedScene)ResourceLoader.LoadThreadedGet(LOAD_SCENE_PATH);
		Node lNode = loadScene.Instantiate();

		// Remove old pause menu to prevent doubling and transition to dungeon
		UI_ROOT.GetNode("PauseMenu").QueueFree();
		UI_ROOT.AddChild(tNode);
		DungNum = 2;
		onDeathMode = 1;
		
		// Begin the transition
		((Transition)tNode).BeginTransition(lNode, this, Transition.Mode.topRight);
	}
	
	private void ToDungeon3() {
		// Get copies of Transition and third dungeon
		PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene loadScene = (PackedScene)ResourceLoader.LoadThreadedGet(LOAD_SCENE_PATH);
		Node lNode = loadScene.Instantiate();

		// Remove old pause menu to prevent doubling and transition to dungeon
		UI_ROOT.GetNode("PauseMenu").QueueFree();
		UI_ROOT.AddChild(tNode);
		DungNum = 3;
		onDeathMode = 1;
		
		// Begin the transition
		((Transition)tNode).BeginTransition(lNode, this, Transition.Mode.topRight);
	}
	
	public void OnDeath(Node other)
	{
		if (onDeathMode == 1)
		{
			LoadScreen.LoadData[] data = new LoadScreen.LoadData[2]{
				new LoadScreen.LoadData(PAUSE_MENU_PATH, LoadScreen.Type.UI, false),
				new LoadScreen.LoadData(DUNGEON1, LoadScreen.Type.GAME)
			};
			
			if(DungNum == 1){
				data[1] = new LoadScreen.LoadData(DUNGEON1, LoadScreen.Type.GAME);
			} else if(DungNum == 2){
				data[1] = new LoadScreen.LoadData(DUNGEON2, LoadScreen.Type.GAME);
			} else if(DungNum == 3){
				data[1] = new LoadScreen.LoadData(DUNGEON3, LoadScreen.Type.GAME);
			}
			((LoadScreen)other).Init(data, LoadScreen.Mode.LOAD);
		}
	}
}
