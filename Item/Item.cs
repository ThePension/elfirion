using Godot;
using System;

public class Item
{
	public string Id = string.Empty;
	public string DisplayName = string.Empty;
	public Texture2D? Icon;
	// public int MaxStackSize = 99;
	public bool IsStackable = true;
	public double Weight = 0.0;
}
