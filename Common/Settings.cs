using Godot;
using System;

public partial class Settings : Node
{
	public static Settings Instance { get; private set; }

	public int ChunkSize = 16;
	public int TileSizePx = 16;
	public int ViewDistance = 3;
	public int ChunkSizePx;

	// Player related settings
	public int PlayerBaseSpeed = 400;
	public int PlayerSprintSpeed = 800;
	
	public override void _Ready() {
		Instance = this;
		ChunkSizePx = ChunkSize * TileSizePx;
	}
}
