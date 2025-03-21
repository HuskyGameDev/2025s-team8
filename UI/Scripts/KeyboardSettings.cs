using Godot;
using Godot.Collections;
using System;

public partial class KeyboardSettings : Control, ISettingsCategory {
    [Export]
    private Button[] buttons = new Button[0];
    private string[] actions;

    // Config
    private Config cfg;
    private Dictionary<string, string> changes = new Dictionary<string, string>();

    public override void _Ready() {
        cfg = this.GetTree().GetRoot().GetNode<Config>("./Config");

        if (buttons.Length > 0) {
            actions = new string[buttons.Length];

            // For each button, get its related control and default value
            for (int i = 0; i < buttons.Length; i++) {
                actions[i] = (string)buttons[i].GetMeta("control");
                Array<InputEvent> ieks = InputMap.ActionGetEvents(actions[i]);
                for (int j = 0; j < ieks.Count; j++) {
                    if (ieks[j] is InputEventFromWindow) {
                        string txt = ieks[j].AsText();
                        if (txt.EndsWith(" (Physical)")) {
                            txt = txt.Substring(0, txt.Length-11);
                        }
                        buttons[i].Text = txt;
                        break;
                    }
                    if (j == ieks.Count - 1) {
                        GD.PrintErr("No Key Found!");
                    }
                }
            }
        }
    }

    // Gets the names of all our (rebindable) controls.
    public string[] GetControlNames() {
        string[] ctrls = new string[buttons.Length];
        for (int i = 0; i < buttons.Length; i++) {
            ctrls[i] = (string)buttons[i].GetMeta("control");
        }
        return ctrls;
    }

    // Converts the config string into aan actual InputEvent
    public InputEvent DecodeInputEvent(string s) {
        string[] parts = s.Split(" ");
        if (parts[0].Equals("Key")) {
            InputEventKey ie = new InputEventKey();
            ie.Keycode = OS.FindKeycodeFromString(s);
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

    //*** ISettingsCategoty impl ***//
    public void ApplyChanges() {
        
    }

    public void RevertToDefaults() {
        
    }
}