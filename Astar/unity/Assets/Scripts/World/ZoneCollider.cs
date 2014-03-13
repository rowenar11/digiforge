using UnityEngine;
using System.Collections;

public class ZoneCollider : MonoBehaviour
{
	private Zone _zone;
	public bool meStarted=false;

	public Zone zone
	{
		get { return _zone; }
	}

	public void Enable()
	{
		collider2D.enabled = true;
	}

	public void Disable()
	{
		collider2D.enabled = false;
	}

	void Start()
	{
		meStarted = true;
		_zone = gameObject.transform.parent.gameObject.GetComponent<Zone>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (_zone != null && _zone.initted && other != null) _zone.OnTriggerEnter2D(other);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (_zone != null && _zone.initted && other != null) _zone.OnTriggerExit2D(other);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (_zone != null  && other != null) _zone.OnTriggerExit2D(other);
	}
}
