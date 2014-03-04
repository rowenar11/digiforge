using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyTypes
{
	BLUE=1,
	GOLD_KNIGHT=2,
	PIG=3
}

public class EnemySpawner : MonoBehaviour
{
	public GameObject BlueEnemyObj;
	public GameObject GoldKnightObj;
	public GameObject PigObj;

	private Dictionary<EnemyTypes,List<BaseEnemy>> _enemyPool;

	public void init()
	{
		_enemyPool = new Dictionary<EnemyTypes,List<BaseEnemy>>();
	}

	public BlueEnemy SpawnBlue()
	{
		BlueEnemy enemy;
		if(!_enemyPool.ContainsKey(EnemyTypes.BLUE)) _enemyPool[EnemyTypes.BLUE] = new List<BaseEnemy>();
		if(_enemyPool[EnemyTypes.BLUE].Count > 0)
		{
			Debug.Log("in pool");
			enemy = _enemyPool[EnemyTypes.BLUE][_enemyPool[EnemyTypes.BLUE].Count - 1] as BlueEnemy;
			_enemyPool[EnemyTypes.BLUE].RemoveAt(_enemyPool[EnemyTypes.BLUE].Count - 1);
		}
		else
		{
			Debug.Log("not in pool pool");
			GameObject bEnemy = (GameObject)Instantiate(BlueEnemyObj);
			enemy = bEnemy.GetComponent<BlueEnemy>();
		}

		Debug.Log("return that bitch and stuff");
		return enemy;
	}
}
