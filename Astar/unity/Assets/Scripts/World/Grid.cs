using UnityEngine;
using System;
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

	private GameObject topLeft;
	private GameObject topRight;
	private GameObject bottomLeft;
	private GameObject bottomRight;

	private float _width;
	private float _height;

	public void init()
	{
		topLeft = GameObject.Find("Marker_TopLeft");
		topRight = GameObject.Find("Marker_TopRight");
		bottomLeft = GameObject.Find("Marker_BottomLeft");
		bottomRight = GameObject.Find("Marker_BottomRight");

		_width = topRight.transform.position.x - topLeft.transform.position.x;
		_height = topRight.transform.position.y - bottomLeft.transform.position.y;

		Debug.Log("topLeft:"+topLeft.transform.position);
		Debug.Log("topRight:"+topRight.transform.position);
		Debug.Log("bottomLeft:"+bottomLeft.transform.position);
		Debug.Log("bottomRight:"+bottomRight.transform.position);
		Debug.LogWarning("WIDTH:"+_width+" x HEIGHT:"+_height);

		draw();
	}

	private void draw()
	{
		float colWidth = _width / numCols;
		float colHeight = _height / numRows;

		

		GameObject lastZone = gameObject;
		for (int y = 0; y < numRows; y++)
		{
			for (int x = 0; x < numCols; x++)
			{
				_zone = (GameObject)Instantiate(zone);
				Zone z = _zone.GetComponent<Zone>();
				z.init(ZONE_TYPE.zone1);
				//z.gameObject.renderer.sortingLayerName = "Grid";

				Vector3 pos;
				if(x == 0 && y == 0)
				{
					pos = new Vector3(-_width/2,_height/2,0);
					pos.x += colWidth / 2;
					pos.y -= colHeight / 2;
					Debug.LogError(" POS : "+pos);
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
				lastZone = _zone;
			}
		}
	}
}