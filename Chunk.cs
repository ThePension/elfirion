using Godot;
using System;
using System.Collections.Generic;

public class TileInfo 
{
	public int SourceId;
	public Vector2I Coord;
}

public static class TileDictionary
{
	public static TileInfo WATER = new TileInfo() { SourceId = 1, Coord = new Vector2I(0, 0) };
	public static TileInfo GRASS = new TileInfo() { SourceId = 0, Coord = new Vector2I(1, 1) };
}

public partial class Chunk : Godot.TileMapLayer
{
	public static int CHUNK_WIDTH = 16;
	public static int CHUNK_HEIGHT = 16;
	
	private FastNoiseLite fastNoiseLite = new();
	
	public override void _Process(double delta) {
		if (Input.IsActionPressed("refresh")) {
			//GenerateWorld();
		}
	}
	
	public void GenerateWorld(Vector2I chunkCoord)
	{
		int offsetX = chunkCoord.X * CHUNK_WIDTH * 16;
		int offsetY = chunkCoord.Y * CHUNK_HEIGHT * 16;
		
		// A random number generator which we will use for the noise seed
		RandomNumberGenerator rng = new();
		// The list of tiles we want to use with the noise. Order matters !
		List<TileInfo> tilesList =
		[
			TileDictionary.WATER,
			TileDictionary.GRASS
		];

		rng.Randomize();
		//fastNoiseLite.Seed = rng.RandiRange(0, 500);

		// Try out other parameters from [NoiseTypeEnum] for cool variants !
		fastNoiseLite.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
		// The number of layers we want to generate on the noise. Each tile will have its own layer.
		fastNoiseLite.FractalOctaves = tilesList.Count;
		fastNoiseLite.FractalGain = 0;
		
		for (int x = 0; x < CHUNK_WIDTH; x++)
		{
			for (int y = 0; y < CHUNK_HEIGHT; y++)
			{
				// We get the noise coordinate as an absolute value (which represents the gradient - or layer) .
				float absNoise = Math.Abs(fastNoiseLite.GetNoise2D(x + offsetX, y + offsetY));
				
				var position = new Vector2I(x, y);
				
				// We determine which tile our value corresponds to.
				int tileToPlace = (int)Math.Floor((absNoise * tilesList.Count));
				
				var tileInfo = tilesList[tileToPlace];
				
				SetCell(position, tileInfo.SourceId, tileInfo.Coord);
			}
		}
	}
}
