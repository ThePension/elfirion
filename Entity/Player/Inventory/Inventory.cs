using System.Collections.Generic;
using Godot;

public partial class Inventory : Control
{
	public List<InventorySlot> Slots = new();

	private ItemList _itemList;
	private Panel panel;

	public Item? SelectedItem;
	private int _selectedItemIndex = -1;
	
	private TextureRect selectedItemIcon;

	public override void _Ready()
	{
		panel = GetNode<Panel>("ListPanel");
		_itemList = GetNode<ItemList>("ListPanel/ScrollContainer/ItemList");
		selectedItemIcon = GetNode<TextureRect>("SelectedItemPanel/ItemTexture");
	}

	public override void _Process(double delta)
	{
		// Check if the e key is pressed
		if (Input.IsActionJustPressed("e_key"))
		{
			if (panel.Visible)
			{
				panel.Hide();
			}
			else
			{
				panel.Show();
			}
		}
	}

	public bool IsInventoryOpen() => panel.Visible;

	public void AddItem(ItemTypes type, int amount = 1)
	{
		Item item = ItemDatabase.GetItem(type);

		if (item == null)
		{
			GD.PrintErr($"Item {type} not found in database.");
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
			if (slot.Item.Type == type)
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
		
		// Reselect the selected
		if (_selectedItemIndex >= 0 && _selectedItemIndex < Slots.Count)
		{
			_itemList.Select(_selectedItemIndex);
		}
		else
		{
			_itemList.DeselectAll();
		}
	}

	// When an item is selected in the ItemList
	public void OnItemSelected(int index)
	{
		if (index < 0 || index >= Slots.Count)
		{
			return;
		}

		SelectedItem = Slots[index].Item;
		_selectedItemIndex = index;
		// Update the selected item icon
		selectedItemIcon.Texture = SelectedItem?.Icon;
	}

	public void OnItemActivated(int index)
	{
		// Item? item = null;
		// if (index < 0 || index >= Slots.Count)
		// {
		// 	return;
		// }
		// item = Slots[index].Item;

		// if (item == null)
		// {
		// 	return;
		// }

		// if (item.Category == ItemCategories.Consumable)
		// {
		// 	item.Activate(GetParent<Player>());
		// 	// Remove the item from the inventory
		// 	Slots[index].Count--;
		// 	if (Slots[index].Count <= 0)
		// 	{
		// 		Slots.RemoveAt(index);
		// 	}
		// 	UpdateUI();
		// }
		// else if (item.Category == ItemCategories.Tool)
		// {
		// 	GD.Print($"Using tool: {item.DisplayName}");
		// 	item.Activate(GetParent<Player>());
		// }
		// else
		// {
		// 	GD.Print($"Item {item.DisplayName} is not consumable or tool.");
		// }
	}
}
