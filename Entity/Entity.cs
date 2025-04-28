using Godot;
using System;

public partial class Entity : StaticBody2D
{
	protected Vector2I ChunkCoord;
	protected Vector2I _LocalPosition;
	
	protected double Health = 100.0;

	protected Label DebugLabel;
	protected ProgressBar HealhBar;

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

	public virtual void Interact(Player player) { }
	
	public virtual void SaveState() { }
}
