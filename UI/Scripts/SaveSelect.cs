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
        Node GAME_ROOT = this.GetTree().GetNodesInGroup("GAME_ROOT")[0];

        // Get the save file for the selected save file
        BaseButton bb = widgetGroup.GetPressedButton();
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

        // // Inject the save data (since nothing truly "uses" SaveState in any game scene yet)
        // // NYI
        // this.QueueFree();
        // return;
        // // NYI

        // Array<Node> ns = this.GetTree().GetNodesInGroup("PLAYER");
        // if (ns.Count == 1) // if it is greater than 1, then somthing is up
        // {
        //     Node player = ns[0];
        //     Node invRoot = player.GetNode("./CanvasLayer/Control");

        //     InventorySlot wpn = invRoot.GetNode<InventorySlot>("./WeaponSlot");
        //     InventorySlot arm = invRoot.GetNode<InventorySlot>("./ArmorSlot");
        //     InventorySlot rnd = invRoot.GetNode<InventorySlot>("./RandomSlot");
        //     Array<Node> inv = invRoot.GetNode("./Player_inv").GetChildren(); // should all be InventorySlots, but cannot implicit cast godot arrays like that

        //     if (pd.save.weaponId != null && pd.save.weaponId != "")
        //     {
        //         InventoryItem wpnItem = new InventoryItem();
        //         wpnItem.Init(Items.items.Get(pd.save.weaponId));
        //         wpn.AddChild(wpnItem);
        //     }
        //     if (pd.save.armorId != null && pd.save.armorId != "")
        //     {
        //         InventoryItem armItem = new InventoryItem();
        //         armItem.Init(Items.items.Get(pd.save.armorId));
        //         arm.AddChild(armItem);
        //     }
        //     if (pd.save.consumableId != null && pd.save.consumableId != "")
        //     {
        //         InventoryItem rndItem = new InventoryItem();
        //         rndItem.Init(Items.items.Get(pd.save.consumableId));
        //         rnd.AddChild(rndItem);
        //     }

        //     for (int i = 0; i < pd.save.inv.Count; i++)
        //     {
        //         // NYI, as this will conflict with TestingGround's automatic item gen
        //         if (pd.save.inv[i] != null && pd.save.inv[i] != "")
        //         {

        //         }
        //     }

        //     ns = this.GetTree().GetNodesInGroup("STASH");
        //     if (ns.Count == 1)
        //     {
        //         Node stash = ns[0];
        //         // NYI!!!!
        //         // We do not have a stash system yet lmao
        //     }

        // }
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
