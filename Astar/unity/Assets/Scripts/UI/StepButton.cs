using UnityEngine;
using System.Collections;

public class StepButton : Touchable
{
	public Sprite up;
	public Sprite ovr;

	private SpriteRenderer _renderer;

	void Start () 
	{
		_renderer = gameObject.GetComponent<SpriteRenderer>();

		enableMouseOvr();
		enableTap();
	}

	protected override void OnTap(Vector2 fingerPos)
	{
		Main.instance.Grid.PathFinder.Step();
	}

	protected override void OnMouseOvr()
	{
		_renderer.sprite = ovr;
	}

	protected override void OnMouseOff()
	{
		_renderer.sprite = up;
	}
}
