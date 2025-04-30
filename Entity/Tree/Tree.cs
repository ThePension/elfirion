using Godot;
using System;

public partial class Tree : Entity
{
	public override void _Process(double delta) {
		base._Process(delta);
	}

	public void Init(Vector2I localPosition, Vector2I globalPosition, Vector2I chunkCoord) {
		base.Init(localPosition, globalPosition, chunkCoord, 100.0);
	}

	public override void Interact(Player player)
	{
		if (player == null)
		{
			GD.PrintErr("Player is null");
			return;
		}

		if (player.Inventory.SelectedItem?.Type == ItemTypes.Axe)
		{
			this.Health -= 50.0; // Decrease tree health by 10
		}
		else
		{
			this.Health -= 25.0; // Decrease tree health by 50
		}

		Shake();

		HealhBar.Value = this.Health;

		HealhBar.Visible = true;

		if (this.Health > 0) {
			return;
		}
		
		player.Inventory.AddItem(ItemTypes.Stick, 4);
		player.Inventory.AddItem(ItemTypes.Log, 2);
		
		// Small chance of dropping an apple
		// if (GD.Randf() < 0.1f) // 10% chance to drop an apple
		// {
			player.Inventory.AddItem(ItemTypes.Apple, 1);
		// }

		QueueFree(); // Remove the tree
	}

	public void _on_area_2d_mouse_entered() {
		GD.Print("Mouse entered area at " + GlobalPosition);

		// Stop propagating the signal
		GetViewport().SetInputAsHandled();
	}
}
