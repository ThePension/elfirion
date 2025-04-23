using Godot;
using System;

public partial class Chunk : Node2D
{
	public Vector2I ChunkCoord { get; private set; }
	public int Seed { get; private set; }
	public Terrain Terrain { get; private set; }

	public void Init(Vector2I chunkCoord, int seed)
	{
		ChunkCoord = chunkCoord;
		Seed = seed;
	}

	public override void _Ready()
	{
		Terrain = GetNode<Terrain>("Terrain");
		GenerateChunk();
	}

	public void GenerateChunk() {
		Terrain.GenerateTerrain(ChunkCoord, Seed);
		GenerateEntities();
	}

	public void GenerateEntities() {
		var rng = new RandomNumberGenerator();
		rng.Randomize();

		// Generate entities here based on Terrain data
		for (int x = 0; x < Settings.Instance.ChunkSize; x++)
		{
			for (int y = 0; y < Settings.Instance.ChunkSize; y++)
			{
				// Check the tile type and generate entities accordingly
				TileInfo tileInfo = Terrain.data[x, y];

				if (tileInfo != null && tileInfo.SourceId == 0)
				{
					if (rng.Randf() < 0.1f) // 10% chance to spawn a tree
					{
						// Generate a tree entity
						Tree tree = GD.Load<PackedScene>("res://Entity/Tree/tree2.tscn").Instantiate<Tree>();

						tree.Position = new Vector2I(x, y) * Settings.Instance.TileSizePx + ChunkCoord;

						AddChild(tree);
					}
				}
			}
		}
	}

	public void UpdateTileInChunk(Vector2I tileCoord)
	{
		Terrain.UpdateTile(tileCoord);
	}
}
