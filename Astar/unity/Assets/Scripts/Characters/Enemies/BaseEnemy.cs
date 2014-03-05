using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour 
{
	protected List<Zone> _path;

	protected Animator _animator;

	protected int _health = 10;

	private int _pathIterator = 0;

	public void Init(Zone startingZone)
	{
		gameObject.transform.position = startingZone.position;
		_animator = gameObject.GetComponent<Animator>();

		_path = MainTurret.instance.Grid.PathFinder.FindPath(startingZone,MainTurret.instance.Grid.GetRandomBlankZone());
		navigatePath();
	}

	private void navigatePath()
	{
		_pathIterator++;

		Zone nextZone = _path[_path.Count - 1];
		_path.RemoveAt(_path.Count - 1);

		Vector3 diff = gameObject.transform.position - nextZone.transform.position;

		if(diff != Vector3.zero)
		{
			if(Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
			{
				if(diff.x < 0) _animator.SetInteger("Direction", (int) DIRECTIONS.RIGHT);
				else _animator.SetInteger("Direction", (int) DIRECTIONS.LEFT);
			}
			else
			{
				if(diff.y < 0) _animator.SetInteger("Direction", (int) DIRECTIONS.UP);
				else _animator.SetInteger("Direction", (int) DIRECTIONS.DOWN);
			}
			
			iTween.MoveTo(gameObject,iTween.Hash("x", nextZone.position.x, "y", nextZone.position.y, "easeType", "easeOutExpo", "oncomplete","navigateToZoneComplete"));
		}
		else navigatePath();
	}

	private void navigateToZoneComplete()
	{
		StartCoroutine(waitNextZone());
	}

	private IEnumerator waitNextZone()
	{
		yield return new WaitForSeconds(0.45f);
		if(_path.Count > 0)
		{
			navigatePath();
		}
		else
		{
			Debug.LogWarning("ALL DONE AND SHIT");
		}
	}

	public void TakeHit()
	{
		_health -= 1;
		if(_health <= 0)
		{
			//Debug.LogError("DEAD!");
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.GetComponent<ZoneCollider>() != null)
		{
			ZoneCollider zc = other.gameObject.GetComponent<ZoneCollider>();
		}
	}
}