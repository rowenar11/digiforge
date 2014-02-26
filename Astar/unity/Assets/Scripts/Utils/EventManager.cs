using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void EventManagerCallback(EventObject eventObj);

public class EventManager
{
	private static Dictionary<string, List<EventManagerCallback>> _events;

	void Awake()
	{
		_events = new Dictionary<string, List<EventManagerCallback>>();
	}

	public static void AddListener(string eventName, EventManagerCallback callback)
	{
		if(!_events.ContainsKey(eventName)) _events.Add(eventName, new List<EventManagerCallback>());

		Debug.Log("** ADDING EVENT  " + eventName);
		_events[eventName].Add(callback);
	}

	public static void RemoveListener(string eventName, EventManagerCallback callback)
	{
		if(_events.ContainsKey(eventName))
		{
			Debug.Log("** REMOVING EVENT  " + eventName);
			_events[eventName].Remove(callback);
		}
	}

	public static void BroadcastEvent(string eventName, EventObject eventObj)
	{
		if(_events.ContainsKey(eventName))
		{
			List<EventManagerCallback> callbackList = _events[eventName];
			foreach(EventManagerCallback callback in callbackList)
			{
				if(callback != null)
				{
					Debug.Log("** BROADCAST EVENT  " + eventName);
					callback(eventObj);
				}
			}
		}
	}
}

public class EventObject
{
	public string type;
	public object data;
}