using UnityEngine;
using System.Collections;

public class Main : MonoSingleton<Main>
{
	public GameObject grid;
	public Grid Grid;

	public Vector2 area;
	public Arena arena;

	public Dude dude;

	private bool _init=false;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		init();
	}
	
	private void init()
	{
		if (!_init)
		{
			_init = true;
			Debug.Log("");
			Debug.Log(" INIT()  :::::::::: ");
			Debug.Log("");

			float h = 2*Camera.main.orthographicSize;
			float w = h*Camera.main.aspect;

			area = new Vector2(w, h);

			GameObject gridPrefab = (GameObject) Instantiate(grid);
			Grid = gridPrefab.GetComponent<Grid>();
			Grid.init();

			GameObject levelOne = GameObject.Find("LevelOne");
			arena = levelOne.GetComponent<Arena>();
			arena.init();

			GameObject dudeObj = GameObject.Find("dude");
			dude = dudeObj.GetComponent<Dude>();

			//Link ui buttons
			GameObject.Find("StepButton").GetComponent<BaseButton>().Subscribe(this, Grid.PathFinder.Step);
			GameObject.Find("ClearButton").GetComponent<BaseButton>().Subscribe(this, Grid.PathFinder.Clear);
			GameObject.Find("IncrementButton").GetComponent<BaseButton>().Subscribe(this, Grid.PathFinder.ToggleMode);
		}
		else
		{
			Debug.LogWarning("NOPE");
		}
	}
}
