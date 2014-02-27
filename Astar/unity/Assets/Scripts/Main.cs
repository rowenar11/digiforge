using UnityEngine;
using System.Collections;

public class Main : MonoSingleton<Main>
{
	public GameObject grid;
	private Grid _grid;

	public Vector2 area;
	public GameObject levelOne;

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
		_grid = gridPrefab.GetComponent<Grid>();
		_grid.init();

		levelOne = GameObject.Find("LevelOne");
	}
}
