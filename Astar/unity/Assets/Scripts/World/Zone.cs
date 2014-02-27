using UnityEngine;
using System.Collections;

public enum ZONE_TYPE
{
	FLOOR=0,
	BLOCK=1
}

public enum ZONE_STATES
{
	NONE=0,
	SELECTED=1,
	HERO=2
}

public class Zone : Touchable
{
	public Material FLOOR;
	public Material BLOCK;
	public Material SELECTED;
	public Material HIGHLIGHTED;
	public Material HERO;

	private bool _collisionActive=false;
	private ZoneCollider _collider;

	private Vector2 _id;
	public Vector2 id
	{
		get { return _id; }
	}

	private bool _initted = false;
	public bool initted
	{
		get { return _initted; }
	}

	public void init(ZONE_TYPE type,Vector2 id)
	{
		if(!_initted)
		{
			_initted = true;
			_id = id;

			switch(type)
			{
				case ZONE_TYPE.FLOOR:
					this.gameObject.renderer.material = FLOOR;
					break;
			}

			if(type == ZONE_TYPE.FLOOR)
			{
				enableTap();
				enableMouseOvr();
			}

			_collider = gameObject.transform.Find("ZoneCollider").gameObject.GetComponent<ZoneCollider>();

			StartCoroutine(_activate());
		}
	}
	
	IEnumerator _activate()
	{
		yield return new WaitForSeconds(0.25f);
		_collider.Enable();
	}

	public void setHeroState()
	{
		disableTap();
		disableMouseOvr();

		this.gameObject.renderer.material = HERO;
	}

	protected override void OnMouseOvr()
	{
		this.gameObject.renderer.material = HIGHLIGHTED;
	}

	protected override void OnMouseOff()
	{
		this.gameObject.renderer.material = FLOOR;
	}

	protected override void OnTap(Vector2 fingerPos)
	{
		Main.instance.Grid.SelectZone(this);
	}

	public void CollisionOn()
	{
		_collisionActive = true;
	}

	public void CollisionOff()
	{
		_collisionActive = false;
	}

	public void Select()
	{
		this.gameObject.renderer.material = SELECTED;
		disableTap();
		disableMouseOvr();
	}

	public void DeSelect()
	{
		this.gameObject.renderer.material = FLOOR;
		enableTap();
		enableMouseOvr();
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(_collisionActive)
		{
			Debug.Log("ZONE TriggerEnter" + other.gameObject.name);
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (_collisionActive)
		{
			Debug.Log("ZONE OnTriggerExit2D" + other.gameObject.name);
		}
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if (_collisionActive)
		{
			Debug.Log("ZONE OnTriggerStay2D()" + other.gameObject.name);
		}
	}
}