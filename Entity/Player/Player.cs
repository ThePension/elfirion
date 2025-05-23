using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;
	
	[Export]
	public int SprintSpeed { get; set; } = 800;
	
	public Vector2 ScreenSize;
	
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		
		Speed = Settings.PlayerBaseSpeed;
		SprintSpeed = Settings.PlayerSprintSpeed;
	}
	
	public override void _Process(double delta)
	{
		var camera = GetViewport().GetCamera2D(); // Get the active camera

		ZIndex = Mathf.RoundToInt(GlobalPosition.Y - camera.GlobalPosition.Y + 1000);
		
		var velocity = Vector2.Zero; // The player's movement vector.
		
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}
		
		
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized();
			
			// Handle sprint
			if (Input.IsActionPressed("sprint"))
			{
				velocity *= SprintSpeed;

				// Update animation speed
				animatedSprite2D.SpeedScale = 1.5f;
			}
			else
			{
				velocity *= Speed;
				
				// Reset animation speed
				animatedSprite2D.SpeedScale = 1.0f;
			}
			
			// Play different directions animated based on velocity (walk_up, walk_down, walk_left, walk_right)
			if (Mathf.Abs(velocity.X) > Mathf.Abs(velocity.Y))
			{
				if (velocity.X > 0)
				{
					animatedSprite2D.Play("walking_right");
				}
				else
				{
					animatedSprite2D.Play("walking_left");
				}
			}
			else
			{
				if (velocity.Y > 0)
				{
					animatedSprite2D.Play("walking_down");
				}
				else
				{
					animatedSprite2D.Play("walking_up");
				}
			}

		}
		else
		{
			animatedSprite2D.Stop();
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
}
