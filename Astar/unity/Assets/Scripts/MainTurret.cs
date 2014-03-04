using UnityEngine;
using System.Collections;

public class MainTurret : MonoBehaviour
{
	public GameObject TurretObj;
	private Turret TurretDude;

	public GameObject grid;
	public Grid Grid;

	public Vector2 area;
	public Arena arena;

	private bool _init = false;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		init();
	}

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
			Grid.init(false);
		}
		else
		{
			Debug.LogWarning("Nope");
		}
	}
}
