using Godot;

// TODO : Refactor this class, should inherit from Entity
public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;
	
	[Export]
	public int SprintSpeed { get; set; } = 800;
	
	public Vector2 ScreenSize;

	public Inventory Inventory { get; set; }

	public ItemTypes SelectedItemType { get; set; } = ItemTypes.None;
	
	public override void _Ready()
	{
		// Initialize the inventory
		Inventory = GetNode<Inventory>("Inventory");

		// Add pickaxe and axe to the inventory
		Inventory.AddItem(ItemTypes.Pickaxe);
		Inventory.AddItem(ItemTypes.Axe);

		ScreenSize = GetViewportRect().Size;
		
		Speed = Settings.PlayerBaseSpeed;
		SprintSpeed = Settings.PlayerSprintSpeed;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Inventory.IsInventoryOpen())
		{
			return; // TODO : Try to improve this
		}

		var camera = GetViewport().GetCamera2D();
		ZIndex = Mathf.RoundToInt(GlobalPosition.Y - camera.GlobalPosition.Y + 1000);

		var input = Vector2.Zero;

		if (Input.IsActionPressed("move_right")) input.X += 1;
		if (Input.IsActionPressed("move_left")) input.X -= 1;
		if (Input.IsActionPressed("move_down")) input.Y += 1;
		if (Input.IsActionPressed("move_up")) input.Y -= 1;

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (input != Vector2.Zero)
		{
			input = input.Normalized();

			if (Input.IsActionPressed("sprint"))
			{
				Velocity = input * SprintSpeed;
				animatedSprite2D.SpeedScale = 1.5f;
			}
			else
			{
				Velocity = input * Speed;
				animatedSprite2D.SpeedScale = 1.0f;
			}

			// Set animation based on direction
			if (Mathf.Abs(input.X) > Mathf.Abs(input.Y))
			{
				animatedSprite2D.Play(input.X > 0 ? "walking_right" : "walking_left");
			}
			else
			{
				animatedSprite2D.Play(input.Y > 0 ? "walking_down" : "walking_up");
			}
		}
		else
		{
			Velocity = Vector2.Zero;
			animatedSprite2D.Stop();
		}

		MoveAndSlide();
	}

	public Vector2 GetGlobalPositionCentered()
	{
		return GlobalPosition + new Vector2(Settings.TileSizePx / 2, Settings.TileSizePx / 2);
	}

}
