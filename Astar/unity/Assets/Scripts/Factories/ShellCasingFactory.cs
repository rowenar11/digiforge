using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShellCasingFactory : MonoBehaviour
{
	public List<Sprite> spriteSheet;

	private List<ShellCasing> _shellCasingPool;

	public GameObject shellCasingObj;

	private Transform ShellCasings;

	public void init()
	{
		_shellCasingPool = new List<ShellCasing>();

		GameObject bh = new GameObject("ShellCasings");
		ShellCasings = bh.transform;
	}

	public ShellCasing GetShellCasing(Vector3 hitPoint)
	{
		int randomBullet = Random.Range(1,7);

		ShellCasing shellCasing;
		if(_shellCasingPool.Count > 0)
		{
			shellCasing = _shellCasingPool[_shellCasingPool.Count - 1];
			_shellCasingPool.RemoveAt(_shellCasingPool.Count - 1);
		}
		else
		{
			GameObject prefab = (GameObject)Instantiate(shellCasingObj);
			shellCasing = prefab.GetComponent<ShellCasing>();
			shellCasing.transform.parent = ShellCasings;
		}

		shellCasing.Init(hitPoint,spriteSheet[Random.Range(0,spriteSheet.Count-1)]);
		return shellCasing;
	}

	public void ReturnToPool(ShellCasing shellCasing)
	{
		_shellCasingPool.Add(shellCasing);
	}
}
