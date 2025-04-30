using Godot;
using System.Collections.Generic;

public static class ItemDatabase
{
	public static Item GetItem(ItemTypes type)
	{
		if (Items.ContainsKey(type))
		{
			return Items[type];
		}
		else
		{
			GD.PrintErr($"Item with ID '{type}' not found in the database.");
			return null;
		}
	}
	
	public static Dictionary<ItemTypes, Item> Items = new()
	{
		{ 
			ItemTypes.Stick, 
			new Item { 
				Id = "stick", 
				DisplayName = "stick", 
				IsStackable = true, 
				Type = ItemTypes.Stick,
				Category = ItemCategories.Miscellaneous,
				Weight = 1.0,
				Icon = ResourceLoader.Load<Texture2D>("res://assets/items/stick.png")
			}
		},
		{ 
			ItemTypes.Log, 
			new Item { 
				Id = "log",
				DisplayName = "Log",
				IsStackable = true,
				Type = ItemTypes.Log,
				Category = ItemCategories.Miscellaneous,
				Weight = 5.0,
				Icon = ResourceLoader.Load<Texture2D>("res://assets/items/log.png")
			} 
		},
		{ 
			ItemTypes.Rock, 
			new Item { 
				Id = "rock",
				DisplayName = "Rock",
				IsStackable = true,
				Type = ItemTypes.Rock,
				Category = ItemCategories.Miscellaneous,
				Weight = 5.0,
				Icon = ResourceLoader.Load<Texture2D>("res://assets/items/rock.png")
			} 
		},
		{
			ItemTypes.Apple,
			new Item
			{
				Id = "apple",
				DisplayName = "Apple",
				IsStackable = true,
				Type = ItemTypes.Apple,
				Category = ItemCategories.Consumable,
				Weight = 0.5,
				Icon = ResourceLoader.Load<Texture2D>("res://assets/items/apple.png")
			}
		},
		{
			ItemTypes.Pickaxe,
			new Item
			{
				Id = "pickaxe",
				DisplayName = "Pickaxe",
				IsStackable = false,
				Type = ItemTypes.Pickaxe,
				Category = ItemCategories.Weapon,
				Weight = 5.0,
				Icon = ResourceLoader.Load<Texture2D>("res://assets/items/pickaxe.png")
			}
		},
		{
			ItemTypes.Axe,
			new Item
			{
				Id = "axe",
				DisplayName = "Axe",
				IsStackable = false,
				Type = ItemTypes.Axe,
				Category = ItemCategories.Weapon,
				Weight = 5.0,
				Icon = ResourceLoader.Load<Texture2D>("res://assets/items/axe.png")
			}
		}
	};
}
