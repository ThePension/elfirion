using Godot;
using System;
using System.Collections.Generic;

public partial class Map : Node2D
{
	[Export] public PackedScene ChunkScene;

	public int RandomGlobalSeed;

	private Node2D chunkContainer;
	private Dictionary<Vector2I, Chunk> chunks = new();
	private Stack<Vector2I> chunksToAdd = new();
	private Stack<Vector2I> chunksToRemove = new();

	private Vector2I currentChunkCoord = Vector2I.Zero;

	private MainLabel mainDebugLabel;

	private Polygon2D highlightSprite;

	public override void _Ready()
	{
		chunkContainer = GetNode<Node2D>("ChunkContainer");
		mainDebugLabel = GetNode<MainLabel>("Player/DebugInfo/MainLabel");

		highlightSprite = GetNode<Polygon2D>("ChunkContainer/HighlightSprite");

		RandomNumberGenerator rng = new();
		rng.Randomize();
		
		RandomGlobalSeed = rng.RandiRange(0, 500);
				
		UpdateChunksStacks();
	}

	public override void _Process(double delta)
	{
		var player = GetNode<Node2D>("Player");
		var newChunkCoord = WorldToChunk(player.GlobalPosition);

		if (newChunkCoord != currentChunkCoord)
		{
			currentChunkCoord = newChunkCoord;
			UpdateChunksStacks();

			mainDebugLabel.ChunkCount = chunks.Count;
		}

		if (chunksToAdd.Count > 0)
		{
			var chunkCoord = chunksToAdd.Pop();
			CreateAndAddChunk(chunkCoord, RandomGlobalSeed);
		}

		if (chunksToRemove.Count > 0)
		{
			var chunkCoord = chunksToRemove.Pop();
			if (chunks.ContainsKey(chunkCoord))
			{
				chunks[chunkCoord].QueueFree();
				chunks.Remove(chunkCoord);
			}
		}
		
		Vector2 globalMousePos = GetGlobalMousePosition();

		// Handle mouse position
		Vector2I mousePos = new Vector2I(
			Mathf.FloorToInt(globalMousePos.X),
			Mathf.FloorToInt(globalMousePos.Y)
		);

		var mouseChunkCoord = WorldToChunk(mousePos);

		if (chunks.ContainsKey(mouseChunkCoord))
		{
			highlightSprite.Position = WorldToTile(mousePos) * Settings.Instance.TileSizePx;

			highlightSprite.Visible = true;

			// If the mouse is clicked, update the tile in the chunk
			if (Input.IsActionJustPressed("mouse_left_click"))
			{
				chunks[mouseChunkCoord].UpdateTileInChunk(mousePos);
			}
		}
		else
		{
			highlightSprite.Visible = false;
		}
	}

	private Vector2I WorldToChunk(Vector2 pos)
	{
		return new Vector2I(
			Mathf.FloorToInt(pos.X / (float)(Settings.Instance.ChunkSizePx)),
			Mathf.FloorToInt(pos.Y / (float)(Settings.Instance.ChunkSizePx))
		);
	}

	private Vector2I WorldToTile(Vector2 pos)
	{
		return new Vector2I(
			Mathf.FloorToInt(pos.X / (float)(Settings.Instance.TileSizePx)),
			Mathf.FloorToInt(pos.Y / (float)(Settings.Instance.TileSizePx))
		);
	}

	/// <summary>
	/// Converts a global chunk coordinate to a local chunk coordinate.
	/// For example, if the chunk coordinate is (18, 18) and the chunk size is 16,
	/// the local chunk coordinate will be (2, 2).
	/// </summary>
	private Vector2I GlobalChunkCoordoToLocal(Vector2I chunkCoord)
	{
		return new Vector2I(
			chunkCoord.X % Settings.Instance.ChunkSize,
			chunkCoord.Y % Settings.Instance.ChunkSize
		);
	}

	private void UpdateChunksStacks()
	{
		HashSet<Vector2I> needed = new();

		for (int x = -Settings.Instance.ViewDistance; x <= Settings.Instance.ViewDistance; x++)
		{
			for (int y = -Settings.Instance.ViewDistance; y <= Settings.Instance.ViewDistance; y++)
			{
				Vector2I chunkCoord = currentChunkCoord + new Vector2I(x, y);
				needed.Add(chunkCoord);

				if (!chunks.ContainsKey(chunkCoord) && !chunksToAdd.Contains(chunkCoord))
				{
					// Add chunk to the list of chunks to add
					chunksToAdd.Push(chunkCoord);
				}
			}
		}

		// Update to remove stack
		foreach (var key in chunks.Keys)
		{
			if (!needed.Contains(key))
			{
				chunksToRemove.Push(key);
			}
		}
	}

	public void CreateAndAddChunk(Vector2I chunkCoord, int seed)
	{
		var chunk = ChunkScene.Instantiate<Chunk>();
		chunk.Init(chunkCoord, seed);
		chunk.Position = new Vector2(chunkCoord.X * Settings.Instance.ChunkSizePx,
									 chunkCoord.Y * Settings.Instance.ChunkSizePx);
		chunkContainer.AddChild(chunk);
		chunks[chunkCoord] = chunk;
	}
}
