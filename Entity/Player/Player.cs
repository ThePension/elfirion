using System;
using Godot;

// TODO : Refactor this class, should inherit from Entity
public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;
	
	[Export]
	public int SprintSpeed { get; set; } = 800;

	[Export]
	public float Health { get; set; } = 100.0f;
	public float MaxHealth { get; set; } = 100.0f;

	private ProgressBar HealthBar;
	
	[Export]
	public float Energy { get; set; } = 100.0f;
	private ProgressBar EnergyBar;
	
	[Export]
	public float Food { get; set; } = 100.0f;
	private ProgressBar FoodBar;

	private Directions Direction;
	private EntityStates State;

	private AnimatedSprite2D Animations;

	private Area2D MeleeAttack;


	// Timer for energy regeneration
	private Timer EnergyRegenerationTimer;
	
	public Vector2 ScreenSize;

	public Inventory Inventory { get; set; }

	public ItemTypes SelectedItemType { get; set; } = ItemTypes.None;
	
	public override void _Ready()
	{	
		HealthBar = GetNode<ProgressBar>("HealthBar");
		HealthBar.MaxValue = this.Health;
		HealthBar.Value = this.Health;
		
		EnergyBar = GetNode<ProgressBar>("EnergyBar");
		EnergyBar.MaxValue = this.Energy;
		EnergyBar.Value = this.Energy;
		
		FoodBar = GetNode<ProgressBar>("FoodBar");
		FoodBar.MaxValue = this.Food;
		FoodBar.Value = this.Food;

		// Initialize the energy regeneration timer
		EnergyRegenerationTimer = GetNode<Timer>("EnergyRegenerationTimer");
		EnergyRegenerationTimer.WaitTime = 1.0f; // 1 second
		EnergyRegenerationTimer.OneShot = true;

		Animations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		MeleeAttack = GetNode<Area2D>("MeleeAttack");

		var meleeAttackAnimation = MeleeAttack.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Add a callback for when the animation is finished
		meleeAttackAnimation.AnimationFinished += () =>
		{
			MeleeAttack.Monitoring = false;
			MeleeAttack.Visible = false;
		};

		// Callback for when the animation is finished
		Animations.AnimationFinished += () =>
		{
			if (State == EntityStates.Interacting)
			{
				State = EntityStates.Idle;
				Animations.Stop();

				// Set the default frame to the idle animation
				Animations.Frame = 0;
			}
		};

		// Initialize the inventory
		Inventory = GetNode<Inventory>("Inventory");

		// Add pickaxe and axe to the inventory
		Inventory.AddItem(ItemTypes.Pickaxe);
		Inventory.AddItem(ItemTypes.Axe);
		Inventory.AddItem(ItemTypes.Sword);

		ScreenSize = GetViewportRect().Size;
		
		Speed = Settings.PlayerBaseSpeed;
		SprintSpeed = Settings.PlayerSprintSpeed;

		State = EntityStates.Idle;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		// Regenerate energy if the timer is running
		if (EnergyRegenerationTimer.IsStopped())
		{
			Energy += Settings.EnergyRegenerationRate * (float)delta;
			EnergyBar.Value = Energy;

			if (Energy >= MaxHealth)
			{
				Energy = MaxHealth;
			}
		}

		if (Inventory.IsInventoryOpen())
		{
			return; // TODO : Try to improve this
		}

		// Handle melee attack
		if (Input.IsActionJustPressed("mouse_left_click") && Inventory.SelectedItem?.Category == ItemCategories.Weapon)
		{
			PerformMeleeAttack();
		}

		var camera = GetViewport().GetCamera2D();
		ZIndex = Mathf.RoundToInt(GlobalPosition.Y - camera.GlobalPosition.Y + 1000);

		var input = Vector2.Zero;

		if (Input.IsActionPressed("move_right")) input.X += 1;
		if (Input.IsActionPressed("move_left")) input.X -= 1;
		if (Input.IsActionPressed("move_down")) input.Y += 1;
		if (Input.IsActionPressed("move_up")) input.Y -= 1;

		if (input != Vector2.Zero)
		{
			input = input.Normalized();

			State = EntityStates.Moving;

			if (Input.IsActionPressed("sprint") && Energy > 0)
			{
				Velocity = input * SprintSpeed;
				Animations.SpeedScale = 1.5f;

				// Decrease energy when sprinting
				Energy -= Settings.EnergySprintCostRate * (float)delta;
				EnergyBar.Value = Energy;

				// Run the timer for 3 seconds
				EnergyRegenerationTimer.Start(Settings.EnergyRegenerationDelayInSeconds);
				
				// Decrease food when sprinting
				Food -= Settings.FoodSprintCostRate * (float)delta;
				FoodBar.Value = Food;
			}
			else
			{
				Velocity = input * Speed;
				Animations.SpeedScale = 1.0f;

				// Decrease food when walking
				Food -= Settings.FoodWalkCostRate * (float)delta;
				FoodBar.Value = Food;
			}

			// Set animation based on direction
			if (Mathf.Abs(input.X) > Mathf.Abs(input.Y))
			{
				Animations.Play(input.X > 0 ? "walking_right" : "walking_left");

				Direction = input.X > 0 ? Directions.Right : Directions.Left;
			}
			else
			{
				Animations.Play(input.Y > 0 ? "walking_down" : "walking_up");

				Direction = input.Y > 0 ? Directions.Down : Directions.Up;
			}
		}
		else
		{
			Velocity = Vector2.Zero;

			if (State == EntityStates.Moving)
			{
				State = EntityStates.Idle;
				Animations.Stop();
			}
		}

		UpdateUI();

		MoveAndSlide();
	}

	public void UpdateUI()
	{
		// Update the health bar
		HealthBar.Value = Health;

		// Update the energy bar
		EnergyBar.Value = Energy;

		// Update the food bar
		FoodBar.Value = Food;
	}

	private void PerformMeleeAttack()
	{
		// if (State != EntityStates.Idle && State != EntityStates.Moving)
		// {
		// 	return; // Don't perform melee attack if not idle or moving
		// }

		if (Inventory.SelectedItem == null)
		{
			GD.PrintErr("No item selected for melee attack.");
			return;
		}

		var shape = (RectangleShape2D)MeleeAttack.GetNode<CollisionShape2D>("CollisionShape2D").Shape;
		shape.Size = new Vector2(Inventory.SelectedItem.Range, Inventory.SelectedItem.Range / 4.0f); // Range x Arc thickness

		// Set the attack direction based on mouse position
		float angleToMouse = (GetGlobalMousePosition() - GlobalPosition).Angle();

		// Add 90 degrees to the angle to make the attack direction match the sprite
		angleToMouse += Mathf.Pi / 2;

		MeleeAttack.Rotation = angleToMouse;

		// Enable the melee area to detect hits
		MeleeAttack.Visible = true;
		MeleeAttack.Monitoring = true;
		MeleeAttack.Monitorable = true;

		// Play the attack animation
		var animation = MeleeAttack.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		animation.Play("attack");;
	}

	public Vector2 GetGlobalPositionCentered()
	{
		return GlobalPosition + new Vector2(Settings.TileSizePx / 2, Settings.TileSizePx / 2);
	}
	
	public void UseItem(Item item) {
		switch (item.Type)
		{
			case ItemTypes.Axe:
				Animations.Play($"using_axe_{Direction.ToString().ToLower()}");
				State = EntityStates.Interacting;
				break;
			case ItemTypes.Sword:
				State = EntityStates.Attacking;
				break;
			default:
				GD.PrintErr("Unknown item type.");
				break;
		}
	}

	public bool CanInteract() {
		return State == EntityStates.Idle || State == EntityStates.Moving;
	}

	public float SquaredDistanceTo(Entity entity)
	{
		return GetGlobalPositionCentered().DistanceSquaredTo(entity.GlobalPosition);
	}

	public bool IsEntityReachableByPlayer(Entity entity)
	{
		float itemRange = Inventory.SelectedItem?.Range ?? 1.0f;

		return entity.GlobalPosition.DistanceSquaredTo(GetGlobalPositionCentered()) < 
			Mathf.Pow(itemRange * Settings.TileSizePx, 2);
	}

	// Handle area collision
	private void _on_MeleeAttack_body_entered(Node body)
	{
		if (body is Ennemy ennemy)
		{
			// Deal damage or trigger interaction
			ennemy.Interact(this);
		}
	}
}
