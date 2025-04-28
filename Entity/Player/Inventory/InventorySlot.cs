using System;

public class InventorySlot
{
	public InventorySlot(Item item, int count)
	{
		Item = item;
		Count = count;
	}

	public Item Item;
	public int Count;

	public double Weight => Item.Weight * Count;
}
