using Godot;
using System;

namespace Dark.Player;

public partial class CameraLightSource : PlayerLightSource
{
	#region Variables

	[Export] private Light3D _light;
	[Export] private AudioStreamPlayer _audio;

	private const float _lightEnergy = 20;
	private const float _lightDim = 0.1f;

	private Tween _lightTween;

	#endregion

	#region PlayerLightSource

	public override void OnEquip()
	{
		throw new NotImplementedException();
	}

	public override void OnTrigger()
	{
		_lightTween?.Stop();

		_audio.Play();

		_light.LightEnergy = _lightEnergy;

		_lightTween = GetTree().CreateTween();
		_lightTween.TweenProperty(_light, "light_energy", 0, _lightDim);
	}

	public override void OnUnequip()
	{
		_lightTween?.Stop();

		_light.LightEnergy = 0;
	}

	#endregion
}
