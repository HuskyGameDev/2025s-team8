using Godot;
using System;
using System.Collections.Generic;

public partial class AudioSettings : SettingsCategory {
    [Export]
    private Slider masterAudio;
    private Config cfg;

    public override void _Ready() {
        cfg = this.GetTree().GetRoot().GetNode<Config>("./Config");

        if (masterAudio != null) {
            masterAudio.ValueChanged += (val) => {
                Update("Master", val);
            };
            masterAudio.Value = (double)cfg.Get("Audio", "Master", 1);
        }
    }


    private void Update(string cat, double val) {
        cfg.Set("Audio", cat, val);
        cfg.Save(); // The user should hear audio changes as they make them
    }

    //*** ISettingsCategoty impl ***//
    public override void ApplyChanges() {
        // Everything already saved, do nothing (again, user should hear changes as they are made, not as they apply them)
    }
    
    public override void DiscardChanges() {
        // NYI
    }

    protected override void OnChangeMade() {
        // NYI
    }

    public override void RevertToDefaults() {
        double d_masterAudio = (double)masterAudio.GetMeta("default");
        cfg.Set("Audio", "Master", d_masterAudio);

        cfg.Save();
    }
    
    public override void CfgInit() {
        if ((double)cfg.Get("Audio", "Master", -1) == -1) {
            cfg.Set("Audio", "Master", (double)masterAudio.GetMeta("default"));
        }
    }
}
