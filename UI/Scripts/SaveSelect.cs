using Godot;
using System;

public partial class SaveSelect : Control
{
    [Export]
    private Button contButton;
    [Export]
    private Button eraseButton;
    [Export]
    private Button backButton;
    [Export]
    private Node widgetLoc;

    const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
    const string LOAD_SCREEN_PATH = "res://UI/Scenes/LoadScreen.tscn";
    private const string MAINMENU_SCENE_PATH = "res://UI/Scenes/MainMenu.tscn";
    const string PAUSE_MENU_PATH = "res://UI/Scenes/PauseMenu.tscn";
    const string GAME_SCENE_PATH = "res://Scenes/TestingGround.tscn";

    private Node UI_ROOT;

    private const string SAVEWIDGET_PATH = "res://UI/Prefabs/SaveWidget.tscn";
    private ButtonGroup widgetGroup;

    public override void _Ready()
    {
        if (contButton != null)
        {
            ResourceLoader.LoadThreadedRequest(GAME_SCENE_PATH);
            ResourceLoader.LoadThreadedRequest(PAUSE_MENU_PATH);
            ResourceLoader.LoadThreadedRequest(LOAD_SCREEN_PATH);
            contButton.Pressed += Continue;

        }
        if (eraseButton != null)
        {
            eraseButton.Pressed += Erase;

        }
        if (backButton != null)
        {
            ResourceLoader.LoadThreadedRequest(MAINMENU_SCENE_PATH);
            ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);
            backButton.Pressed += GoBack;
        }

        UI_ROOT = this.GetTree().GetNodesInGroup("UI_ROOT")[0];
        widgetGroup = new ButtonGroup();

        widgetGroup.Pressed += (b) => { EnableSaveButtons(); };

        if (widgetLoc == null)
        {
            GD.PrintErr("No Widget Location! Saves Will Not Be Loaded!");
            return;
        }

        // Find and list all save files
        DirAccess dir = DirAccess.Open("user://saves");
        dir.ListDirBegin();
        string s = dir.GetNext();
        while (s != "")
        {
            if (s.EndsWith(".hdsave"))
            {
                GD.Print(s);
                BuildWidget(s, widgetGroup);
            }

            s = dir.GetNext();
        }
    }

    private void BuildWidget(string s, ButtonGroup bg)
    {
        // Setup
        ResourceLoader.LoadThreadedRequest(SAVEWIDGET_PATH);

        // Build the save widget
        string name = s.Substring(0, s.Length - 7); // remove the ".hdsave" part, as it is not part
        GD.Print(name);
        PlayerData.SaveState sv = PlayerData.Load(name);

        PackedScene widgetScene = (PackedScene)ResourceLoader.LoadThreadedGet(SAVEWIDGET_PATH);
        SaveWidget widget = (SaveWidget)widgetScene.Instantiate();
        widget.SetButtonGroup(bg);
        widget.SetPlayerName(sv.playerName);
        widget.SetSaveName(sv.saveName);

        // Add it to the save browser
        widgetLoc.AddChild(widget);
    }


    private void Continue()
    {
        PlayerData pd = this.GetTree().GetRoot().GetNode<PlayerData>("./PlayerData");

        // Get the save file for the selected save file
        BaseButton bb = widgetGroup.GetPressedButton();
        SaveWidget widget = (SaveWidget)widgetGroup.GetPressedButton();
        string saveName = (string)widget.GetMeta("saveName");
        pd.save = PlayerData.Load(saveName);

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

    private void Erase()
    {
        // NYI
    }

    private void GoBack()
    {
        // Get copies of Transition and SaveSelect
        PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
        Node tNode = transitionScene.Instantiate();
        PackedScene mainScene = (PackedScene)ResourceLoader.LoadThreadedGet(MAINMENU_SCENE_PATH);
        Node mNode = mainScene.Instantiate();

        // Add into scene (UI Root -> Transition -> SaveSelect)
        UI_ROOT.AddChild(tNode);

        // Begin the transition
        ((Transition)tNode).BeginTransition(mNode, this, Transition.Mode.bottomRight);
    }

    private void EnableSaveButtons()
    {
        if (contButton != null)
        {
            contButton.Disabled = false;
            ((NPRButton)contButton).UpdateState();
        }
        if (eraseButton != null)
        {
            eraseButton.Disabled = false;
            ((NPRButton)eraseButton).UpdateState();
        }
    }
}
