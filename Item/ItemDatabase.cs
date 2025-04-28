using Godot;
using System;
using System.Collections.Generic;

public static class ItemDatabase
{
	public static Dictionary<string, Item> Items = new()
	{
		{ 
			"stick", 
			new Item { 
				Id = "stick", 
				DisplayName = "stick", 
				IsStackable = true, 
				Weight = 1.0,
				Icon = ResourceLoader.Load<Texture2D>("res://assets/items/stick.png")
			}
		},
		{ 
			"log", 
			new Item { 
				Id = "log",
				DisplayName = "Log",
				IsStackable = true,
				Weight = 5.0,
				Icon = ResourceLoader.Load<Texture2D>("res://assets/items/log.png")
			} 
		},
		{ 
			"rock", 
			new Item { 
				Id = "rock",
				DisplayName = "Rock",
				IsStackable = true,
				Weight = 5.0,
				Icon = ResourceLoader.Load<Texture2D>("res://assets/items/rock.png")
			} 
		},
		{
			"apple",
			new Item
			{
				Id = "apple",
				DisplayName = "Apple",
				IsStackable = true,
				Weight = 0.5,
				Icon = ResourceLoader.Load<Texture2D>("res://assets/items/apple.png")
			}
		}
	};
}
