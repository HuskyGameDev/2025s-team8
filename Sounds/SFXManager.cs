using Godot;
using System;

public partial class SFXManager : AudioStreamPlayer {
    [Export]
    string audioBus = "Master";
    public override void _Ready() {
        base._Ready();

        // Update audio bus this ASP is on from default, unless we were given an invalid bus name
        int busId = AudioServer.GetBusIndex(audioBus);
        if (busId != -1) {
            this.Bus = audioBus;
        }
    }

    // Play() should inherit from AudioStreamPlayer
    //    if a sound is already loaded you could call this arg-less method instead

    public void Play(Sfx sound) {
        this.Stream = sound.Get();
        if (this.Stream == null) {
            GD.Print("No Sound Loaded!");
            return;
        }
        this.Play();
    }

    public void PlayFrom(Sfx sound, float seconds) {
        this.Stream = sound.Get();
        if (this.Stream == null) {
            GD.Print("No Sound Loaded!");
            return;
        }
        this.Play(seconds);
    }

    // void Stop() should inherit from AudioStreamPlayer

    // void Seek(float seconds) should inherit from AudioStreamPlayer
}
