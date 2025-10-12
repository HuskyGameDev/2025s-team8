using Godot;
using Godot.Collections;
using System;

public partial class Config : Node {

	// Important Config Things
    private ConfigFile config = new ConfigFile();
    bool fine = true; // Flag for if loading the config fails

	// Update Signals
	bool keyboardUpdated = false;
	[Signal]
	public delegate void OnKeyboardUpdateEventHandler();

	bool audioUpdated = false;
	[Signal]
	public delegate void OnAudioUpdateEventHandler();

	bool gameplayUpdated = false;
	[Signal]
	public delegate void OnGameplayUpdateEventHandler();

	// Path References
    const string SETTINGS_SCENE_PATH = "res://UI/Scenes/SettingsMenu.tscn";

    public override void _Ready() {
        Error lErr = config.Load("user://settings.cfg");
        if (lErr != Error.Ok) {
			if (lErr != Error.FileNotFound) {
				GD.PrintErr("ERR: Config Failed to Load!");
				GD.PrintErr(lErr);
				fine = false;
				return;
				// should probably close the game...
			}

			// Make a new config file
			config = new ConfigFile();
			config.Save("user://settings.cfg"); // save empty file so that the config exists now
		}
		// Initialize the Config, whether pre-existing (to load custom configs), or new (to populate the config with defaults)
		ResourceLoader.LoadThreadedRequest(SETTINGS_SCENE_PATH);
		InitConfig();
    }

    public Variant Get(string cat, string id, Variant ifnull) {
		Variant v = ifnull;
		if (fine) {
			v = config.GetValue(cat, id, "!FAILED TO READ!");
		}
		if ((string)v == "!FAILED TO READ!") {
			this.Set(cat, id, ifnull); // set the setting to the ifnull (default) value
			return ifnull;
		}
		return v;
	}

    public void Set(string cat, string id, Variant val) { // val is any type
		if (fine) {
			config.SetValue(cat, id, val);

			// update marker for signal that needs to be sent on save
			switch (cat) {
				case "Gameplay":
					gameplayUpdated = true;
					break;
				case "Audio":
					audioUpdated = true;
					break;
				case "Keyboard":
					keyboardUpdated = true;
					break;
				default:
					// what did i just save?
					break;
			}
		}		
	}

	public void Save() { // val is any type
		if (fine) {
			config.Save("user://settings.cfg");

			// call update signals for any changes (per category basis)
			if (gameplayUpdated) {
				gameplayUpdated = false; // reset
				EmitSignal(SignalName.OnGameplayUpdate);
			}
			if (audioUpdated) {
				audioUpdated = false; // reset
				EmitSignal(SignalName.OnAudioUpdate);

				// General audioUpdate code
				int MASTER_BUS = AudioServer.GetBusIndex("Master");
				float masterVol = (float)Get("Audio", "Master", 1);
				AudioServer.SetBusVolumeDb(MASTER_BUS, Mathf.LinearToDb(masterVol));
				AudioServer.SetBusMute(MASTER_BUS, masterVol < 0.05);
			}
			if (keyboardUpdated) {
				keyboardUpdated = false; //reset
				EmitSignal(SignalName.OnKeyboardUpdate);
			}
		}
	}

//////////////////// PRIVATE CONFIG SETUP AND HELPERS BELOW ////////////////////
	private void InitConfig() {
		if (!fine) return;
		// Get a copy of settings
		PackedScene settingsScene = (PackedScene) ResourceLoader.LoadThreadedGet(SETTINGS_SCENE_PATH);
		Control sNode = (Control)settingsScene.Instantiate();
		sNode.Name = "TEMP";
		sNode.Visible = false;
		this.AddChild(sNode);

		//* Gameplay Init (none yet)

		//* Audio Init
		sNode.GetNode<SettingsCategory>("./TabContainer/audio").CfgInit();

		//* Keyboard Init
		sNode.GetNode<SettingsCategory>("./TabContainer/keyboard").CfgInit();

		//*Controller Init (not anywhere near ready to be implemented)

		//* Cleanup and Finalize
		SaveNoSignal();
		this.GetNode("./TEMP").QueueFree();
	}

	// Same as Save(), but does not signal to other scripts. Intended primarily for Config setup/init
	private void SaveNoSignal() {
		if (fine) {
			config.Save("user://settings.cfg");

			// call update signals for any changes (per category basis)
			if (gameplayUpdated) {
				gameplayUpdated = false; // reset
			}
			if (audioUpdated) {
				audioUpdated = false; // reset

				// General audioUpdate code
				int MASTER_BUS = AudioServer.GetBusIndex("Master");
				float masterVol = (float)Get("Audio", "Master", 1);
				AudioServer.SetBusVolumeDb(MASTER_BUS, Mathf.LinearToDb(masterVol));
				AudioServer.SetBusMute(MASTER_BUS, masterVol < 0.05);
			}
			if (keyboardUpdated) {
				keyboardUpdated = false; //reset
			}
		}
	}
}
