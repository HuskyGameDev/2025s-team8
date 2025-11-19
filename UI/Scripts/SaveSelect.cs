using Godot;
using System;

public partial class SaveSelect : Control {
    [Export]
    private Button contButton;
    [Export]
    private Button eraseButton;
    [Export]
    private Button backButton;

    const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
    const string LOAD_SCREEN_PATH = "res://UI/Scenes/LoadScreen.tscn";
    private const string MAINMENU_SCENE_PATH = "res://UI/Scenes/MainMenu.tscn";
    

    public override void _Ready() {
        if (contButton != null) {
		    ResourceLoader.LoadThreadedRequest(LOAD_SCREEN_PATH);
            backButton.Pressed += Continue;

        }
        if (eraseButton != null) {
            backButton.Pressed += Erase;
            
        }
        if (backButton != null) {
		    ResourceLoader.LoadThreadedRequest(MAINMENU_SCENE_PATH);
		    ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);
            backButton.Pressed += GoBack;
        }

        // Find and list all save files
        // NYI
    }

    private void Continue() {
        // NYI
    }

    private void Erase() {
        // NYI
    }

    private void GoBack() {
        // Get copies of Transition and SaveSelect
        PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
        PackedScene mainScene = (PackedScene) ResourceLoader.LoadThreadedGet(MAINMENU_SCENE_PATH);
		Node mNode = mainScene.Instantiate();

        // Add into scene (Root -> Transition -> SaveSelect)
        Node root = this.GetTree().GetRoot();
        root.AddChild(tNode);

        // Begin the transition
        ((Transition)tNode).BeginTransition(mNode, this, Transition.Mode.bottomRight);
    }
}
