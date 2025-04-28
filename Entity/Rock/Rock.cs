using Godot;
using System;

public partial class Rock : Entity
{
	public override void _Process(double delta) {
		base._Process(delta);
	}

	public void Init(Vector2I localPosition, Vector2I globalPosition, Vector2I chunkCoord) {
		base.Init(localPosition, globalPosition, chunkCoord, 50.0);
	} 

	public override void Interact(Player player)
	{
		if (player == null)
		{
			GD.PrintErr("Player is null");
			return;
		}

		this.Health -= 5.0; // Decrease tree health by 10
		HealhBar.Value = this.Health;

		HealhBar.Visible = true;

		if (this.Health > 0) {
			return;
		}
		
		player.Inventory.AddItem("rock", 2);

		QueueFree(); // Remove the tree
	}
}
