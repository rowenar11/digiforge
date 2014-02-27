using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
	public GameObject zone;
	private GameObject _zone;

	private int _numCols = 16;
	private int _numRows = 16;

	private GameObject _topLeft;
	private GameObject _topRight;
	private GameObject _bottomLeft;
	private GameObject _bottomRight;

	private float _width;
	private float _height;

	public List<Zone> selectedZones;

	public Dictionary<int, Dictionary<int, Zone>> TheGrid; 

	public void init()
	{
		_topLeft = GameObject.Find("Marker_TopLeft");
		_topRight = GameObject.Find("Marker_TopRight");
		_bottomLeft = GameObject.Find("Marker_BottomLeft");
		_bottomRight = GameObject.Find("Marker_BottomRight");

		_width = _topRight.transform.position.x - _topLeft.transform.position.x;
		_height = _topRight.transform.position.y - _bottomLeft.transform.position.y;

		draw();
	}

	private void draw()
	{
		TheGrid = new Dictionary<int, Dictionary<int, Zone>>();

		float colWidth = _width / _numCols;
		float colHeight = _height / _numRows;

		GameObject lastZone = gameObject;
		for (int y = 0; y < _numRows; y++)
		{
			TheGrid[y] = new Dictionary<int, Zone>();

			for (int x = 0; x < _numCols; x++)
			{
				_zone = (GameObject)Instantiate(zone);
				Zone z = _zone.GetComponent<Zone>();
				z.gameObject.renderer.sortingLayerName = "Grid";

				TheGrid[y][x] = z;

				Vector3 pos;
				if(x == 0 && y == 0)
				{
					pos = new Vector3(-_width/2,_height/2,0);
					pos.x += colWidth / 2;
					pos.y -= colHeight / 2;
				}
				else
				{
					pos = lastZone.transform.position;
					if(x == 0)
					{
						pos.x = (-_width/2) + (colWidth / 2);
						pos.y -= 1 * colHeight;
					}
					else pos.x += colWidth;
				}

				_zone.transform.localScale = new Vector3(colWidth,colHeight,transform.localScale.z);

				_zone.transform.position = pos;

				z.init(ZONE_TYPE.FLOOR, new Vector2(x, y));

				lastZone = _zone;
			}
		}
	}

	public void SelectZone(Zone zone)
	{
		UnselectAllZones();
		selectedZones.Add(zone);
		zone.Select();
	}

	public void UnselectAllZones()
	{
		foreach (Zone zone in selectedZones)
		{
			zone.DeSelect();
		}
	}
}