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
	public Texture FLOOR;
	public Texture BLOCK;
	public Texture SELECTED;
	public Texture HIGHLIGHTED;
	public Texture NEIGHBOR;
	public Texture NEAREST;
	public Texture HERO;
	public Texture PATH;

	private bool _collisionActive=false;
	private ZoneCollider _collider;

	public Zone parentZone;

	public float gScore;
	public float hScore;
	public float fScore;

	private ZONE_STATES _state = ZONE_STATES.NONE;
	public ZONE_STATES ZoneState
	{
		get { return _state; }
	}

	
	private ZONE_TYPE _type;
	public ZONE_TYPE ZoneType
	{
		get { return _type; }
	}

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
			_type = type;

			switch(type)
			{
				case ZONE_TYPE.FLOOR:
					this.gameObject.renderer.material.mainTexture = FLOOR;
					break;

				case ZONE_TYPE.BLOCK:
					this.gameObject.renderer.material.mainTexture = BLOCK;
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

	public void setPathState()
	{
		disableTap();
		disableMouseOvr();

		this.gameObject.renderer.material.mainTexture = PATH;
	}

	public void setNeighborState()
	{
		disableTap();
		disableMouseOvr();

		if (_state != ZONE_STATES.SELECTED)
		{
			this.gameObject.renderer.material.mainTexture = NEIGHBOR;
		}
	}

	public void setNearestState()
	{
		disableTap();
		disableMouseOvr();

		if (_state != ZONE_STATES.SELECTED)
		{
			this.gameObject.renderer.material.mainTexture = NEAREST;
		}
	}

	public void setHeroState()
	{
		disableTap();
		disableMouseOvr();

		_state = ZONE_STATES.HERO;
		this.gameObject.renderer.material.mainTexture = HERO;
	}

	protected override void OnMouseOvr()
	{
		this.gameObject.renderer.material.mainTexture = HIGHLIGHTED;
	}

	protected override void OnMouseOff()
	{
		this.gameObject.renderer.material.mainTexture = FLOOR;
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
		this.gameObject.renderer.material.mainTexture = SELECTED;

		_state = ZONE_STATES.SELECTED;

		disableTap();
		disableMouseOvr();
	}

	public void DeSelect()
	{
		this.gameObject.renderer.material.mainTexture = FLOOR;

		_state = ZONE_STATES.NONE;

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

	public void Dispose()
	{
		Destroy(gameObject);
	}
}