using Godot;

public class Item
{
	public string Id = string.Empty;
	public string DisplayName = string.Empty;
	public Texture2D? Icon;
	// public int MaxStackSize = 99;
	public ItemTypes Type = ItemTypes.None;
	public ItemCategories Category = ItemCategories.None;
	public bool IsStackable = true;
	public double Weight = 0.0;

	public float Range = 0.0f;

	// Activate method
	public virtual void Activate(Player player)
	{
		GD.Print($"Activating item: {DisplayName}");

		switch (Type)
		{
			case ItemTypes.Stick:
				GD.Print("You used a stick.");
				break;
			case ItemTypes.Log:
				GD.Print("You used a log.");
				break;
			case ItemTypes.Rock:
				GD.Print("You used a rock.");
				break;
			case ItemTypes.Apple:
				GD.Print("You ate an apple.");
				player.Food += 10.0f; // Increase food by 10
				if (player.Food > 100.0f)
				{
					player.Food = 100.0f; // Cap food at 100
				}

				player.Energy += 5.0f; // Increase energy by 5
				if (player.Energy > 100.0f)
				{
					player.Energy = 100.0f; // Cap energy at 100
				}
				break;
			default:
				GD.Print("Unknown item type.");
				break;
		}
	}
}
