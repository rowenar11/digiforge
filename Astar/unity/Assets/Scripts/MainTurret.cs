using UnityEngine;
using System.Collections;

public class MainTurret : MonoSingleton<MainTurret>
{
	public GameObject TurretObj;
	public Turret TurretDude;

	public GameObject grid;
	public Grid Grid;

	public Vector2 area;
	public Arena arena;

	private bool _init = false;
	private bool _gameRunning = false;

	public GameObject enemySpawnerObj;
	public EnemySpawner enemySpawner;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		init();
	}

	private bool _createBlueEnemy = true;

	private void init()
	{
		if(!_init)
		{
			_init = true;
			GameObject dudeObj = (GameObject) Instantiate(TurretObj);
			TurretDude = dudeObj.GetComponent<Turret>();
			TurretDude.init();

			float h = 2 * Camera.main.orthographicSize;
			float w = h * Camera.main.aspect;

			area = new Vector2(w, h);

			GameObject gridPrefab = (GameObject)Instantiate(grid);
			Grid = gridPrefab.GetComponent<Grid>();
			Grid.init(25,2,true);

			GameObject eObj = (GameObject)Instantiate(enemySpawnerObj);
			enemySpawner = eObj.GetComponent<EnemySpawner>();
			enemySpawner.init();
		}

		StartCoroutine(StartGame());
	}

	private IEnumerator StartGame()
	{
		yield return new WaitForSeconds(0.4f);
		_gameRunning = true;
	}

	void Update()
	{
		if(_gameRunning)
		{
			if(_createBlueEnemy)
			{
				_createBlueEnemy = false;
				BlueEnemy enemy = enemySpawner.SpawnBlue();
				enemy.Init(Grid.GetRandomBlankZone());

			}
		}
	}
}
