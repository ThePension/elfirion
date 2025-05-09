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

	private Player player;

	private Vector2I currentChunkCoord = Vector2I.Zero;

	private MainLabel mainDebugLabel;

	private Polygon2D highlightSprite;

	public override void _Ready()
	{
		chunkContainer = GetNode<Node2D>("ChunkContainer");
		mainDebugLabel = GetNode<MainLabel>("Player/DebugInfo/MainLabel");

		player = GetNode<Player>("Player");

		highlightSprite = GetNode<Polygon2D>("ChunkContainer/HighlightSprite");

		// Sbuscribe to the signals
		SignalsBus.Instance?.Connect("EntityInteract", new Callable(this, nameof(OnEntityInteract)));
		SignalsBus.Instance?.Connect("EntityMouseEntered", new Callable(this, nameof(OnEntityMouseEntered)));
		SignalsBus.Instance?.Connect("EntityMouseExited", new Callable(this, nameof(OnEntityMouseExited)));

		RandomNumberGenerator rng = new();
		rng.Randomize();
		
		RandomGlobalSeed = rng.RandiRange(0, 500);
				
		UpdateChunksStacks();
	}

	public override void _Process(double delta)
	{
		var player = GetNode<Player>("Player");
		var newChunkCoord = CoordinatesHelper.WorldToChunk(player.GlobalPosition);

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

		// Mouse interactions

		// TODO : Try to improve this
		// The mouse interactions with entities should not be handled this way
		// It should be handled by the entity itself, using signals
		if (player.Inventory.IsInventoryOpen()) {
			return; // TODO : Try to improve this
		}
		
		Vector2 globalMousePos = GetGlobalMousePosition();

		// Handle mouse position
		Vector2I mousePos = new Vector2I(
			Mathf.FloorToInt(globalMousePos.X),
			Mathf.FloorToInt(globalMousePos.Y)
		);

		// If the mouse is too far from the player, don't do anything, and change the highlight color to red
		if (globalMousePos.DistanceSquaredTo(player.GetGlobalPositionCentered()) > Settings.InteractionDistance * Settings.InteractionDistance)
		{
			// Set the highlight sprite to red
			highlightSprite.Visible = false;
			highlightSprite.Modulate = new Color(1, 0, 0, 0.5f);
			highlightSprite.Position = CoordinatesHelper.WorldToNearestTile(mousePos);
			highlightSprite.Visible = true;

			return;
		}
		else {
			// Set the highlight sprite to white
			highlightSprite.Modulate = new Color(1, 1, 1, 0.5f);
		}

		var mouseChunkCoord = CoordinatesHelper.WorldToChunk(mousePos);

		// if (chunks.ContainsKey(mouseChunkCoord))
		// {
		// 	highlightSprite.Position = CoordinatesHelper.WorldToNearestTile(mousePos);

		// 	highlightSprite.Visible = true;

		// 	Entity? entity = chunks[mouseChunkCoord].GetEntityAt(mousePos);

		// 	if (entity == null) {
		// 		return;
		// 	}

		// 	// If the mouse is clicked, update the tile in the chunk
		// 	// if (Input.IsActionJustPressed("mouse_left_click"))
		// 	// {
		// 	// 	if (player.CanInteract()) {
		// 	// 		entity.Interact(player);
		// 	// 	}

		// 	// }

		// 	// if (Input.IsActionJustPressed("mouse_right_click"))
		// 	// {
		// 	// 	entity.ToggleDisplayDebugInfo();
		// 	// }
		// }
		// else
		// {
		// 	highlightSprite.Visible = false;
		// }
	}

	public void OnEntityInteract(Entity entity)
	{
		if (entity == null)
		{
			GD.PrintErr("Entity is null");
			return;
		}

		if (player.CanInteract() && player.IsEntityReachableByPlayer(entity))
		{
			entity.Interact(player);
		}
	}

	public void OnEntityMouseEntered(Entity entity)
	{
		if (entity == null)
		{
			GD.PrintErr("Entity is null");
			return;
		}

		if (player.CanInteract() && player.IsEntityReachableByPlayer(entity))
		{
			entity.Highlight();
		}
	}

	public void OnEntityMouseExited(Entity entity)
	{
		if (entity == null)
		{
			GD.PrintErr("Entity is null");
			return;
		}

		if (player.CanInteract())
		{
			entity.Unhighlight();
		}
	}

	private void UpdateChunksStacks()
	{
		HashSet<Vector2I> needed = new();

		for (int x = -Settings.ViewDistance; x <= Settings.ViewDistance; x++)
		{
			for (int y = -Settings.ViewDistance; y <= Settings.ViewDistance; y++)
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
		chunk.Position = new Vector2(chunkCoord.X * Settings.ChunkSizePx,
									 chunkCoord.Y * Settings.ChunkSizePx);
		chunkContainer.AddChild(chunk);
		chunks[chunkCoord] = chunk;
	}
}
