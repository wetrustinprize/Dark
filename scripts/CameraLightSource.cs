using Godot;
using System;

namespace Dark.Player;

public partial class CameraLightSource : Node3D
{
	#region Variables

	[Export] private Light3D _light;
	[Export] private AudioStreamPlayer _audio;

	private const float _lightEnergy = 20;
	private const float _lightDim = 0.1f;

	private Tween _lightTween;

	#endregion

	public void OnTrigger()
	{
		_lightTween?.Stop();

		_audio.Play();

		_light.LightEnergy = _lightEnergy;

		_lightTween = GetTree().CreateTween();
		_lightTween.TweenProperty(_light, "light_energy", 0, _lightDim);
	}
}
