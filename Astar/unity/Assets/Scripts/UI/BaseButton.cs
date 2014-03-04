using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void delegateButtonPush();

public class BaseButton : Touchable
{
	public Sprite up;
	public Sprite ovr;

	private Dictionary<Object, delegateButtonPush> _subscribers;

	protected SpriteRenderer _renderer;

	void Start () 
	{
		_renderer = gameObject.GetComponent<SpriteRenderer>();

		enableMouseOvr();
		enableTap();
	}
	
	public void Subscribe(Object obj, delegateButtonPush pushAction)
	{
		if (_subscribers == null) _subscribers = new Dictionary<Object, delegateButtonPush>();

		if(!_subscribers.ContainsKey(obj))
		{
			_subscribers.Add(obj,pushAction);
		}
	}

	public void UnSubscribe(Object obj)
	{
		if (_subscribers == null) _subscribers = new Dictionary<Object, delegateButtonPush>();

		if(_subscribers.ContainsKey(obj))
		{
			_subscribers.Remove(obj);
		}
	}

	protected override void OnMouseOvr()
	{
		_renderer.sprite = ovr;
	}

	protected override void OnMouseOff()
	{
		_renderer.sprite = up;
	}

	protected override void OnTap(Vector2 fingerPos)
	{
		if (_subscribers != null)
		{
			foreach(KeyValuePair<Object, delegateButtonPush> kvp in _subscribers)
			{
				kvp.Value();
			}
		}
	}
}