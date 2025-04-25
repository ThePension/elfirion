using System;
using System.Collections.Generic;

public class Inventory
{
	public List<InventorySlot> Slots = new();

	public void AddItem(string itemId, int amount = 1)
	{
		Item item = ItemDatabase.Items[itemId];

		// Try to find existing slot
		foreach (var slot in Slots)
		{
			if (slot.Item.Id == itemId && !slot.IsFull)
			{
				int toAdd = Math.Min(amount, item.MaxStackSize - slot.Count);
				slot.Count += toAdd;
				amount -= toAdd;
				if (amount <= 0) return;
			}
		}

		// Add new slot if needed
		while (amount > 0)
		{
			int toAdd = Math.Min(amount, item.MaxStackSize);
			Slots.Add(new InventorySlot(item, toAdd));
			amount -= toAdd;
		}
	}

	// Override to string
	public override string ToString()
	{
		string result = "Inventory:\n";
		foreach (var slot in Slots)
		{
			result += $"{slot.Item.DisplayName}: {slot.Count}\n";
		}
		return result;
	}
}
