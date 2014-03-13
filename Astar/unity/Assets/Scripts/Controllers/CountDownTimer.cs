using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void delegateTimerTick(int secondsLeft);

public class CountDownTimer : MonoSingleton<CountDownTimer>
{
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	private int _duration;
	private delegateTimerTick _tick;
	private bool _running = false;

	public void resetTimer()
	{
		CancelInvoke("tickTock");
		_duration = 0;
	}	
	public void startTimer(int duration, delegateTimerTick tick)
	{
		_running = true;

		_duration = duration;
		_tick = tick;

		InvokeRepeating("tickTock",1.0f,1.0f);
	}

	public void stopTimer()
	{
		_running = false;
	}

	public void tickTock()
	{
		if(_running)
		{
			if(--_duration == 0)
			{
				_tick(0);
			}

			_tick(_duration);
		}
	}
}