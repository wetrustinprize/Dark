using Godot;

namespace Dark.Player;

public partial class PlayerController : CharacterBody3D
{
	#region Variables

	[Export] private CameraLightSource _cameraLightSource;

	#region Rotation

	[Export] private Camera3D _camera;
	[Export] private Camera3D _equippedCamera;
	[Export] private Node3D _equippedPivot;

	private const float _lookSpeed = 0.005f;
	private const float _cameraClamp = 90.0f;

	private const float _swaySpeed = 0.0005f;
	private const float _equippedPivotClamp = 45.0f;

	#endregion

	#region Movement

	private const float _movementSpeed = 5.0f;

	#endregion

	// Private
	private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	#endregion

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)
	{
		SmoothReturnEquippedPivot(delta);

		_equippedCamera.GlobalTransform = _camera.GlobalTransform;
	}

	public override void _PhysicsProcess(double delta)
	{
		CalculateMovement(delta);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseInputEvent && Input.MouseMode == Input.MouseModeEnum.Captured)
			CalculateRotation(mouseInputEvent);

		if (@event.IsActionPressed("use_equipped"))
			_cameraLightSource.OnTrigger();
	}

	#region Rotation

	void CalculateRotation(InputEventMouseMotion @event)
	{
		Vector2 lookRotationVector = new(
			-@event.Relative.X * _lookSpeed,
			-@event.Relative.Y * _lookSpeed
		);

		// Rotate the player's body.
		RotateY(lookRotationVector.X);

		// Rotate the camera.
		_camera.RotateX(lookRotationVector.Y);
		_camera.RotationDegrees = new Vector3(
			Mathf.Clamp(_camera.RotationDegrees.X, -_cameraClamp, _cameraClamp),
			_camera.RotationDegrees.Y,
			_camera.RotationDegrees.Z
		);

		// Rotate the equipped pivot.
		_equippedPivot.RotateX(-@event.Relative.Y * _swaySpeed);
		_equippedPivot.RotateY(-@event.Relative.X * _swaySpeed);
		_equippedPivot.RotationDegrees = new Vector3(
			Mathf.Clamp(_equippedPivot.RotationDegrees.X, -_equippedPivotClamp, _equippedPivotClamp),
			Mathf.Clamp(_equippedPivot.RotationDegrees.Y, -_equippedPivotClamp, _equippedPivotClamp),
			0
		);
	}

	void SmoothReturnEquippedPivot(double delta)
	{
		_equippedPivot.RotationDegrees = new Vector3(
			Mathf.Lerp(_equippedPivot.RotationDegrees.X, 0, 0.1f),
			Mathf.Lerp(_equippedPivot.RotationDegrees.Y, 0, 0.1f),
			_equippedPivot.RotationDegrees.Z
		);
	}

	#endregion

	void CalculateMovement(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= _gravity * (float)delta;

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * _movementSpeed;
			velocity.Z = direction.Z * _movementSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _movementSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _movementSpeed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
