using Godot;
using System;

public partial class Hud : CanvasLayer
{
    public static event Action PauseGame;
    private Label _scoreLabel;
	private AudioStreamPlayer _soundCoin;
	private AudioStreamPlayer _soundRedCoin;
    public static Button PauseButton;
    public override void _Ready()
    {
        _scoreLabel = GetNode<Label>("Score");
		_soundCoin = GetNode<AudioStreamPlayer>("Coin");
		_soundRedCoin = GetNode<AudioStreamPlayer>("RedCoin");
        PauseButton = GetNode<Button>("Pause");

        PauseButton.ButtonDown += () =>
        {
            PauseGame?.Invoke();
            Main.IsPaused = true;
            PauseButton.Visible = false;
        };
        Main.ResumeGame += () =>
            PauseButton.Visible = true;
        Pipes.RefreshScore += () =>
		{
            _scoreLabel.Text = Convert.ToString(World.Score);
			if (Array.IndexOf(World.SpeedStages, World.Score) == -1)
				_soundCoin.Play();
			else
				_soundRedCoin.Play();
		};
    }
}