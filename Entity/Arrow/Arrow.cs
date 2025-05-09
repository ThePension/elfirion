using Godot;
using System;

public partial class Arrow : RigidBody2D
{
	[Export] public float Speed = 800.0f;
	[Export] public float Damage = 10.0f;
	
	public Vector2 Direction;

	public override void _Ready()
	{
		// Apply initial velocity
		ApplyCentralImpulse(Direction * Speed);
	}

	public override void _PhysicsProcess(double delta)
	{
		// Check if the arrow is out of bounds
		if (GlobalPosition.X < -100 || GlobalPosition.X > GetViewportRect().Size.X + 100 ||
			GlobalPosition.Y < -100 || GlobalPosition.Y > GetViewportRect().Size.Y + 100)
		{
			GD.Print("Arrow out of bounds, destroying...");
			QueueFree();
		}
	}

	private void _on_body_entered(Node body)
	{
		if (body is Entity entity)
		{
			GD.Print($"Arrow hit {entity.Name}");

			// Deal damage or trigger interaction
			// entity.Interact(this); // Or you can create a TakeDamage method
			entity.TakeDamage(Damage);
		}

		// Destroy if it hits anything else
		QueueFree();
	}
}
