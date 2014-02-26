using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
	public GameObject zone;

	private GameObject _zone;

	private int numCols = 16;
	private int numRows = 16;


	public Dictionary<int, GameObject> zones;
	public Dictionary<int, GameObject> rowZones;

	public void init()
	{
		Debug.Log("Grid.init()");
		draw();
	}

	private void draw()
	{
		Debug.Log("DRAW THAT SHIT BITCH");

		float colWidth = Main.instance.area.x / numCols;
		float colHeight = Main.instance.area.y / numRows;
		Debug.Log("COL " + colWidth + "x" + colHeight);

		GameObject lastZone = gameObject;
		for (int y = 0; y < numCols; y++)
		{
			for (int x = 0; x < numCols; x++)
			{
				_zone = (GameObject)Instantiate(zone);
				Vector3 pos;

				if (x == 0 && y == 0)
				{
					Debug.Log("DO THAT SHIT : " + _zone.renderer.bounds.size.x + ":" + _zone.renderer.bounds.size.y);
					
					

					pos = new Vector3(-Main.instance.area.x / 2, Main.instance.area.y / 2, 0);
					pos.x -= colWidth / 2;
					pos.y -= colHeight / 2;
					Debug.Log(" :: " + pos.x + "|" + pos.y + "  " + colWidth + "x" + colHeight);
				}
				else
				{
					pos = lastZone.transform.position;
					pos.x += colWidth;
				}

				_zone.transform.position = pos;
				lastZone = _zone;
			}


		}

	}


}
