using Godot;
using System;

public partial class ChangeScene : Control, ITransitionOnDeath
{
	[Export]
	private Area2D area;
    private Node tutRoot;

	const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
	const string LOAD_SCENE_PATH = "res://UI/Scenes/LoadScreen.tscn";
	const string PAUSE_MENU_PATH = "res://UI/Scenes/PauseMenu.tscn";
	const string GAME_SCENE_PATH = "res://Scenes/TestingGround.tscn";

    public override void _Ready()
    {
        ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);
        ResourceLoader.LoadThreadedRequest(LOAD_SCENE_PATH);

        area.AreaEntered += (area) => SwitchScene();
        tutRoot = this.GetParent();
	}
	public void SwitchScene()
	{
		Node UI_ROOT = this.GetTree().GetNodesInGroup("UI_ROOT")[0];
		// New Experimental Means of Loading
		PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene loadScene = (PackedScene)ResourceLoader.LoadThreadedGet(LOAD_SCENE_PATH);
		Node lNode = loadScene.Instantiate();

		// Add into scene (UI Root -> Transition -> LoadScreen)
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
