using Godot;
using System;

/// <summary>
/// SignalsBus is a singleton that holds all the signals for the game.
/// It is used to decouple the game logic from the UI and other systems.
/// </summary>
public partial class SignalsBus : Node
{
	public static SignalsBus? Instance { get; private set; }

	// Entity signals
	[Signal]
	public delegate void EntityMouseEnteredEventHandler(Entity entity);

	[Signal]
	public delegate void EntityMouseExitedEventHandler(Entity entity);

	[Signal]
	public delegate void EntityInteractEventHandler(Entity entity);

	// Player signals

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
			// this.SetAsToplevel(true);
			this.SetProcess(false);
			this.SetPhysicsProcess(false);
			this.SetProcessInput(false);
			this.SetProcessUnhandledInput(false);
			this.SetProcessUnhandledKeyInput(false);
		}
		else
		{
			GD.PrintErr("SignalsBus instance already exists. Destroying this instance.");
			this.QueueFree();
		}
	}
}
