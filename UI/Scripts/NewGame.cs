using Godot;
using System;

public partial class NewGame : Control
{
    [Export]
    private Button beginButton;
    [Export]
    private Button backButton;
    [Export]
    private LineEdit playerName;

    const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
    const string MAIN_MENU_PATH = "res://UI/Scenes/MainMenu.tscn";

    const string PAUSE_MENU_PATH = "res://UI/Scenes/PauseMenu.tscn";
    const string GAME_SCENE_PATH = "res://Scenes/TestingGround.tscn";
    
    private Node UI_ROOT;
    private Node GAME_ROOT;

    public override void _Ready()
    {
        ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);

        if (beginButton != null && playerName != null)
        {
            ResourceLoader.LoadThreadedRequest(GAME_SCENE_PATH);
            ResourceLoader.LoadThreadedRequest(PAUSE_MENU_PATH);
            beginButton.Pressed += BeginGame;
        }
        if (backButton != null)
        {
            ResourceLoader.LoadThreadedRequest(MAIN_MENU_PATH);
            backButton.Pressed += Back;
        }
        
        UI_ROOT = this.GetTree().GetNodesInGroup("UI_ROOT")[0];
        GAME_ROOT = this.GetTree().GetNodesInGroup("GAME_ROOT")[0];
    }

    private void BeginGame()
    {
        // Ensure there is a valid player name
        if (playerName.Text.Length == 0) return;

        // Set up a new SaveState
        PlayerData pd = this.GetTree().GetRoot().GetNode<PlayerData>("./PlayerData");
        pd.save = new PlayerData.SaveState();
        pd.save.playerName = playerName.Text;

        string saveName = playerName.Text;
        if (PlayerData.DoesSaveExist(saveName)) // ensure we do not conflict with a pre-existing save
        {
            int count = 2;
            while (PlayerData.DoesSaveExist(saveName + "~"+ 2))
            {
                count++;
            }
            saveName = saveName + count;
        }
        pd.save.saveName = saveName;

        // Imma be honest this code is probably not worth saving, when I get around to using LoadScreen for this
        // // // Get copies
        // // PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
        // // Node tNode = transitionScene.Instantiate();
        // // PackedScene loadScene = (PackedScene) ResourceLoader.LoadThreadedGet(LOAD_SCREEN_PATH);
        // // Node lNode = loadScene.Instantiate();

        // // // Add into scene (Root -> Transition -> LoadScreen)
        // // Node root = this.GetTree().GetRoot();
        // // root.AddChild(tNode);

        // // // PROBABLY SOME EXTRAS HERE FOR SETTING UP LOADSCREEN TO ACTUALLY START A NEW GAME
        // // //   BUT THAT IS NOT IMPLEMENTED YET

        // // // Begin the transition
        // // ((Transition)tNode).BeginTransition(lNode, this, Transition.Mode.topLeft);

        // Add the Pause Menu in
        PackedScene pauseScene = (PackedScene) ResourceLoader.LoadThreadedGet(PAUSE_MENU_PATH);
        Node pNode = pauseScene.Instantiate();
        ((PauseMenu)pNode).BeginHidden();
        UI_ROOT.AddChild(pNode);

        // Add the actual game scene in
        PackedScene gameScene = (PackedScene) ResourceLoader.LoadThreadedGet(GAME_SCENE_PATH);
        Node gNode = gameScene.Instantiate();
        GAME_ROOT.AddChild(gNode);
        this.QueueFree();
    }

    private void Back()
    {
        // Get copies of Transition and MainMenu
        PackedScene transitionScene = (PackedScene) ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
        PackedScene mainScene = (PackedScene) ResourceLoader.LoadThreadedGet(MAIN_MENU_PATH);
		Node mNode = mainScene.Instantiate();

        // Add into scene (UI Root -> Transition -> MainMenu)
        UI_ROOT.AddChild(tNode);

        // Begin the transition
        ((Transition)tNode).BeginTransition(mNode, this, Transition.Mode.bottomLeft);
    }
}
