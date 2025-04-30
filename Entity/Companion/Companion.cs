using Godot;
using System;

public partial class Companion : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;
	
	[Export]
	public float FollowDistance { get; set; } = 50f; // Minimum distance to player
	
	private Player player;
	private AnimatedSprite2D animatedSprite2D;
	private NavigationAgent2D agent;

	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("Map/Player"); // Path to your player node
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		agent = GetNode<NavigationAgent2D>("NavigationAgent2D");

		Speed = Settings.PlayerBaseSpeed;
		
		agent.MaxSpeed = Speed;
		agent.PathDesiredDistance = 8f; // Acceptable "close to target" distance
		agent.TargetDesiredDistance = FollowDistance;
	}
	
	public override void _PhysicsProcess(double delta)
{
	var camera = GetViewport().GetCamera2D();
	ZIndex = Mathf.RoundToInt(GlobalPosition.Y - camera.GlobalPosition.Y + 1000);

	if (player == null || agent == null)
		return;

	float distance = GlobalPosition.DistanceTo(player.GlobalPosition);

	if (distance > FollowDistance)
	{
		// Dynamically update target position
		if (player.GlobalPosition.DistanceTo(agent.TargetPosition) > 8f)
		{
			agent.TargetPosition = player.GlobalPosition;
		}

		Vector2 nextPathPos = agent.GetNextPathPosition();
		Vector2 direction = (nextPathPos - GlobalPosition).Normalized();
		Velocity = direction * Speed;

		// === HANDLE ANIMATION BASED ON DIRECTION ===
		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			animatedSprite2D.Play(direction.X > 0 ? "walking_right" : "walking_left");
		}
		else
		{
			animatedSprite2D.Play(direction.Y > 0 ? "walking_down" : "walking_up");
		}
	}
	else
	{
		Velocity = Vector2.Zero;
		animatedSprite2D.Stop();
	}

	MoveAndSlide();
}

}
