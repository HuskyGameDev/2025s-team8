using Godot;
using Godot.Collections;
using System;

public partial class KeyboardSettings : Control {
    [Export]
    private Button[] buttons = new Button[0];

    private string[] actions;
    private InputEvent[] defaults;

    public override void _Ready() {
        if (buttons.Length > 0) {
            actions = new string[buttons.Length];
            defaults = new InputEvent[buttons.Length];

            // For each button, get its related control and default value
            for (int i = 0; i < buttons.Length; i++) {
                actions[i] = (string)buttons[i].GetMeta("control");
                Array<InputEvent> ieks = InputMap.ActionGetEvents(actions[i]);
                for (int j = 0; j < ieks.Count; j++) {
                    if (ieks[j] is InputEventFromWindow) {
                        defaults[i] = ieks[j];
                        break;
                    }
                }

                //GD.Print("Read Input Mapper #" + i + " (" + actions[i] + ": " + defaults[i] + ")");

                // // USE THIS FOR READING FROM SETTINGS
                // if (value.StartsWith("Key")) {
                //     // Get actual key value, ensure i
                //     string key = value.Substring(4);
                //     if (OS.FindKeycodeFromString(key) == Key.None) {
                //         GD.PushWarning("Button #" + i + " (" + actions[i] + ") failed to read!");
                //         continue;
                //     }
                //     defaults[i] = value;
                // } else if (value.StartsWith("Mouse")) {
                //     string id = value.Substring(6);
                //     if (!id.IsValidInt() || id.ToInt() > 9) {
                //         GD.PushWarning("Button #" + i + " (" + actions[i] + ") failed to read!");
                //         continue;
                //     }
                //     defaults[i] = value;
                // } else {
                //     GD.PushWarning("Button #" + i + " (" + actions[i] + ") failed to read!");
                // }
            }
        }
    }
}
