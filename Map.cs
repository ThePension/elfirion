using Godot;
using System;
using System.Collections.Generic;

public partial class Map : Node2D
{
	[Export] public PackedScene ChunkScene;
	[Export] public int ChunkSize = 16;
	[Export] public int TileSizePx = 16;
	[Export] public int ViewDistance = 3;


	public int ChunkSizePx = 64;

	private Node2D chunkContainer;
	private Dictionary<Vector2I, Chunk> chunks = new();
	private Vector2I currentChunkCoord = Vector2I.Zero;

	public override void _Ready()
	{
		chunkContainer = GetNode<Node2D>("ChunkContainer");
				
		UpdateVisibleChunks();
	}

	public override void _Process(double delta)
	{
		var player = GetNode<Node2D>("Player");
		var newChunkCoord = WorldToChunk(player.GlobalPosition);

		if (newChunkCoord != currentChunkCoord)
		{
			currentChunkCoord = newChunkCoord;
			UpdateVisibleChunks();
		}
	}

	private Vector2I WorldToChunk(Vector2 pos)
	{
		return new Vector2I(
			Mathf.FloorToInt(pos.X / (float)(ChunkSize * TileSizePx)),
			Mathf.FloorToInt(pos.Y / (float)(ChunkSize * TileSizePx))
		);
	}

	private void UpdateVisibleChunks()
	{
		HashSet<Vector2I> needed = new();

		for (int x = -ViewDistance; x <= ViewDistance; x++)
		{
			for (int y = -ViewDistance; y <= ViewDistance; y++)
			{
				Vector2I chunkCoord = currentChunkCoord + new Vector2I(x, y);
				needed.Add(chunkCoord);

				if (!chunks.ContainsKey(chunkCoord))
				{
					var chunk = ChunkScene.Instantiate<Chunk>();
					
					chunk.Position = new Vector2(chunkCoord.X * ChunkSize * TileSizePx,
												 chunkCoord.Y * ChunkSize * TileSizePx);					
												
					chunk.GenerateWorld(chunkCoord); // custom method you add to generate noise based on chunkCoord
					chunkContainer.AddChild(chunk);
					chunks[chunkCoord] = chunk;
				}
			}
		}

		// Remove chunks that are no longer needed
		var toRemove = new List<Vector2I>();
		foreach (var key in chunks.Keys)
		{
			if (!needed.Contains(key))
			{
				chunks[key].QueueFree();
				toRemove.Add(key);
			}
		}
		foreach (var key in toRemove)
			chunks.Remove(key);
	}
}
