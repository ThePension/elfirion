using Godot;
using System;

public partial class Entity : StaticBody2D
{
	protected Vector2I ChunkCoord;
	protected Vector2I _LocalPosition;
	
	protected double Health = 100.0;

	protected Label DebugLabel;
	protected ProgressBar HealhBar;

	protected Tween? tween;

	public override void _Ready() {
		DebugLabel = GetNode<Label>("DebugLabel");
		DebugLabel.Text = $"Entity ({GetType().Name}) \n" +
			$"ChunkCoord: {ChunkCoord} \n" +
			$"Global Position: {GlobalPosition} \n" +
			$"Local Position: {_LocalPosition} \n";

		DebugLabel.Visible = Settings.DisplayDebugInfo;

		HealhBar = GetNode<ProgressBar>("HealthBar");
		HealhBar.MaxValue = this.Health;
		HealhBar.Value = this.Health;
		HealhBar.Visible = false;
	}

	public override void _Process(double delta) {
		var camera = GetViewport().GetCamera2D(); // returns the active one

		ZIndex = Mathf.RoundToInt(GlobalPosition.Y - camera.GlobalPosition.Y + 1000);

		DebugLabel.ZIndex = ZIndex + 1;
	}

	public virtual void Init(Vector2I localPosition, Vector2I globalPosition, Vector2I chunkCoord, double health = 100.0) {
		ChunkCoord = chunkCoord;
		_LocalPosition = localPosition;
		GlobalPosition = globalPosition;
		Health = health;
	}

	public virtual void ToggleDisplayDebugInfo() {
		DebugLabel.Visible = !DebugLabel.Visible;
	}

	public virtual void Interact(Entity other) { }

	public virtual void Interact(Player player) {
		if (player.Inventory.SelectedItem != null) {
			player.UseItem(player.Inventory.SelectedItem);
		}
	}

	public void TakeDamage(double damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			// TODO : Emit a signal to the world to remove the entity
			// Die();
		}
	}
	
	public virtual void SaveState() { }


	public virtual void Shake()
	{
		if (tween != null)
		{
			// Stop the animation and reset the position
			tween.Stop();
			Position = new Vector2(Position.X, Position.Y + 1);
			tween = null;
		}

		tween = CreateTween();
		tween.TweenProperty(this, "position", Position + new Vector2(0, -1), 0.1f);
		tween.TweenProperty(this, "position", Position, 0.1f).SetDelay(0.1f);		
	}

	public virtual void OnMouseEntered()
	{
		SignalsBus.Instance?.EmitSignal("EntityMouseEntered", this);
	}

	public virtual void OnMouseExited()
	{
		SignalsBus.Instance?.EmitSignal("EntityMouseExited", this);
	}
	
	public virtual void _on_area_2d_input_event(Node viewport, InputEvent @event, int shape_idx)
	{
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.IsPressed())
			{
				if (mouseButtonEvent.ButtonIndex == MouseButton.Left)
				{
					SignalsBus.Instance?.EmitSignal("EntityInteract", this);
				}
			}
		}
	}

	public virtual void Highlight()
	{
		// Change the color of the tree to indicate it is being hovered
		this.Modulate = new Color(1, 1, 0, 1); // Yellow color
	}

	public virtual void Unhighlight()
	{
		this.Modulate = new Color(1, 1, 1, 1); // White color
	}
}
