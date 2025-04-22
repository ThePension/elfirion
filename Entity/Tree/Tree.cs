using Godot;
using System;

public partial class Tree : Entity
{
	public override void _Process(double delta) {
		var camera = GetViewport().GetCamera2D(); // returns the active one

		ZIndex = Mathf.RoundToInt(GlobalPosition.Y - camera.GlobalPosition.Y + 1000);
	}
}
