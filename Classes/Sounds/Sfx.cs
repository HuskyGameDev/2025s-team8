using Godot;
// Sfx class for all base sfx sounds
public class Sfx {
    protected static readonly string SFX_LOC = "res://Sounds/Sfx/"; // Might need to be changed
    protected string fp;

    public Sfx(string fp) {
        this.fp = fp;
    }

        // If the filepath for a SFX is invalid, this will return null.
    public virtual AudioStream Get() {
        return GD.Load<AudioStream>(SFX_LOC + fp);
    }
}