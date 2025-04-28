using System;
using System.Collections.Generic;
using Godot;

public partial class Inventory : Control
{
	public List<InventorySlot> Slots = new();

	private ItemList _itemList;
	
	public override void _Ready()
	{
		_itemList = GetNode<ItemList>("ScrollContainer/ItemList");

		GD.Print("ItemList node found : " + _itemList);
	}

	public override void _Process(double delta)
	{
		// Check if the e key is pressed
		if (Input.IsActionJustPressed("e_key"))
		{
			if (_itemList.Visible)
			{
				_itemList.Hide();
			}
			else
			{
				_itemList.Show();
			}
		}
	}

	public void AddItem(string itemId, int amount = 1)
	{
		Item item = ItemDatabase.Items[itemId];

		if (item == null)
		{
			GD.PrintErr($"Item {itemId} not found in database.");
			return;
		}

		if (!item.IsStackable)
		{
			Slots.Add(new InventorySlot(item, 1));
			UpdateUI();
			return;
		}

		// Try to find existing slot
		foreach (var slot in Slots)
		{
			if (slot.Item.Id == itemId)
			{
				slot.Count += amount;
				UpdateUI();
				return;
			}
		}

		// If not found, create a new slot
		InventorySlot newSlot = new(item, amount);
		Slots.Add(newSlot);

		// Update UI
		UpdateUI();
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

	private void UpdateUI()
	{
		_itemList.Clear();
		foreach (var slot in Slots)
		{
			_itemList.AddItem($"{slot.Item.DisplayName} x{slot.Count}", slot.Item.Icon);
		}
	}
}
