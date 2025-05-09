using Godot;
using System;

public partial class Ennemy : Entity
{
	private Player Player;

	private EntityStates State; 

	private AnimatedSprite2D AnimatedSprite;

	public override void _Ready() {
		base._Ready();

		// Initialize the player reference from the scene tree
		// TODO Find a better way to get the player
		Player = GetNode<Player>("../../../Player");

		AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		AnimatedSprite.Play("idle");

		// callback for the end of the animation
		AnimatedSprite.AnimationFinished += () =>
		{
			if (State == EntityStates.Dying)
			{
				// Remove the entity from the scene
				QueueFree();
				return;
			}

			if (AnimatedSprite.Animation == "attacking_left" || AnimatedSprite.Animation == "attacking_right")
			{
				AnimatedSprite.Play("idle");

				// If the player is still in range, decrease health
				if (GlobalPosition.DistanceTo(Player.GlobalPosition) < Settings.EnnemyAttackRange)
				{
					Player.Health -= Settings.EnnemyAttackDamage;
				}

				State = EntityStates.Idle;
			}
		};

		// Set the initial health bar value
		this.Health = 300.0;
		HealhBar.MaxValue = this.Health;
	}

	public override void _Process(double delta) {
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (State == EntityStates.Dying)
		{
			return;
		}

		base._PhysicsProcess(delta);

		if (Player == null)
		{
			GD.PrintErr("Player is null");
			return;
		}

		float squaredDistToPlayer = Player.SquaredDistanceTo(this);

		// Follow the player
		if (squaredDistToPlayer > Settings.EnnemyAttackRange * Settings.EnnemyAttackRange && squaredDistToPlayer < Settings.EnnemyDetectionRange * Settings.EnnemyDetectionRange && State != EntityStates.Attacking)
		{
			GD.Print("Ennemy is following the player");

			// Move towards the player
			Vector2 direction = (Player.GlobalPosition - GlobalPosition).Normalized();
			Position += direction * (float)(delta * Settings.EnnemyBaseSpeed); // Move towards the player at a speed of 100 units per second
		
		
			// Play walking_left or walking_right animation
			if (Player.GlobalPosition.X < GlobalPosition.X)
			{
				AnimatedSprite.Play("walking_left");
			}
			else
			{
				AnimatedSprite.Play("walking_right");
			}
		}
		else if (squaredDistToPlayer < Settings.EnnemyAttackRange * Settings.EnnemyAttackRange && State != EntityStates.Attacking)
		{
			State = EntityStates.Attacking;

			// Play attacking_left or attacking_right animation
			if (Player.GlobalPosition.X < GlobalPosition.X)
			{
				AnimatedSprite.Play("attacking_left");
			}
			else
			{
				AnimatedSprite.Play("attacking_right");
			}
		}
	}

	public void Init(Vector2I localPosition, Vector2I globalPosition, Vector2I chunkCoord) {
		base.Init(localPosition, globalPosition, chunkCoord, 300.0);
	}

	public override void Interact(Player player)
	{
		base.Interact(player);

		if (player == null)
		{
			GD.PrintErr("Player is null");
			return;
		}

		if (player.Inventory.SelectedItem?.Type == ItemTypes.Sword)
		{
			this.Health -= 50.0; // Decrease tree health by 10
		}
		else
		{
			this.Health -= 25.0; // Decrease tree health by 50
		}

		Shake();

		// Stop attacking animation
		if (AnimatedSprite.Animation == "attacking_left" || AnimatedSprite.Animation == "attacking_right")
		{
			AnimatedSprite.Play("idle");
			State = EntityStates.Idle;
		}

		// Push back the ennemy using velocity
		// TODO : Use velocity instead of position -> Change the node type to do so
		Vector2 direction = (GlobalPosition - player.GlobalPosition).Normalized();
		Position += direction * (float)(Settings.EnnemyPushBackForce); // Move towards the player at a speed of 100 units per second

		HealhBar.Value = this.Health;

		HealhBar.Visible = true;

		if (this.Health > 0) {
			return;
		}

		State = EntityStates.Dying;
		
		player.Inventory.AddItem(ItemTypes.Stick, 4);
		player.Inventory.AddItem(ItemTypes.Log, 2);

		// Hide the health bar
		HealhBar.Visible = false;

		// Play dying_left or dying_right animation
		if (Player.GlobalPosition.X < GlobalPosition.X)
		{
			AnimatedSprite.Play("dying_left");
		}
		else
		{
			AnimatedSprite.Play("dying_right");
		}
	}
}
