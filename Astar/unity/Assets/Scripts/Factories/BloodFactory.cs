using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodFactory : MonoBehaviour
{
	public GameObject bloodObj;

	private Transform Blood;
	private List<BloodSpurt> _bloodPool; 

	public void init()
	{
		_bloodPool = new List<BloodSpurt>();

		GameObject bh = new GameObject("BloodSpurts");
		Blood = bh.transform;
	}

	public BloodSpurt GetBlood(Vector3 hitPoint,bool isDeath=false)
	{
		BloodSpurt blood;
		if (_bloodPool.Count > 0)
		{
			blood = _bloodPool[_bloodPool.Count - 1];
			_bloodPool.RemoveAt(_bloodPool.Count - 1);
		}
		else
		{
			GameObject bPrefab = (GameObject)Instantiate(bloodObj);
			blood = bPrefab.GetComponent<BloodSpurt>();
			blood.transform.parent = Blood;
		}

		blood.Init(hitPoint);

		return blood;
	}

	public void ReturnToPool(BloodSpurt bSpurt)
	{
		_bloodPool.Add(bSpurt);
	}
}
