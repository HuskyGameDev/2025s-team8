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

    public override void _Ready() {
		ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);

        if (continueButton != null) {
            ResourceLoader.LoadThreadedRequest(SAVE_SELECT_PATH);
            continueButton.Pressed += ToSaveSelect;
        }
        if (newgameButton != null) {
            newgameButton.Pressed += BeginNewGame;
        }
        if (settingsButton != null) {
		    ResourceLoader.LoadThreadedRequest(SETTINGS_SCENE_PATH);
            settingsButton.Pressed += ToSettings;
        }
        if (quitButton != null) {
            quitButton.Pressed += QuitGame;
        }
    }

    private void ToSaveSelect() {
        // Get copies of Transition and SaveSelect
        PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
        PackedScene savesScene = (PackedScene) ResourceLoader.LoadThreadedGet(SAVE_SELECT_PATH);
		Node sNode = savesScene.Instantiate();

        // Add into scene (Root -> Transition -> SaveSelect)
        Node root = this.GetTree().GetRoot();
        root.AddChild(tNode);

        // Begin the transition
        ((Transition)tNode).BeginTransition(sNode, this, Transition.Mode.topLeft);
    }

    private void BeginNewGame() {
        // Currently Does Nothing
    }

    private void ToSettings() {
        // Get copies of Transition and Settings
        PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
        PackedScene settingsScene = (PackedScene) ResourceLoader.LoadThreadedGet(SETTINGS_SCENE_PATH);
		Node sNode = settingsScene.Instantiate();

        ((SettingsMenu)sNode).ReturnTo("res://UI/Scenes/MainMenu.tscn");

        // Add into scene (Root -> Transition -> Settings)
        Node root = this.GetTree().GetRoot();
        root.AddChild(tNode);

        // Begin the transition
        ((Transition)tNode).BeginTransition(sNode, this, Transition.Mode.topRight);
    }

    private void QuitGame() {
		GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest); // send notif that we are about to quit
		GetTree().Quit();
	}
}
