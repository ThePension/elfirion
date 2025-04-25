using Godot;
using System;
using System.Collections.Generic;

public static class ItemDatabase
{
	public static Dictionary<string, Item> Items = new()
	{
		{ "wood", new Item { Id = "wood", DisplayName = "Wood", MaxStackSize = 64 } },
		{ "stone", new Item { Id = "stone", DisplayName = "Stone", MaxStackSize = 64 } }
	};
}
