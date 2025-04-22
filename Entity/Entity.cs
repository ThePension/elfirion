using Godot;
using System;

public partial class Entity : StaticBody2D
{
	//protected Vector2I Position;
	protected Vector2I ChunkCoord;

	public override void _Process(double delta) {
		var camera = GetViewport().GetCamera2D(); // returns the active one

		ZIndex = Mathf.RoundToInt(GlobalPosition.Y - camera.GlobalPosition.Y + 1000);
	}

	public virtual void Interact(Entity other) { }
	
	public virtual void SaveState() { }
}
