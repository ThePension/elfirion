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

public partial class Terrain : TileMapLayer
{
	private FastNoiseLite fastNoiseLite = new();
	private Vector2I chunkCoord;
	
	// Structure to hold grid (terrain) data
	public TileInfo[,] data;
	
	public override void _Process(double delta) {
		if (Input.IsActionPressed("refresh")) {
			RandomNumberGenerator rng = new();
			rng.Randomize();
			int seed = rng.RandiRange(0, 500);
			GenerateTerrain(chunkCoord, seed);
		}
	}
	
	public void GenerateTerrain(Vector2I chunkCoord, int seed)
	{
		this.chunkCoord = chunkCoord;
		this.data = new TileInfo[Settings.Instance.ChunkSize, Settings.Instance.ChunkSize];

		int offsetX = chunkCoord.X * Settings.Instance.ChunkSize;
		int offsetY = chunkCoord.Y * Settings.Instance.ChunkSize;

		// The list of tiles we want to use with the noise. Order matters !
		List<TileInfo> tilesList =
		[
			TileDictionary.GRASS,
			TileDictionary.WATER
		];

		fastNoiseLite.Seed = seed;

		// Try out other parameters from [NoiseTypeEnum] for cool variants !
		fastNoiseLite.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
		
		// The number of layers we want to generate on the noise. Each tile will have its own layer.
		fastNoiseLite.FractalOctaves = tilesList.Count;
		fastNoiseLite.FractalGain = 0;
		
		for (int x = 0; x < Settings.Instance.ChunkSize; x++)
		{
			for (int y = 0; y < Settings.Instance.ChunkSize; y++)
			{
				// We get the noise coordinate as an absolute value (which represents the gradient - or layer) .
				float scale = 0.7f; // tweak between 0.01 and 0.2 depending on world size
				float absNoise = Math.Abs(fastNoiseLite.GetNoise2D((x + offsetX) * scale, (y + offsetY) * scale));
								
				var position = new Vector2I(x, y);
				
				// We determine which tile our value corresponds to.
				int tileToPlace = (int)Math.Floor((absNoise * tilesList.Count));
				
				var tileInfo = tilesList[tileToPlace];
				
				SetCell(position, tileInfo.SourceId, tileInfo.Coord);

				data[x, y] = tileInfo;
			}
		}
	}
 
	public void UpdateTile(Vector2I tileCoord)
	{
		// Set the mouse over tile in the terrain
		tileCoord = new Vector2I(
			Mathf.FloorToInt(tileCoord.X / (float)(Settings.Instance.TileSizePx)),
			Mathf.FloorToInt(tileCoord.Y / (float)(Settings.Instance.TileSizePx))
		);

		tileCoord = new Vector2I(
			tileCoord.X % Settings.Instance.ChunkSize,
			tileCoord.Y % Settings.Instance.ChunkSize
		);

		// Handle negative coordinates
		if (tileCoord.X < 0)
		{
			tileCoord.X += Settings.Instance.ChunkSize;
		}
		if (tileCoord.Y < 0)
		{
			tileCoord.Y += Settings.Instance.ChunkSize;
		}

		
		if (tileCoord.X >= 0 && tileCoord.X < Settings.Instance.ChunkSize && 
			tileCoord.Y >= 0 && tileCoord.Y < Settings.Instance.ChunkSize)
		{
			// Set the tile to grass
			SetCell(tileCoord, TileDictionary.GRASS.SourceId, TileDictionary.GRASS.Coord);
		}
	}
}
