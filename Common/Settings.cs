using System;
using Godot;

public partial class Settings : Node
{
	public static Settings Instance { get; private set; }

	public static int ChunkSize = 16;
	public static int TileSizePx = 16;
	public static int ViewDistance = 1;
	public static int ChunkSizePx;

	// Player related settings
	public static int PlayerBaseSpeed = 40;
	public static int PlayerSprintSpeed = 80;

	// Entity related settings
	public static bool DisplayDebugInfo = false;
	
	public override void _Ready() {
		Instance = this;
		ChunkSizePx = ChunkSize * TileSizePx;
	}
}
