using System;
using Godot;

public partial class Settings : Node
{
	public static Settings Instance { get; private set; }

	public static int ChunkSize = 16;
	public static int TileSizePx = 16;
	public static int ViewDistance = 1;
	public static int ChunkSizePx;

	// Player related settings
	public static int PlayerBaseSpeed = 40;
	public static int PlayerSprintSpeed = 80;
	public static float InteractionDistance = TileSizePx * 2.5f; // Distance to interact with objects
	public static float EnergySprintCostRate = 25.0f; // Cost of energy to sprint
	public static float EnergyRegenerationRate = 10.0f; // Regenerate 5 energy per second
	public static float EnergyRegenerationDelayInSeconds = 2.0f; // Delay before energy starts regenerating

	public static float FoodSprintCostRate = 0.5f; // Cost of food to sprint
	public static float FoodWalkCostRate = 0.1f; // Cost of food to walk

	// Ennemy related settings
	public static int EnnemyBaseSpeed = 40;

	public static int EnnemyDetectionRange = 100; // Distance at which the enemy can detect the player
	public static int EnnemyAttackDamage = 10; // Damage dealt by the enemy
	public static int EnnemyAttackRange = 50; // Distance at which the enemy can attack the player

	public static float EnnemyPushBackForce = 10.0f; // Force applied to the player when hit by the enemy

	// Entity related settings
	public static bool DisplayDebugInfo = false;

	public static float TreeSpawnRate = 0.05f; // 10% chance to spawn a tree
	public static float RockSpawnRate = 0.05f; // 10% chance to spawn a rock
	public static float BushSpawnRate = 0.05f; // 10% chance to spawn a bush
	public static float EnnemySpawnRate = 0.005f; // 10% chance to spawn an enemy
	
	public override void _Ready() {
		Instance = this;
		ChunkSizePx = ChunkSize * TileSizePx;
	}
}
