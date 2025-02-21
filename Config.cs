using Godot;
using System;

public partial class Config : Node {
    private ConfigFile config = new ConfigFile();
    bool fine = true; // Flag for if loading the config fails

    public override void _Ready() {
        Error lErr = config.Load("user://settings.cfg");
        if (lErr != Error.Ok) {
			if (lErr == Error.FileNotFound) {
				config = new ConfigFile();
				config.Save("user://settings.cfg"); // save empty file so that the config exists now
			} else {
				GD.Print("ERR: Config Failed to Load!");
				GD.Print(lErr);
				fine = false;
				return;
				// should probably close the game...
			}
		} else {
			// settings already exist, load controls since they are stored in InputMap and not Settings.cs
			// SetupControls();
		}
    }

    public Variant Get(String cat, String id, Variant ifnull) {
		Variant v = ifnull;
		if (fine) {
			v = config.GetValue(cat, id, "!FAILED TO READ!");
		}
		if ((String)v == "!FAILED TO READ!") {
			this.Set(cat, id, ifnull); // set the setting to the ifnull (default) value
			return ifnull;
		}
		return v;
	}

    public void Set(String cat, String id, Variant val) { // val is any type
		if (fine) {
			config.SetValue(cat, id, val);

			// // update marker for signal that needs to be sent on save
			// switch (cat) {
			// 	case "Gameplay":
			// 		gameplayUpdated = true;
			// 		break;
			// 	case "Audio":
			// 		audioUpdated = true;
			// 		break;
			// 	case "Video":
			// 		videoUpdated = true;
			// 		break;
			// 	case "Keyboard":
			// 		keyboardUpdated = true;
			// 		break;
			// 	case "Controller":
			// 		controllerUpdated = true;
			// 		break;
			// 	default:
			// 		// what did i just save?
			// 		break;
			// }
		}		
	}
}
