using UnityEngine;
using System.Collections;

public class StepButton : Touchable
{
	void Start () 
	{
		enableTap();
	}

	protected override void OnTap(Vector2 fingerPos)
	{
		Main.instance.Grid.PathFinder.Step();
	}
}
