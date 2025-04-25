using Godot;
using System;
using System.Collections.Generic;

public partial class Chunk : Node2D
{
	public Vector2I ChunkCoord { get; private set; }
	public int Seed { get; private set; }
	public Terrain Terrain { get; private set; }
	public Dictionary<Vector2I, Entity> Entities { get; private set; } = new();

	private Label DebugLabel;

	public void Init(Vector2I chunkCoord, int seed)
	{
		ChunkCoord = chunkCoord;
		Seed = seed;
	}

	public override void _Ready()
	{
		Terrain = GetNode<Terrain>("Terrain");

		DebugLabel = GetNode<Label>("DebugLabel");
		DebugLabel.Text = $"Chunk : {ChunkCoord} \n"
						+ $"Seed: {Seed}";

		DebugLabel.ZIndex = 1000;

		DebugLabel.Visible = Settings.DisplayDebugInfo;

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
		for (int x = 0; x < Settings.ChunkSize; x++)
		{
			for (int y = 0; y < Settings.ChunkSize; y++)
			{
				// Check the tile type and generate entities accordingly
				TileInfo tileInfo = Terrain.data[x, y];

				if (tileInfo != null && tileInfo.SourceId == 0)
				{
					if (rng.Randf() < 0.1f) // 10% chance to spawn a tree
					{
						// Generate a tree entity
						Tree tree = GD.Load<PackedScene>("res://Entity/Tree/tree2.tscn").Instantiate<Tree>();

						tree.Init(new Vector2I(x, y), new Vector2I(x, y) * Settings.TileSizePx, ChunkCoord);

						Entities.Add(new Vector2I(x, y), tree);

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

	public Entity GetEntityAt(Vector2I globalTileCoord)
	{
		var localCoord = CoordinatesHelper.WorldToLocal(globalTileCoord);

		if (Entities.ContainsKey(localCoord))
		{
			return Entities[localCoord];
		}
		else
		{
			return null;
		}
	}
}
