using Godot;
using Godot.Collections;
using System;

public partial class PauseMenu : Control {
	[Export]
	private Button resumeButton {get; set;} = null;
	[Export]
	private Button optionsButton {get; set;} = null;
	[Export]
	private Button exitButton {get; set;} = null;

    private const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
	private const string SETTINGS_SCENE_PATH = "UI/Scenes/SettingsMenu.tscn";
	private const string MAIN_MENU_SCENE_PATH = "UI/Scenes/MainMenu.tscn";
	private Node MENU_ROOT = null;

	private bool ignoreNext = false;

	public override void _Ready()
    {
        if (resumeButton != null)
        {
            resumeButton.Pressed += ResumeGame;
            resumeButton.GrabFocus();
        }
        if (optionsButton != null)
        {
            optionsButton.Pressed += GoToOptions;
        }
        if (exitButton != null)
        {
            exitButton.Pressed += GoToMainMenu;
        }

        ResourceLoader.LoadThreadedRequest(SETTINGS_SCENE_PATH);
        ResourceLoader.LoadThreadedRequest(MAIN_MENU_SCENE_PATH);
        ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);
    }

	public override void _Input(InputEvent @event) {
		if (Input.IsActionJustPressed("Back")) {
			if (ignoreNext) {
				ignoreNext = false;
				return;
			}
			// not ignoring this "Back"
			if (this.Visible) {
				this.Hide();
				GetTree().Paused = false;
			} else {
				this.Show();
				GetTree().Paused = true;
				if (resumeButton != null) resumeButton.GrabFocus();
			}
		}
	}

	public void IgnoreNextOpen(bool doIgnore) {
		ignoreNext = doIgnore;
	}

	private void ResumeGame() {
		this.Hide();
		GetTree().Paused = false;
	}

	private void GoToOptions() {
        // Get copies
        PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
        PackedScene returnScene = (PackedScene) ResourceLoader.LoadThreadedGet(SETTINGS_SCENE_PATH);
		Node sNode = returnScene.Instantiate();

        // Add into scene
        Node root = this.GetTree().GetRoot();
        root.AddChild(tNode);

        // Ensure return path is set properly
        ((SettingsMenu)sNode).ReturnTo("res://UI/Scenes/PauseMenu.tscn");

        // Begin the transition
        ((Transition)tNode).BeginTransition(sNode, this, Transition.Mode.topRight);
	}

	private void GoToMainMenu() {
        // // Save Game
        // !!!!NYI!!!!

        // Get copies
        PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
        PackedScene returnScene = (PackedScene) ResourceLoader.LoadThreadedGet(MAIN_MENU_SCENE_PATH);
		Node mNode = returnScene.Instantiate();

        // Add into scene
        Node root = this.GetTree().GetRoot();
        root.AddChild(tNode);

        // Begin the transition
        ((Transition)tNode).BeginTransition(mNode, this, Transition.Mode.bottomLeft);
	}

	public void BeginHidden() {
		this.Hide();
	}
}
