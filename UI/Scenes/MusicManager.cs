using Godot;
using System;

/* How to use MusicManager in scenes
1) Add script to root node of scene if not already one there
2) In _Ready(), call MusicManager.Instance.PlayTrack(""); where the quotes will be replaced with the
path to the music
3) The music will play forever, even between scenes, until explicitly changed or stopped
*/

public partial class MusicManager : Node2D
{
	public static MusicManager Instance { get; private set; }
	private AudioStreamPlayer audio;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		audio = GetNode<AudioStreamPlayer>("Audio");
		GD.Print("Ready is complete");
	}
	
	// Path is the actual music path in file struture to play
	// All prints are just there for debugging
	public void PlayTrack(string path, bool restartIfSame = false)
	{
		GD.Print("Trying to play");
		// If no music was provided
		if (string.IsNullOrEmpty(path))
		{
			GD.Print("Path was empty");
			audio.Stop();
			return;
		}
		
		// If an error occured when loading the music
		var newStream = GD.Load<AudioStream>(path);
		if (newStream == null)
		{
			GD.Print("New stream was null");
			GD.PushError($"MusicManager: Failed to load track: {path}");
			return;
		}

		// Avoid restarting if it's the same song and already playing, even between scenes
		if (audio.Stream == newStream && audio.Playing && !restartIfSame)
		{
			GD.Print("Encountered restart");
			return;
		}
		
		GD.Print("Should be playing now");
		audio.Stream = newStream;
		audio.Play();
	}

	public void Stop() => audio.Stop();
	public bool IsPlaying() => audio.Playing;
}
