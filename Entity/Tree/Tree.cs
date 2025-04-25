using Godot;
using System;

public partial class Tree : Entity
{
	public override void _Process(double delta) {
		// Call the base class process method
		base._Process(delta);
		// Add any additional processing for the tree here
	}

	public override void Interact(Player player)
	{
		player.Inventory.AddItem("wood", 1);

		GD.Print($"Player interacted with tree at {GlobalPosition}");

		GD.Print($"Player Inventory: {player.Inventory}");

		QueueFree(); // Remove the tree
	}
}
