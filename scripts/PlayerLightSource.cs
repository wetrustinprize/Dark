using Godot;

namespace Dark.Player;

public abstract partial class PlayerLightSource : Node
{
	public abstract void OnEquip();
	public abstract void OnUnequip();
	public abstract void OnTrigger();
	public virtual void OnMove(Vector3 vector) { }
	public virtual void OnRotate(Vector2 vector) { }
}