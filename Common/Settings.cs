using Godot;
using System;

public partial class Settings : Node
{
	public static Settings Instance { get; private set; }

	public int ChunkSize = 16;
	public int TileSizePx = 16;
	public int ViewDistance = 2;
	public int ChunkSizePx;
	
	public override void _Ready() {
		Instance = this;
		ChunkSizePx = ChunkSize * TileSizePx;
	}
}
