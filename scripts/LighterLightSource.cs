using Godot;
using System;

namespace Dark.Player;

public partial class LighterLightSource : PlayerLightSource
{
    #region Variables

    [Export] private MeshInstance3D _flameMesh;
    [Export] private OmniLight3D _light;

    #region Light Flicker

    private const float _lightFlickerSpeed = 2.0f;
    private const float _lightMaxEnergy = 0.5f;
    private const float _lightMinEnergy = 0.05f;

    #endregion

    #region Flame Sway

    private Vector3 _flameSwayVector = Vector3.Zero;

    private Vector3 _movementVector = Vector3.Zero;
    private Vector2 _rotationVector = Vector2.Zero;

    private const float _flameMovementMaxDistance = 0.05f;
    private const float _flameRotationMaxDistance = 1f;
    private const float _flameSwaySpeed = 10f;

    private ShaderMaterial _material;

    #endregion

    #endregion

    public override void _Ready()
    {
        _material = (ShaderMaterial)_flameMesh.GetSurfaceOverrideMaterial(0);
    }

    public override void _Process(double delta)
    {
        ProcessFlameSway(delta);
        ProcessLightFlicker(delta);
    }

    void ProcessLightFlicker(double delta)
    {
        float random = (float)GD.RandRange(_lightMinEnergy, _lightMaxEnergy);

        float energy = Mathf.Lerp(_light.LightEnergy, random, (float)(delta * _lightFlickerSpeed));
        _light.LightEnergy = energy;

        float energyPercentage = (energy - _lightMinEnergy) / (_lightMaxEnergy - _lightMinEnergy);
        _material.SetShaderParameter("brightness", energyPercentage);
    }

    void ProcessFlameSway(double delta)
    {
        _flameSwayVector = _flameSwayVector.Lerp(
            _movementVector + new Vector3(_rotationVector.X, 0, _rotationVector.Y
        ), (float)(delta * _flameSwaySpeed));

        _material.SetShaderParameter("movement", _flameSwayVector);

        _rotationVector = _rotationVector.Lerp(Vector2.Zero, (float)(delta * _flameSwaySpeed));
    }

    #region PlayerLightSource methods

    public override void OnEquip()
    {
    }

    public override void OnMove(Vector3 vector)
    {
        _movementVector = new(
            -vector.X * _flameMovementMaxDistance,
            -vector.Y * _flameMovementMaxDistance,
            -vector.Z * _flameMovementMaxDistance
        );
    }

    public override void OnRotate(Vector2 vector)
    {
        _rotationVector = new(
            vector.X * _flameRotationMaxDistance,
            vector.Y * _flameRotationMaxDistance
        );
    }

    public override void OnTrigger()
    {
        throw new NotImplementedException();
    }

    public override void OnUnequip()
    {
        throw new NotImplementedException();
    }

    #endregion

}