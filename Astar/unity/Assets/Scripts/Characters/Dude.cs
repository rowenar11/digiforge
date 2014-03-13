using UnityEngine;
using System.Collections;

public enum DIRECTIONS
{
	UP = 1,
	DOWN = 2,
	RIGHT = 3,
	LEFT = 4
}

public class Dude : MonoBehaviour
{
	private Animator animator;

	private Zone myZone;

	private bool initted = false;
	private bool detectStay = true;

	// Use this for initialization
	void Start()
	{
		animator = this.GetComponent<Animator>();
		animator.SetInteger("Direction", (int)DIRECTIONS.DOWN);

		StartCoroutine(waitInit());
	}

	private IEnumerator waitInit()
	{
		yield return new WaitForSeconds(1.0f);
		initted = true;
	}

	// Update is called once per frame
	void Update()
	{
		var vertical = Input.GetAxis("Vertical");
		var horizontal = Input.GetAxis("Horizontal");

		if (vertical > 0)
		{
			animator.SetInteger("Direction", (int)DIRECTIONS.UP);
		}
		else if (vertical < 0)
		{
			animator.SetInteger("Direction", (int)DIRECTIONS.DOWN);
		}
		else if (horizontal > 0)
		{
			animator.SetInteger("Direction", (int)DIRECTIONS.RIGHT);
		}
		else if (horizontal < 0)
		{
			animator.SetInteger("Direction", (int)DIRECTIONS.LEFT);
		}
	}

	public void GotTo(Zone dest)
	{
		Debug.Log("my zone:" + myZone);
		Main.instance.Grid.PathFinder.FindPath(myZone,dest);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(initted)
		{
			Debug.Log("HERO HERO HERO OnTriggerEnter2D " + other.gameObject.name);
			if(other.gameObject.name == "ZoneCollider")
			{
				ZoneCollider zc = other.gameObject.GetComponent<ZoneCollider>();
				Debug.Log("zc:" + zc.meStarted);
				Zone zone = zc.zone;
				Debug.Log("ZONE:" + zone);
				if(zone != null)
				{
					zone.setHeroState();
					myZone = zone;
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		//Debug.Log("D5");
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(initted && detectStay)
		{
			Debug.Log("HERO HERO HERO OnTriggerEnter2D " + other.gameObject.name);
			if (other.gameObject.name == "ZoneCollider")
			{
				ZoneCollider zc = other.gameObject.GetComponent<ZoneCollider>();
				Debug.Log("zc:" + zc.meStarted);
				Zone zone = zc.zone;
				Debug.Log("ZONE:" + zone);
				if (zone != null)
				{
					detectStay = false;
					zone.setHeroState();
					myZone = zone;
				}
			}
		}
	}

}