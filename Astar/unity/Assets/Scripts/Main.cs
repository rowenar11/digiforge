using UnityEngine;
using System.Collections;

public class Main : MonoSingleton<Main>
{
	public GameObject grid;
	public Grid Grid;

	public Vector2 area;
	public Arena arena;

	public Dude dude;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		init();
	}
	
	private void init()
	{
		float h = 2 * Camera.main.orthographicSize;
		float w = h * Camera.main.aspect;

		area = new Vector2(w,h);

		Debug.Log("INIT MAIN : "+Screen.width+" x "+Screen.height);
		Debug.Log("camera : " + area.x + " x " + area.y);

		GameObject gridPrefab = (GameObject)Instantiate(grid);
		Grid = gridPrefab.GetComponent<Grid>();
		Grid.init();

		GameObject levelOne = GameObject.Find("LevelOne");
		arena = levelOne.GetComponent<Arena>();
		arena.init();

		GameObject dudeObj = GameObject.Find("dude");
		dude = dudeObj.GetComponent<Dude>();
	}
}
