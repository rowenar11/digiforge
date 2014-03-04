using UnityEngine;
using System;
using System.Collections.Generic;

public class StopWatch
{
	private string _key;

	public string key
	{
		get { return _key; }
	}

	private bool _isRunning;

	public bool isRunning
	{
		get { return _isRunning; }
	}

	private double totalDuration;
	private double lastStart;

	public StopWatch(string keyIn)
	{
		_key = keyIn;
		_isRunning = false;
	}

	public void start()
	{
		if(!isRunning)
		{
			_isRunning = true;
			lastStart = EpochUtils.getNowUTCSeconds();
		}
	}

	public void stop()
	{
		if(isRunning)
		{
			_isRunning = false;
			totalDuration += EpochUtils.getNowUTCSeconds() - lastStart;
		}
	}

	public void reset()
	{
		_isRunning = false;
		lastStart = 0;
		totalDuration = 0;
	}

	public double getDuration()
	{
		return totalDuration;
	}
}

public class StopWatchUtil : MonoSingleton<StopWatchUtil>
{
	private static Dictionary<string, StopWatch> stopWatchKeys = new Dictionary<string, StopWatch>();

	public static void startTimer(string key)
	{
		if(!stopWatchKeys.ContainsKey(key) || stopWatchKeys[key] == null) stopWatchKeys[key] = new StopWatch(key);

		stopWatchKeys[key].start();
	}

	public static void stopTimer(string key)
	{
		if(stopWatchKeys.ContainsKey(key) && stopWatchKeys[key] != null)
		{
			stopWatchKeys[key].stop();
		}
		else throw new System.InvalidOperationException("StopWatchUtil: you issued a stopTimer with a key that does not exist:" + key);
	}

	public static void resetTimer(string key)
	{
		if(stopWatchKeys.ContainsKey(key) && stopWatchKeys[key] != null)
		{
			stopWatchKeys[key].reset();
		}
		else throw new System.InvalidOperationException("StopWatchUtil: you issued a resetTimer with a key that does not exist:" + key);
	}

	public static double getDuration(string key)
	{
		if(stopWatchKeys.ContainsKey(key) && stopWatchKeys[key] != null)
		{
			return stopWatchKeys[key].getDuration();
		}
		else throw new System.InvalidOperationException("StopWatchUtil: you issued a getDuration with a key that does not exist:" + key);
	}
}