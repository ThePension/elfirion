using Godot;
using System;

public partial class FpsLabel : Label
{
	public override void _Ready()
	{
		// Display the initial FPS
		UpdateFps();
	}

	public override void _Process(double delta)
	{
		// Update the FPS every frame
		UpdateFps();
	}

	private void UpdateFps()
	{
		// Get the current FPS
		int fps = (int)(1.0 / GetProcessDeltaTime());
		
		// Update the label text
		Text = $"FPS: {fps}";

		// Update location to the top right corner
		//RectPosition = new Vector2(GetViewport().Size.X - RectSize.X - 10, 10);
	}
}
