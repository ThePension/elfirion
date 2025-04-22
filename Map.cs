using Godot;
using System;
using System.Collections.Generic;

public partial class Map : Node2D
{
	[Export] public PackedScene ChunkScene;

	public int RandomGlobalSeed;

	private Node2D chunkContainer;
	private Dictionary<Vector2I, Chunk> chunks = new();
	private Vector2I currentChunkCoord = Vector2I.Zero;

	private MainLabel mainDebugLabel;

	public override void _Ready()
	{
		chunkContainer = GetNode<Node2D>("ChunkContainer");
		mainDebugLabel = GetNode<MainLabel>("Player/DebugInfo/MainLabel");

		RandomNumberGenerator rng = new();
		rng.Randomize();
		
		RandomGlobalSeed = rng.RandiRange(0, 500);
				
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

			mainDebugLabel.ChunkCount = chunks.Count;
		}
	}

	private Vector2I WorldToChunk(Vector2 pos)
	{
		return new Vector2I(
			Mathf.FloorToInt(pos.X / (float)(Settings.Instance.ChunkSizePx)),
			Mathf.FloorToInt(pos.Y / (float)(Settings.Instance.ChunkSizePx))
		);
	}

	private void UpdateVisibleChunks()
	{
		HashSet<Vector2I> needed = new();

		for (int x = -Settings.Instance.ViewDistance; x <= Settings.Instance.ViewDistance; x++)
		{
			for (int y = -Settings.Instance.ViewDistance; y <= Settings.Instance.ViewDistance; y++)
			{
				Vector2I chunkCoord = currentChunkCoord + new Vector2I(x, y);
				needed.Add(chunkCoord);

				if (!chunks.ContainsKey(chunkCoord))
				{
					var chunk = ChunkScene.Instantiate<Chunk>();
					
					chunk.Position = new Vector2(chunkCoord.X * Settings.Instance.ChunkSizePx,
												 chunkCoord.Y * Settings.Instance.ChunkSizePx);					
												
					chunk.GenerateWorld(chunkCoord, RandomGlobalSeed); // custom method you add to generate noise based on chunkCoord
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
		
		// Pretty print chunks coordinates
		//string chunkCoords = string.Empty;
		//foreach (var key in chunks.Keys)
		//{
			//chunkCoords += $"({key.X}, {key.Y}) ";
		//}
		//GD.Print($"Visible Chunks: {chunkCoords}");
	}
}
