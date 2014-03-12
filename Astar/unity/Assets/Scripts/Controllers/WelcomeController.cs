using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WelcomeController : MonoSingleton<WelcomeController>
{
	private bool _init = false;
	void Start()
	{
		init();
	}

	private void init()
	{
		if (!_init)
		{
			_init = true;
			NetworkControl.instance.currentState = "welcome";
			NetworkControl.instance.StartPolling();
		}
		else
		{
			Debug.LogWarning("NOPE");
		}
	}
	

	void OnGUI()
	{
		GUIStyle myLabelStyle = new GUIStyle(GUI.skin.label);
		myLabelStyle.fontSize = 40;
		myLabelStyle.normal.textColor = Color.black;

		GUI.Label(new Rect(175, 650, 575, 200), " Welcome "+NetworkControl.instance.userName+"", myLabelStyle);
	}
}
