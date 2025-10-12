using Godot;
using System;

public partial class MainMenu : Control {
	[Export]
	Button continueButton;
	[Export]
	Button newgameButton;
	[Export]
	Button settingsButton;
	[Export]
	Button quitButton;

	const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
	const string SAVE_SELECT_PATH = "res://UI/Scenes/SaveSelect.tscn";
	const string SETTINGS_SCENE_PATH = "res://UI/Scenes/SettingsMenu.tscn";
	const string NEW_GAME_PATH = "res://UI/Scenes/NewGame.tscn";

	private Node UI_ROOT;

	public override void _Ready()
	{
		ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);

		if (continueButton != null)
		{
			ResourceLoader.LoadThreadedRequest(SAVE_SELECT_PATH);
			continueButton.Pressed += ToSaveSelect;
		}
		if (newgameButton != null)
		{
			ResourceLoader.LoadThreadedRequest(NEW_GAME_PATH);
			newgameButton.Pressed += BeginNewGame;
		}
		if (settingsButton != null)
		{
			ResourceLoader.LoadThreadedRequest(SETTINGS_SCENE_PATH);
			settingsButton.Pressed += ToSettings;
		}
		if (quitButton != null)
		{
			quitButton.Pressed += QuitGame;
		}

		UI_ROOT = this.GetTree().GetNodesInGroup("UI_ROOT")[0];
	}

	private void ToSaveSelect() {
		// Get copies of Transition and SaveSelect
		PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene savesScene = (PackedScene) ResourceLoader.LoadThreadedGet(SAVE_SELECT_PATH);
		Node sNode = savesScene.Instantiate();

		// Add into scene (UI Root -> Transition -> SaveSelect)
		UI_ROOT.AddChild(tNode);

		// Begin the transition
		((Transition)tNode).BeginTransition(sNode, this, Transition.Mode.topLeft);
	}

	private void BeginNewGame() {
		// Get copies of Transition and NewGame
		PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene ngScene = (PackedScene) ResourceLoader.LoadThreadedGet(NEW_GAME_PATH);
		Node ngNode = ngScene.Instantiate(); // Sometimes dying?

		// Add into scene (UI Root -> Transition -> NewGame)
		UI_ROOT.AddChild(tNode);

		// Begin the transition
		((Transition)tNode).BeginTransition(ngNode, this, Transition.Mode.topRight);
	}

	private void ToSettings() {
		// Get copies of Transition and Settings
		PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene settingsScene = (PackedScene) ResourceLoader.LoadThreadedGet(SETTINGS_SCENE_PATH);
		Node sNode = settingsScene.Instantiate();

		((SettingsMenu)sNode).ReturnTo("res://UI/Scenes/MainMenu.tscn");

		// Add into scene (UI Root -> Transition -> Settings)
		UI_ROOT.AddChild(tNode);

		// Begin the transition
		((Transition)tNode).BeginTransition(sNode, this, Transition.Mode.topRight);
	}

	private void QuitGame() {
		GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest); // send notif that we are about to quit
		GetTree().Quit();
	}
}
