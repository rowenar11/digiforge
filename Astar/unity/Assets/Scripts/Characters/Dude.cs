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

	// Use this for initialization
	void Start()
	{
		animator = this.GetComponent<Animator>();
		animator.SetInteger("Direction", (int)DIRECTIONS.DOWN);
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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "ZoneCollider")
		{
			Zone zone = other.gameObject.GetComponent<ZoneCollider>().zone;
			if(zone != null)
			{
				zone.setHeroState();
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		//Debug.Log("D5");
	}

	void OnTriggerStay2D(Collider2D other)
	{
//		if (other.gameObject.name == "ZoneCollider")
//		{
//			Zone zone = other.gameObject.GetComponent<ZoneCollider>().zone;
//			if (zone != null)
//			{
//				//Debug.Log("zone:" + zone);
//			}
//		}
	}

}