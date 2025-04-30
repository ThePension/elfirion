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

	// Activate method
	public virtual void Activate(Player player)
	{
		GD.Print($"Activating item: {DisplayName}");
	}
}
