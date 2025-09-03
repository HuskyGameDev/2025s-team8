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
    const string LOAD_SCREEN_PATH = "res://UI/Scenes/LoadScreen.tscn";


    const string GAME_SCENE_PATH = "res://Scenes/TestingGround.tscn";

    public override void _Ready() {
		ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);
		ResourceLoader.LoadThreadedRequest(GAME_SCENE_PATH);

        if (continueButton != null) {
            ResourceLoader.LoadThreadedRequest(SAVE_SELECT_PATH);
            continueButton.Pressed += ToSaveSelect;
        }
        if (newgameButton != null) {
            ResourceLoader.LoadThreadedRequest(LOAD_SCREEN_PATH);
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
        // // Get copies
        // PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		// Node tNode = transitionScene.Instantiate();
        // PackedScene loadScene = (PackedScene) ResourceLoader.LoadThreadedGet(LOAD_SCREEN_PATH);
		// Node lNode = loadScene.Instantiate();

        // // Add into scene (Root -> Transition -> LoadScreen)
        // Node root = this.GetTree().GetRoot();
        // root.AddChild(tNode);

        // // PROBABLY SOME EXTRAS HERE FOR SETTING UP LOADSCREEN TO ACTUALLY START A NEW GAME
        // //   BUT THAT IS NOT IMPLEMENTED YET
        
        // // Begin the transition
        // ((Transition)tNode).BeginTransition(lNode, this, Transition.Mode.topLeft);

        PackedScene gameScene = (PackedScene) ResourceLoader.LoadThreadedGet(GAME_SCENE_PATH);
        Node gNode = gameScene.Instantiate();
        this.GetParent().AddChild(gNode);
        this.QueueFree();
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
