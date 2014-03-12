using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void delegateTimerTick(int secondsLeft);

public class CountDownTimer : MonoSingleton<NetworkControl>
{
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	private int _duration;
	private delegateTimerTick _tick;
	private bool _running = false;

	public void reset()
	{
		CancelInvoke("Countdown");
		_duration = 0;
	}	
	public void start(int duration, delegateTimerTick tick)
	{
		_duration = duration;
		_tick = tick;

		InvokeRepeating("tickTock",1.0f,1.0f);
	}

	public void stop()
	{
		_running = false;
	}

	public void tickTock()
	{
		if(_running)
		{
			Debug.Log("tickTock() " + _duration);
			if(--_duration == 0)
			{
				CancelInvoke("Countdown");
				_tick(0);
			}

			_tick(_duration);
		}
	}
}