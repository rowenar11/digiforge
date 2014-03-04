using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WALL
{
	TOP=1,
	BOTTOM=2,
	RIGHT=3,
	LEFT=4
}

public class BulletHole : MonoBehaviour
{
	private bool _init = false;

	public void Init(Sprite bulletSprite, Vector3 startPos, WALL wallHit)
	{
		if(!_init)
		{
			_init = true;
			gameObject.SetActive(true);

			gameObject.transform.position = new Vector3(startPos.x + xOffset(wallHit),startPos.y + yOffset(wallHit),startPos.z);

			int randomBulletHole = Random.Range(1,12);
			this.GetComponent<SpriteRenderer>().sprite = bulletSprite;

			StartCoroutine(erase());
		}
	}

	private float xOffset(WALL wallHit)
	{
		float xOff;
		if(wallHit == WALL.LEFT || wallHit == WALL.RIGHT)
		{
			xOff = Random.Range(1, 10)*0.1f;
			if (wallHit == WALL.LEFT) xOff *= -1;
		}
		else
		{
			xOff = Random.Range(1, 3) * 0.1f;
			if (Random.Range(0, 1) == 1) xOff *= -1;
		}

		return xOff;
	}

	private float yOffset(WALL wallHit)
	{
		float yOff;
		if (wallHit == WALL.TOP || wallHit == WALL.BOTTOM)
		{
			yOff = Random.Range(1, 10) * 0.1f;
			if (wallHit == WALL.BOTTOM) yOff *= -1;
		}
		else
		{
			yOff = Random.Range(1, 3) * 0.1f;
			if (Random.Range(0, 1) == 1) yOff *= -1;
		}

		return yOff;
	}
	
	private IEnumerator erase()
	{
		yield return new WaitForSeconds(Random.Range(1.35f, 6.0f));
		Dispose();
	}

	public void Dispose()
	{
		gameObject.SetActive(false);
		_init = false;

		MainTurret.instance.TurretDude.BulletHoleFactory.ReturnToPool(this);
	}

}
