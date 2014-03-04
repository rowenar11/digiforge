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


public class BulletHoles : MonoBehaviour
{
	public Sprite bTop1;
	public Sprite bTop2;
	public Sprite bTop3;
	public Sprite bTop4;
	public Sprite bTop5;
	public Sprite bTop6;
	public Sprite bTop7;
	public Sprite bTop8;
	public Sprite bTop9;
	public Sprite bTop10;
	public Sprite bTop11;
	public Sprite bTop12;

	public Sprite bBottom1;
	public Sprite bBottom2;
	public Sprite bBottom3;
	public Sprite bBottom4;
	public Sprite bBottom5;
	public Sprite bBottom6;
	public Sprite bBottom7;
	public Sprite bBottom8;
	public Sprite bBottom9;
	public Sprite bBottom10;
	public Sprite bBottom11;
	public Sprite bBottom12;

	public Sprite bRight1;
	public Sprite bRight2;
	public Sprite bRight3;
	public Sprite bRight4;
	public Sprite bRight5;
	public Sprite bRight6;
	public Sprite bRight7;
	public Sprite bRight8;
	public Sprite bRight9;
	public Sprite bRight10;
	public Sprite bRight11;
	public Sprite bRight12;

	public Sprite bLeft1;
	public Sprite bLeft2;
	public Sprite bLeft3;
	public Sprite bLeft4;
	public Sprite bLeft5;
	public Sprite bLeft6;
	public Sprite bLeft7;
	public Sprite bLeft8;
	public Sprite bLeft9;
	public Sprite bLeft10;
	public Sprite bLeft11;
	public Sprite bLeft12;

	private Dictionary<WALL,List<Sprite>> _bullotHoles;

	private bool _init = false;

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

	public void Init(GameObject collider, Vector3 startPos)
	{
		if(!_init)
		{
			WALL wallHit=WALL.TOP;
			switch (collider.name)
			{
				case "RightWall": wallHit = WALL.RIGHT;
					break;
				case "LeftWall": wallHit = WALL.LEFT;
					break;
				case "BottomWall": wallHit = WALL.BOTTOM;
					break;
				case "TopWall": wallHit = WALL.TOP;
					break;
			}

			gameObject.transform.position = new Vector3(startPos.x + xOffset(wallHit), startPos.y + yOffset(wallHit), startPos.z);

			_init = true;

			_bullotHoles = new Dictionary<WALL,List<Sprite>>();
			_bullotHoles.Add(WALL.TOP, new List<Sprite>(){bTop1,bTop2,bTop3,bTop4,bTop5,bTop6,bTop7,bTop8,bTop9,bTop10,bTop11,bTop12});
			_bullotHoles.Add(WALL.BOTTOM, new List<Sprite>() { bBottom1, bBottom2, bBottom3, bBottom4, bBottom5, bBottom6, bBottom7, bBottom8, bBottom9, bBottom10, bBottom11, bBottom12 });
			_bullotHoles.Add(WALL.RIGHT, new List<Sprite>() { bRight1, bRight2, bRight3, bRight4, bRight5, bRight6, bRight7, bRight8, bRight9, bRight10, bRight11, bRight12 });
			_bullotHoles.Add(WALL.LEFT, new List<Sprite>() { bLeft1, bLeft2, bLeft3, bLeft4, bLeft5, bLeft6, bLeft7, bLeft8, bLeft9, bLeft10, bLeft11, bLeft12 });

			int randomBulletHole = Random.Range(1, 12);
			this.GetComponent<SpriteRenderer>().sprite = _bullotHoles[wallHit][randomBulletHole];

			StartCoroutine(erase());
		}
	}

	private IEnumerator erase()
	{
		yield return new WaitForSeconds(Random.Range(1.35f, 6.0f));
		Destroy(gameObject);
	}
}
