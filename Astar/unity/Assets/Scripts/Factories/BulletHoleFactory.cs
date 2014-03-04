using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletHoleFactory : MonoBehaviour 
{
	public List<Sprite> TopBullets;
	public List<Sprite> BottomBullets;
	public List<Sprite> LeftBullets;
	public List<Sprite> RightBullets;
	
	private Dictionary<WALL,List<Sprite>> _spriteSheet;

	private List<BulletHole> _bulletPool;

	public GameObject bulletHoleObj;

	private Transform BulletHoles;

	public void init()
	{
		_spriteSheet = new Dictionary<WALL,List<Sprite>>();
		_spriteSheet[WALL.TOP] = TopBullets;
		_spriteSheet[WALL.BOTTOM] = BottomBullets;
		_spriteSheet[WALL.LEFT] = LeftBullets;
		_spriteSheet[WALL.RIGHT] = RightBullets;

		_bulletPool = new List<BulletHole>();

		GameObject bh = new GameObject("BulletHoles");
		BulletHoles = bh.transform;
	}

	public BulletHole GetBullet(GameObject collider,Vector3 hitPoint)
	{
		WALL wallHit = WALL.TOP;
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

		int randomBullet = Random.Range(1,12);

		BulletHole bulletHole;
		if(_bulletPool.Count > 0)
		{
			bulletHole = _bulletPool[_bulletPool.Count - 1];
			_bulletPool.RemoveAt(_bulletPool.Count - 1);
		}
		else
		{
			GameObject bulletPrefab = (GameObject)Instantiate(bulletHoleObj);
			bulletHole = bulletPrefab.GetComponent<BulletHole>();
			bulletHole.transform.parent = BulletHoles;
		}

		bulletHole.Init(_spriteSheet[wallHit][Random.Range(0,11)],hitPoint,wallHit);
		return bulletHole;
	}

	public void ReturnToPool(BulletHole bulletHole)
	{
		_bulletPool.Add(bulletHole);
	}
}
