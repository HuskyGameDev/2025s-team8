using Godot;
using System;

public partial class SaveSelect : Control, ITransitionOnDeath
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
    private SFXManager menuSfx;

    private const string SAVEWIDGET_PATH = "res://UI/Prefabs/SaveWidget.tscn";
    private ButtonGroup widgetGroup;
    private int onDeathMode = 0;

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
        menuSfx = UI_ROOT.GetNode<SFXManager>("./MENU_ASP");

        widgetGroup = new ButtonGroup();

        widgetGroup.Pressed += (b) => { EnableSaveButtons((SaveWidget)b); };

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
        PlayerData.SaveState sv = PlayerData.Load(name);

        PackedScene widgetScene = (PackedScene)ResourceLoader.LoadThreadedGet(SAVEWIDGET_PATH);
        SaveWidget widget = (SaveWidget)widgetScene.Instantiate();
        widget.SetButtonGroup(bg);
        if (sv != null)
        { // Valid File
            widget.validSave = true;
            widget.SetPlayerName(sv.playerName);
            widget.SetSaveName(sv.saveName);
        } else
        { // Invalid File
            widget.SetPlayerName("Broken Save File!");
            widget.SetSaveName(":(");
        }

        // Add it to the save browser
        widgetLoc.AddChild(widget);
    }

    private void Continue()
    {
        // Play Sound
        menuSfx.Play(Sounds.UI_Click);

        PlayerData pd = this.GetTree().GetRoot().GetNode<PlayerData>("./PlayerData");
        Node GAME_ROOT = this.GetTree().GetNodesInGroup("GAME_ROOT")[0];

        // Get the save file for the selected save file
        SaveWidget widget = (SaveWidget)widgetGroup.GetPressedButton();
        string saveName = (string)widget.GetMeta("saveName");
        pd.save = PlayerData.Load(saveName);

        // New Experimental Means of Loading
        PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
        Node tNode = transitionScene.Instantiate();
        PackedScene loadScene = (PackedScene)ResourceLoader.LoadThreadedGet(LOAD_SCREEN_PATH);
        Node lNode = loadScene.Instantiate();

        // Add into scene (UI Root -> Transition -> LoadScreen)
        UI_ROOT.AddChild(tNode);
        onDeathMode = 1;

        // Begin the transition
        ((Transition)tNode).BeginTransition(lNode, this, Transition.Mode.topRight);
    }

    private void Erase()
    {
        // Play Sound
        menuSfx.Play(Sounds.UI_Click);

        // Get the selected save file
        SaveWidget widget = (SaveWidget)widgetGroup.GetPressedButton();
        string saveName = (string)widget.GetMeta("saveName");

        // Delete the selected save file (there is no confirm!!!!!)
        PlayerData.Delete(saveName);
        widget.QueueFree();
        this.DisableSaveButtons();
    }

    private void GoBack()
    {
        // Play Sound
        menuSfx.Play(Sounds.UI_Back);

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

    private void EnableSaveButtons(SaveWidget sw)
    {
        if (contButton != null)
        {
            contButton.Disabled = !sw.validSave;
            ((NPRButton)contButton).UpdateState();
        }
        if (eraseButton != null)
        {
            eraseButton.Disabled = false;
            ((NPRButton)eraseButton).UpdateState();
        }
    }

    private void DisableSaveButtons()
    {
        if (contButton != null)
        {
            contButton.Disabled = true;
            ((NPRButton)contButton).UpdateState();
        }
        if (eraseButton != null)
        {
            eraseButton.Disabled = true;
            ((NPRButton)eraseButton).UpdateState();
        }
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
