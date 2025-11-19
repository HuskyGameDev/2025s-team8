using Godot;
using System;

public partial class NewGame : Control, ITransitionOnDeath
{
    [Export]
    private Button beginButton;
    [Export]
    private Button backButton;
    [Export]
    private LineEdit playerName;

    const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
    const string MAIN_MENU_PATH = "res://UI/Scenes/MainMenu.tscn";
    const string LOAD_SCENE_PATH = "res://UI/Scenes/LoadScreen.tscn";
    const string PAUSE_MENU_PATH = "res://UI/Scenes/PauseMenu.tscn";
    const string GAME_SCENE_PATH = "res://Scenes/TemporaryTestingScene/Tutorial.tscn";

    private Node UI_ROOT;
    private SFXManager menuSfx;
    private Node GAME_ROOT;
    private int onDeathMode = 0;

    public override void _Ready()
    {
        ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);
        ResourceLoader.LoadThreadedRequest(LOAD_SCENE_PATH);

        if (beginButton != null && playerName != null)
        {
            // ResourceLoader.LoadThreadedRequest(GAME_SCENE_PATH);
            // ResourceLoader.LoadThreadedRequest(PAUSE_MENU_PATH);
            beginButton.Pressed += BeginGame;
        }
        if (backButton != null)
        {
            ResourceLoader.LoadThreadedRequest(MAIN_MENU_PATH);
            backButton.Pressed += Back;
        }

        UI_ROOT = this.GetTree().GetNodesInGroup("UI_ROOT")[0];
        menuSfx = UI_ROOT.GetNode<SFXManager>("./ASP");
        GAME_ROOT = this.GetTree().GetNodesInGroup("GAME_ROOT")[0];
    }

    private void BeginGame()
    {
        // Play Sound
        menuSfx.Play(Sounds.UI_Click);

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
            while (PlayerData.DoesSaveExist(saveName + "~" + 2))
            {
                count++;
            }
            saveName = saveName + count;
        }
        pd.save.saveName = saveName;

        // New Experimental Means of Loading
        PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
        Node tNode = transitionScene.Instantiate();
        PackedScene loadScene = (PackedScene)ResourceLoader.LoadThreadedGet(LOAD_SCENE_PATH);
        Node lNode = loadScene.Instantiate();

        // Add into scene (UI Root -> Transition -> LoadScreen)
        UI_ROOT.AddChild(tNode);
        onDeathMode = 1;

        // Begin the transition
        ((Transition)tNode).BeginTransition(lNode, this, Transition.Mode.topRight);
    }

    private void Back()
    {
        // Play Sound
        menuSfx.Play(Sounds.UI_Back);

        // Get copies of Transition and MainMenu
        PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
        Node tNode = transitionScene.Instantiate();
        PackedScene mainScene = (PackedScene)ResourceLoader.LoadThreadedGet(MAIN_MENU_PATH);
        Node mNode = mainScene.Instantiate();

        // Add into scene (UI Root -> Transition -> MainMenu)
        UI_ROOT.AddChild(tNode);

        // Begin the transition
        ((Transition)tNode).BeginTransition(mNode, this, Transition.Mode.bottomLeft);
    }

    public void OnDeath(Node other)
    {
        if (onDeathMode == 1)
        {
            LoadScreen.LoadData[] data = new LoadScreen.LoadData[2]{
                new LoadScreen.LoadData(PAUSE_MENU_PATH, LoadScreen.Type.UI, false),
                new LoadScreen.LoadData(GAME_SCENE_PATH, LoadScreen.Type.GAME)
            };
            ((LoadScreen)other).Init(data, LoadScreen.Mode.LOAD);
        }
    }
}
