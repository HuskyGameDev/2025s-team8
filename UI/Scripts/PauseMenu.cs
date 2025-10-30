using Godot;
using Godot.Collections;
using System;

public partial class PauseMenu : Control, ITransitionOnDeath
{
	[Export]
	private Button resumeButton { get; set; } = null;
	[Export]
	private Button optionsButton { get; set; } = null;
	[Export]
	private Button exitButton { get; set; } = null;

	private const string TRANSITION_PATH = "res://UI/Scenes/Transition.tscn";
	private const string SETTINGS_SCENE_PATH = "res://UI/Scenes/SettingsMenu.tscn";
	private const string MAIN_MENU_SCENE_PATH = "res://UI/Scenes/MainMenu.tscn";
	private const string LOAD_SCREEN_PATH = "res://UI/Scenes/LoadScreen.tscn";
	private Node UI_ROOT = null;

	private bool isDisabled = false;
	private int onDeathMode = 0;

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
		ResourceLoader.LoadThreadedRequest(LOAD_SCREEN_PATH);
		ResourceLoader.LoadThreadedRequest(TRANSITION_PATH);

		UI_ROOT = this.GetTree().GetNodesInGroup("UI_ROOT")[0];
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("Back"))
		{
			if (isDisabled) return;
			// not ignoring this "Back"
			if (this.Visible)
			{
				this.Hide();
				GetTree().Paused = false;
			}
			else
			{
				this.Show();
				GetTree().Paused = true;
				if (resumeButton != null) resumeButton.GrabFocus();
			}
		}
	}

	public void SetDisabled(bool b)
	{
		isDisabled = b;
	}

	private void ResumeGame()
	{
		this.Hide();
		this.GetTree().Paused = false;
	}

	private void GoToOptions()
	{
		// Get copies
		PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
		Node tNode = transitionScene.Instantiate();
		PackedScene returnScene = (PackedScene)ResourceLoader.LoadThreadedGet(SETTINGS_SCENE_PATH);
		Node sNode = returnScene.Instantiate();

		// Add into scene
		UI_ROOT.AddChild(tNode);

		// Ensure return path is set properly
		((SettingsMenu)sNode).ReturnTo("res://UI/Scenes/PauseMenu.tscn");
		onDeathMode = 1;

		// Begin the transition
		((Transition)tNode).BeginTransition(sNode, this, Transition.Mode.topRight);
	}

	private void GoToMainMenu()
	{
		Node root = this.GetTree().GetRoot();

		// Save Game
		PlayerData pd = root.GetNode<PlayerData>("./PlayerData");

		if (pd.save != null)
		{
			// Save Game: attempt to get the player object for equips & inv
			Array<Node> ns = this.GetTree().GetNodesInGroup("PLAYER");
			if (ns.Count == 1) // if it is greater than 1, then somthing is up
			{
				Node player = ns[0];
				Node invRoot = player.GetNode("./CanvasLayer/Control");

				InventorySlot wpn = invRoot.GetNode<InventorySlot>("./WeaponSlot");
				InventorySlot arm = invRoot.GetNode<InventorySlot>("./ArmorSlot");
				InventorySlot rnd = invRoot.GetNode<InventorySlot>("./RandomSlot");
				Array<Node> inv = invRoot.GetNode("./Player_inv").GetChildren(); // should all be InventorySlots, but cannot implicit cast godot arrays like that


				if (wpn.GetChildCount() > 0)
				{
					pd.save.weaponId = ((InventoryItem)wpn.GetChild(0)).data.GetId();
				}
				if (arm.GetChildCount() > 0)
				{
					pd.save.armorId = ((InventoryItem)arm.GetChild(0)).data.GetId();
				}
				if (rnd.GetChildCount() > 0)
				{
					pd.save.consumableId = ((InventoryItem)rnd.GetChild(0)).data.GetId();
				}

				pd.save.inv = new string[inv.Count];
				for (int i = 0; i < inv.Count; i++)
				{
					InventorySlot slot = (InventorySlot)inv[i];
					if (slot.GetChildCount() > 0)
					{
						pd.save.inv[i] = ((InventoryItem)slot.GetChild(0)).data.GetId();
					}
					else
					{
						pd.save.inv[i] = null;
					}
				}

				// Save Game: attempt to get the stash (often will not exist)
				ns = this.GetTree().GetNodesInGroup("STASH");
				if (ns.Count == 1)
				{
					Node stash = ns[0];
					// NYI!!!!
					// We do not have a stash system yet lmao
				}

				PlayerData.Save(pd.save);
			}
		}
		else
		{
			GD.PrintErr("NO SAVE FILE LOADED? SAVE DATA IS LOST!");
		}

		// New Experimental Means of Loading
        PackedScene transitionScene = (PackedScene)ResourceLoader.LoadThreadedGet(TRANSITION_PATH);
        Node tNode = transitionScene.Instantiate();
        PackedScene loadScene = (PackedScene)ResourceLoader.LoadThreadedGet(LOAD_SCREEN_PATH);
        Node lNode = loadScene.Instantiate();

        // Add into scene (UI Root -> Transition -> LoadScreen)
        UI_ROOT.AddChild(tNode);
        onDeathMode = 2;

        // Begin the transition
        ((Transition)tNode).BeginTransition(lNode, this, Transition.Mode.topRight);
	}

	public void BeginHidden()
	{
		this.Hide();
	}

	// ITransitionOnDeath impl
	public void OnDeath(Node other)
	{
		if (onDeathMode == 0)
		{
			this.GetTree().Paused = false;
		} // else if (onDeathMode == 1) // do nothing
		else if (onDeathMode == 2)
		{
			// Remove the game scene
			Node GAME_ROOT = this.GetTree().GetNodesInGroup("GAME_ROOT")[0];
			GAME_ROOT.GetChild(0).QueueFree();
			this.GetTree().Paused = false; // unpause!

			// Setup the LoadScreen
            LoadScreen.LoadData[] data = new LoadScreen.LoadData[1]{
                new LoadScreen.LoadData(MAIN_MENU_SCENE_PATH, LoadScreen.Type.UI)
            };
            ((LoadScreen)other).Init(data, LoadScreen.Mode.SAVE_LOAD);
		}
	}
}
