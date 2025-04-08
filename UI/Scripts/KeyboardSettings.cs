using Godot;
using Godot.Collections;
using System;

public partial class KeyboardSettings : SettingsCategory {
    // Button refs
    [Export]
    private Button[] buttons = new Button[0];
    private string[] actions;

    // Rebinding things
    private int rebindId = -1; // -1: Not rebinding, 0+, will rebind that button id
    private int rebindState = 0;

    // Config
    private Config cfg;
    private Dictionary<string, string> changes = new Dictionary<string, string>();

    public override void _Ready() {
        cfg = this.GetTree().GetRoot().GetNode<Config>("./Config");
        if (buttons.Length > 0) {
            actions = new string[buttons.Length];

            // For each button, get its related control, update display
            for (int i = 0; i < buttons.Length; i++) {
                actions[i] = (string)buttons[i].GetMeta("control");
                Array<InputEvent> ieks = InputMap.ActionGetEvents(actions[i]);
                for (int j = 0; j < ieks.Count; j++) {
                    if (ieks[j] is InputEventFromWindow) {
                        string txt = StringifyEvent(ieks[j]);
                        buttons[i].Text = txt;
                        break;
                    }
                    if (j == ieks.Count - 1) {
                        GD.PrintErr("No Key Found!");
                    }
                }
                
                // Attach rebind code to each button
                int k = i;
                buttons[k].Pressed += () => {SetNextRebind(k);};
            }
        }
    }


    public override void _Input(InputEvent @event) {
        // Rebind validation
        if (rebindState != 1) return;
        if (!@event.IsActionType() || !(@event is InputEventFromWindow || @event is InputEventMouseMotion)) return;

        // Begin rebinding
        rebindState = -1;

        // Find the old keybind
        Array<InputEvent> curBinds = InputMap.ActionGetEvents(actions[rebindId]);
        InputEvent olde = null;
        foreach (InputEvent ie in curBinds) {
            if (ie is InputEventFromWindow) {
                olde = ie;
                break;
            }
        }

        if (olde == null) {
            GD.PrintErr("FAILED TO FIND OLD KEYBIND FOR ACTION #" + rebindId);
            return; // permanently locks out rebinding, as something is already broken
        }

        // Cancel?
        if (@event is InputEventKey && ((InputEventKey)@event).Keycode == Key.Escape) {
            buttons[rebindId].Text = StringifyEvent(olde); // set disp to prev event
            buttons[rebindId].GetNode("./Panel").QueueFree();
            buttons[rebindId].ZIndex = 0; //reset
            rebindId = -1;
            rebindState = 0;
            return;
        }

        // Format: No Double-Click binds!
        if (@event is InputEventMouseButton) {
            ((InputEventMouseButton)@event).DoubleClick = false;
        }

        // Replace the old action with the new one
        InputMap.ActionEraseEvent(actions[rebindId], olde);
        InputMap.ActionAddEvent(actions[rebindId], @event);

        // Cleanup and Finish
        cfg.Set("Keyboard", actions[rebindId], EncodeInputEvent(@event));
        buttons[rebindId].Text = StringifyEvent(@event);
        this.GetNode("./HighlightPanel").QueueFree();
        buttons[rebindId].ZIndex = 0; //reset
        rebindId = -1;
        rebindState = 0;
        // this.OnChangeMade();
    }

    //***** HELPER METHODS *****//
    private void SetNextRebind(int id) {
        if (rebindState != 0) return;
        rebindState = 1;
        rebindId = id;
		buttons[id].Text = "Enter key...";

        Panel p = new Panel();
        p.GlobalPosition = new Vector2(0, 0);
        p.Size = new Vector2(1920, 1080);
        p.ZIndex = 1;
        p.Name = "HighlightPanel";
        this.AddChild(p);
        buttons[id].ZIndex = 2;
    }

    private string StringifyEvent(InputEvent ie) {
        string txt = ie.AsText();
        if (txt.EndsWith(" (Physical)")) {
            txt = txt.Substring(0, txt.Length-11);
        }
        return txt;
    }

    // Converts the config string into aan actual InputEvent
    public InputEvent DecodeInputEvent(string s) {
        string[] parts = s.Split(" ");
        if (parts[0].Equals("Key")) {
            InputEventKey ie = new InputEventKey();
            ie.Keycode = OS.FindKeycodeFromString(parts[1]);
            if (ie.Keycode == Key.None) return null;
            return ie;
        } else if (parts[0].Equals("Mouse")) {
            InputEventMouseButton ie = new InputEventMouseButton();
            ie.ButtonIndex = (MouseButton)parts[1].ToInt();
            return ie;
        } else {
            return null;
        }
    }

    // Converts the actual InputEvent into a config string
    public string EncodeInputEvent(InputEvent ie) {
        string s = "";
        if (ie is InputEventKey) {
            s += "Key ";
            s += OS.GetKeycodeString(((InputEventKey)ie).Keycode);
        }
        else if (ie is InputEventMouseButton) {
            s += "Mouse ";
            s += ((InputEventMouseButton)ie).ButtonIndex;
        }
        else return null;

        return s;
    }

    //***** ISettingsCategoty impl *****//
    public override void ApplyChanges() {
        // Does nothing, keybinds are updated as they change
    }
    
    public override void DiscardChanges() {
        // NYI
    }

    public override void RevertToDefaults() {
        if (buttons.Length > 0) {
            // For each button, get its related default value
            for (int i = 0; i < buttons.Length; i++) {
                string defCtrl = (string)buttons[i].GetMeta("default");
                InputEvent ie = DecodeInputEvent(defCtrl);


                Array<InputEvent> ieks = InputMap.ActionGetEvents(actions[i]);
                for (int j = 0; j < ieks.Count; j++) {
                    if (ieks[j] is InputEventFromWindow) {
                        
                        // Replace this action with the default one
                        InputMap.ActionEraseEvent(actions[i], ieks[j]);
                        InputMap.ActionAddEvent(actions[i], ie);

                        // Ensure that the button shows the correct bind
                        string txt = StringifyEvent(ie);
                        buttons[i].Text = txt;

                        // Send changes to Config
                        cfg.Set("Keyboard", actions[i], EncodeInputEvent(ie));
                        break;
                    }
                    if (j == ieks.Count - 1) {
                        GD.PrintErr("No Cur Keybind Found!");
                    }
                }
            }
        }
        cfg.Save();
    }

    public override void CfgInit() {
		foreach (string ctrl in actions) {
			Array<InputEvent> ies = InputMap.ActionGetEvents(ctrl);
			foreach (InputEvent ie in ies) {
				if (ie is not InputEventFromWindow) continue;
				// This action *is* the keyboard (or mouse) action
				string value = (string)cfg.Get("Keyboard", ctrl, "NOTFOUND");
				if (value.Equals("NOTFOUND")) {
					// Set it to the default in settings
					cfg.Set("Keyboard", ctrl, this.EncodeInputEvent(ie));
				} else if (!value.Equals(this.EncodeInputEvent(ie))){
					// Config has a different setting than default, use it
					InputEvent cfge = this.DecodeInputEvent(value); // Better hope the config does not have a malformed keybind...
					InputMap.ActionEraseEvent(ctrl, ie);
					InputMap.ActionAddEvent(ctrl, cfge);
				}
				// else, it is the default, nothing needs to be done
                break;
			}
		}
    }
}