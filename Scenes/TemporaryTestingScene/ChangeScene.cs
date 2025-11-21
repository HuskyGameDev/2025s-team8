using Godot;
using System;

public partial class ChangeScene : Control, ITransitionOnDeath
{
	[Export]
	private Area2D area;
	private Node tutRoot;

	private const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
	private const string LOAD_SCENE_PATH = "res://UI/Scenes/LoadScreen.tscn";
	private const string PAUSE_MENU_PATH = "res://UI/Scenes/PauseMenu.tscn";
	[Export]
	private string GAME_SCENE_PATH = "res://Scenes/DungeonSelect.tscn";

	public override void _Ready()
	{
		ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);
		ResourceLoader.LoadThreadedRequest(LOAD_SCENE_PATH);

		area.BodyEntered += (body) => SwitchScene(body);
		tutRoot = this.GetParent();
	}
	public void ChangeTo(string s)
	{
		GAME_SCENE_PATH = s;
	}
	public void SwitchScene(Node body)
	{
		if (body.Name != "Player") return;

		Node UI_ROOT = this.GetTree().GetNodesInGroup("UI_ROOT")[0];
		// Find and Destroy the pause menu, if it exists
		Node pMenu = UI_ROOT.GetNode("./PauseMenu");
		if (pMenu != null)
		{
			pMenu.QueueFree();
		}

		// New Experimental Means of Loading
		PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene loadScene = (PackedScene)ResourceLoader.LoadThreadedGet(LOAD_SCENE_PATH);
		Node lNode = loadScene.Instantiate();

		// Add into scene (UI Root -> Transition -> LoadScreen)
		UI_ROOT.GetNode("PauseMenu").QueueFree();
		UI_ROOT.AddChild(tNode);

		// Begin the transition
		((Transition)tNode).BeginTransition(lNode, this, Transition.Mode.topRight);
	}

	public void OnDeath(Node other)
	{
		LoadScreen.LoadData[] data = new LoadScreen.LoadData[2]{
			new LoadScreen.LoadData(PAUSE_MENU_PATH, LoadScreen.Type.UI, false),
			new LoadScreen.LoadData(GAME_SCENE_PATH, LoadScreen.Type.GAME)
		};
		((LoadScreen)other).Init(data, LoadScreen.Mode.LOAD);

		tutRoot.QueueFree();
	}
}
