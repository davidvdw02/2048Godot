using Godot;
using System;

public partial class main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }

	private int _score;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void game_over()
	{
	GetNode<Timer>("MobTimer").Stop();
	GetNode<Timer>("ScoreTimer").Stop();
	
	GetNode<hud>("HUD").ShowGameOver();
	
	GetNode<AudioStreamPlayer>("Music").Stop();
	GetNode<AudioStreamPlayer>("DeathSound").Play();
	}

	public void NewGame() 
	{
	_score = 0;
	
	var player = GetNode<player>("Player");
	var startPosition = GetNode<Marker2D>("StartPosition");
	player.Start(startPosition.Position);

	// Note that for calling Godot-provided methods with strings,
	// we have to use the original Godot snake_case name.
	GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

	GetNode<Timer>("StartTimer").Start();
	
	var hud = GetNode<hud>("HUD");
	hud.UpdateScore(_score);
	hud.ShowMessage("Get Ready!");
	
	GetNode<AudioStreamPlayer>("Music").Play();
	
	}
	
	private void _on_mob_timer_timeout()
	{
	// Note: Normally it is best to use explicit types rather than the `var`
	// keyword. However, var is acceptable to use here because the types are
	// obviously Mob and PathFollow2D, since they appear later on the line.

	// Create a new instance of the Mob scene.
	mob mob = MobScene.Instantiate<mob>();

	// Choose a random location on Path2D.
	var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
	mobSpawnLocation.ProgressRatio = GD.Randf();

	// Set the mob's direction perpendicular to the path direction.
	float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

	// Set the mob's position to a random location.
	mob.Position = mobSpawnLocation.Position;

	// Add some randomness to the direction.
	direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
	mob.Rotation = direction;

	// Choose the velocity.
	var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
	mob.LinearVelocity = velocity.Rotated(direction);

	// Spawn the mob by adding it to the Main scene.
	AddChild(mob);
	}
	
	private void _on_score_timer_timeout()
	{
	_score++;
	
	GetNode<hud>("HUD").UpdateScore(_score);
	}
	
	private void _on_start_timer_timeout()
	{
	GetNode<Timer>("MobTimer").Start();
	GetNode<Timer>("ScoreTimer").Start();
	}
	

	

	
}



