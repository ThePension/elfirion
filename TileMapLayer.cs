using Godot;
using System;

public partial class TileMapLayer : Godot.TileMapLayer
{
	 // Size of the map to generate
	private const int MAP_WIDTH = 20 * 4;
	private const int MAP_HEIGHT = 10 * 4;

	// Number of different tile IDs in your TileSet (adjust as needed)
	private const int TILE_COUNT = 4;

	public override void _Ready()
	{
		var rng = new Random();

		for (int x = 0; x < MAP_WIDTH; x++)
		{
			for (int y = 0; y < MAP_HEIGHT; y++)
			{
				int tileId = rng.Next(0, TILE_COUNT); // Get a random tile ID
				var vector = new Vector2I(x, y);
				SetCell(vector, 0, new Vector2I(1, 1)); // Layer 0, coords, tileId
			}
		}
	}
}
