using Godot;
using System;

public partial class MainLabel : Label
{
 	[Export]
	public int ChunkCount { get; set; } = 0;

	public override void _Ready()
	{
		// Display the initial FPS
		UpdateFps();
	}

	public override void _Process(double delta)
	{
		// Clear the label text
		Text = string.Empty;

		// Update the FPS every frame
		UpdateFps();

		// Update the chunk count
		Text += $"Chunk Count: {ChunkCount}\n";
	}

	private void UpdateFps()
	{
		// Get the current FPS
		int fps = (int)(1.0 / GetProcessDeltaTime());
		
		// Update the label text
		Text += $"FPS: {fps}\n";
	}
}
