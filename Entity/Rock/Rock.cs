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

		if (player.Inventory.SelectedItem?.Type == ItemTypes.Pickaxe)
		{
			this.Health -= 25.0; // Decrease tree health by 50
		} 
		else
		{
			this.Health -= 5.0; // Decrease tree health by 10
		}

		Shake();
		
		HealhBar.Value = this.Health;

		HealhBar.Visible = true;

		if (this.Health > 0) {
			return;
		}
		
		player.Inventory.AddItem(ItemTypes.Rock, 2);

		QueueFree(); // Remove the tree
	}
}
