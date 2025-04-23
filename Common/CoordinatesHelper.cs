using System;
using Godot;

public static class CoordinatesHelper
{
	/// <summary>
	/// Converts a world position to a chunk coordinate.
	///
	/// For example, if the world position is (18, 18) and the chunk size is 16,
	/// the chunk coordinate will be (1, 1).
	/// </summary>
	/// <param name="pos">The world position.</param>
	/// <returns>The chunk coordinate.</returns>
	public static Vector2I WorldToChunk(Vector2 pos)
	{
		return new Vector2I(
			Mathf.FloorToInt(pos.X / (float)(Settings.ChunkSizePx)),
			Mathf.FloorToInt(pos.Y / (float)(Settings.ChunkSizePx))
		);
	}

	/// <summary>
	/// Converts a world position (in pixels) to a tile coordinate.
	///
	/// For example, if the world position is (33, 18) and the tile size is 16,
	/// the tile coordinate will be (2, 1).
	/// </summary>
	/// <param name="pos">The world position.</param>
	/// <returns>The tile coordinate.</returns>
	public static Vector2I WorldToTile(Vector2 pos)
	{
		return new Vector2I(
			Mathf.FloorToInt(pos.X / (float)(Settings.TileSizePx)),
			Mathf.FloorToInt(pos.Y / (float)(Settings.TileSizePx))
		);
	}

	// Method to get the nearest world position for a tile coordinate, from a world position
	// For example, if the world position is (18, 18) and the tile size is 16,
	// the nearest world position will be (16, 16).
	public static Vector2 WorldToNearestTile(Vector2 pos)
	{
		return new Vector2(
			Mathf.FloorToInt(pos.X / (float)(Settings.TileSizePx)) * Settings.TileSizePx,
			Mathf.FloorToInt(pos.Y / (float)(Settings.TileSizePx)) * Settings.TileSizePx
		);
	}

	/// <summary>
	/// Converts a world position to a local tile coordinate within a chunk.
	/// 
	/// For example, if the world position is (18, 18) and the tile size is 16,
	/// the local tile coordinate will be (2, 2) within the chunk.
	///
	/// </summary>
	/// <param name="pos">The world position.</param>
	/// <returns>The local tile coordinate within the chunk.</returns>
	public static Vector2I WorldToLocal(Vector2 pos)
	{
		var tileCoord = new Vector2I(
			Mathf.FloorToInt((pos.X / (float)(Settings.TileSizePx)) % Settings.ChunkSize),
			Mathf.FloorToInt((pos.Y / (float)(Settings.TileSizePx)) % Settings.ChunkSize)
		);

		// Handle negative coordinates
		if (tileCoord.X < 0)
		{
			tileCoord.X += Settings.ChunkSize;
		}
		if (tileCoord.Y < 0)
		{
			tileCoord.Y += Settings.ChunkSize;
		}

		return tileCoord;
	}
}
