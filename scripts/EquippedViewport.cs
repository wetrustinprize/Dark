using Godot;
using System;

namespace Dark.Internals;

public partial class EquippedViewport : SubViewport
{
	public override void _Ready()
	{
		GetViewport().Connect(Viewport.SignalName.SizeChanged, new Callable(this, nameof(UpdateViewport)));

		UpdateViewport();
	}

	void UpdateViewport()
	{
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;

		Size = (Vector2I)viewportSize;
	}
}
