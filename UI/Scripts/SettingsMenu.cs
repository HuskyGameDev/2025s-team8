using Godot;
using System;

public partial class SettingsMenu : Control {
    [Export]
    private Button gameButton;
    [Export]
    private Button audioButton;
    [Export]
    private Button keyboardButton;
    [Export]
    private Button controllerButton;
    [Export]
    private Button back;

    private TabContainer tc;

    private string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
    private string returnPath = "";

    public override void _Ready() {
        if (gameButton != null) {
            gameButton.Pressed += () => {ShowPage(0);};
        }
        if (audioButton != null) {
            audioButton.Pressed += () => {ShowPage(1);};
        }
        if (keyboardButton != null) {
            keyboardButton.Pressed += () => {ShowPage(2);};
        }
        if (controllerButton != null) {
            controllerButton.Pressed += () => {ShowPage(3);};
        }
        if (back != null) {
            back.Pressed += GoBack;
        }

        tc = this.GetNode<TabContainer>("./TabContainer");
        ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);
    }

    private void ShowPage(int i) {
        tc.CurrentTab = i;
    }

    public void ReturnTo(string path) {
        returnPath = path;
        ResourceLoader.LoadThreadedRequest(path);
    }

    private void GoBack() {
        // Get copies
        PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
        PackedScene returnScene = (PackedScene) ResourceLoader.LoadThreadedGet(returnPath);
		Node rNode = returnScene.Instantiate();

        // Add into scene
        Node root = this.GetTree().GetRoot();
        root.AddChild(tNode);

        // Begin the transition
        ((Transition)tNode).BeginTransition(rNode, this, Transition.Mode.bottomLeft);
    }
}
