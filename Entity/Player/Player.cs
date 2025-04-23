using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 400;
	
	[Export]
	public int SprintSpeed { get; set; } = 800;
	
	public Vector2 ScreenSize;
	
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		
		Speed = Settings.Instance.PlayerBaseSpeed;
		SprintSpeed = Settings.Instance.PlayerSprintSpeed;
	}
	
	public override void _Process(double delta)
	{
		var camera = GetViewport().GetCamera2D(); // returns the active one

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
		
		
		//var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized();
			
			// Handle sprint
			if (Input.IsActionPressed("sprint"))
			{
				velocity *= SprintSpeed;
			}
			else
			{
				velocity *= Speed;
			}
			
			//animatedSprite2D.Play();
		}
		else
		{
			//animatedSprite2D.Stop();
		}
		
		Position += velocity * (float)delta;
	}
}
