using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PATH_DIRECTION
{
	NORTH=1,
	SOUTH=2,
	EAST=3,
	WEST=4,
	NORTH_EAST=13,
	NORTH_WEST=14,
	SOUTH_EAST=23,
	SOUTH_WEST=24
}

public class PathFinding : MonoBehaviour 
{

	private List<Zone> closed;
	private List<Zone> opened;

	private List<Zone> path;

	private Grid _grid;

	private Zone _source;
	private Zone _dest;

	private Zone _nextNearest;

	private int _iterations = 0;

	private bool _verbose = false;

	private bool incrementMode = false;

	public void Init(Grid grid)
	{
		_grid = grid;
	}

	private void highlightPath(Zone finalZone)
	{
		finalZone.setPathState();
		path.Add(finalZone);
		Zone nextParent = finalZone.parentZone;

		bool done = false;
		while(!done)
		{
			path.Add(nextParent);
			nextParent.setPathState();

			if (nextParent.parentZone != null)
			{
				nextParent = nextParent.parentZone;
			}
			else done = true;
		}

		_source = null;
		_dest = null;
	}

	public List<Zone> FindPath(Zone source,Zone dest)
	{
		_iterations = 0;

		_nextNearest = null;

		opened = new List<Zone>();
		closed = new List<Zone>();
		path = new List<Zone>();

		Debug.LogWarning("");
		Debug.LogWarning("FindPath() "+source.id+" : "+dest.id);

		_source = source;
		_dest = dest;

		opened.Add(_source);

		iteratePath();

		return path;
	}


	public void Step()
	{
		if(_dest != null) iteratePath();
	}

	public void Clear()
	{
		Debug.Log("");
		Debug.Log("CLEAR() ");

		Main.instance.Grid.ReDraw();
	}

	public void ToggleMode()
	{
		incrementMode = GameObject.Find("IncrementButton").GetComponent<BaseCheckBox>().isChecked;
	}

	private void iteratePath()
	{
//		Debug.Log("");
//		Debug.Log("");
//		Debug.Log("iteratePath () " + _iterations);
		float lowestScore = -1f;
//		Debug.Log("SEARCH OPEN ZONES !");
		foreach(Zone openZone in opened)
		{
//			Debug.Log(" [" + openZone.id + "] " + openZone.fScore);
			if (lowestScore == -1 || openZone.fScore < lowestScore)
			{
//				Debug.LogWarning("is next lowest!~!~!");
				lowestScore = openZone.fScore;
				_nextNearest = openZone;
			}
		}

//		Debug.Log(" FOUND AS THE NEXT NEAREST : ["+_nextNearest.id+"] ");
		if(_nextNearest == _dest)
		{
			highlightPath(_nextNearest);
		}
		else
		{
			if(opened.Contains(_nextNearest)) opened.Remove(_nextNearest);
			closed.Add(_nextNearest);

			_nextNearest.setNearestState();

			Dictionary<PATH_DIRECTION, Zone> neighbors = getNieghbors(_nextNearest);
//			Debug.Log(" LOOP Neighbors : " + neighbors);
			foreach(KeyValuePair<PATH_DIRECTION, Zone> kvp in neighbors)
			{
				if (kvp.Value != null && kvp.Value.ZoneType != ZONE_TYPE.BLOCK && kvp.Value.ZoneState != ZONE_STATES.HERO && (closed.Count <= 0 || !closed.Contains(kvp.Value)))
				{
					if(!opened.Contains(kvp.Value))
					{
						opened.Add(kvp.Value);
						kvp.Value.setNeighborState();

						kvp.Value.parentZone = _nextNearest;

						kvp.Value.gScore = kvp.Value.parentZone.gScore + calculateGScore(kvp.Key);
						kvp.Value.hScore = calculateHScore(kvp.Value);
						kvp.Value.fScore = calculateFScore(kvp.Value);
					}
					else
					{
						if(_nextNearest.gScore + calculateGScore(kvp.Key) < kvp.Value.gScore)
						{
							kvp.Value.parentZone = _nextNearest;
							kvp.Value.gScore = _nextNearest.gScore + calculateGScore(kvp.Key);
							kvp.Value.fScore = calculateFScore(kvp.Value);
						}
					}
				}
			}

			if(opened.Count < 1 || _iterations > 1000)
			{
				Debug.LogError("OH FUCK NO PATH !!!!!!!!!!!!!!");
			}

			_iterations++;
			if(!incrementMode) iteratePath();
		}
	}

	private float calculateFScore(Zone delta)
	{
		return delta.gScore + delta.hScore;
	}

	private float calculateHScore(Zone delta)
	{
		float horizontal = Mathf.Abs(delta.id.x - _dest.id.x);
		float vertical = Mathf.Abs(delta.id.y - _dest.id.y);

		return (horizontal + vertical)*10;
	}

	private float calculateGScore(PATH_DIRECTION direction)
	{
		switch(direction)
		{
			case PATH_DIRECTION.NORTH:
			case PATH_DIRECTION.SOUTH:
			case PATH_DIRECTION.WEST:
			case PATH_DIRECTION.EAST:
				return 10;
				break;

			case PATH_DIRECTION.NORTH_EAST:
			case PATH_DIRECTION.NORTH_WEST:
			case PATH_DIRECTION.SOUTH_EAST:
			case PATH_DIRECTION.SOUTH_WEST:
				return 14;
				break;

			default:
				return 0f;
		}
	}

	private Dictionary<PATH_DIRECTION,Zone> getNieghbors(Zone delta)
	{
		Dictionary<PATH_DIRECTION,Zone> neighbors = new Dictionary<PATH_DIRECTION,Zone>();

		neighbors[PATH_DIRECTION.NORTH] = getNorth(delta);
		neighbors[PATH_DIRECTION.NORTH_EAST] = getNorthEast(delta);
		neighbors[PATH_DIRECTION.NORTH_WEST] = getNorthWest(delta);
		neighbors[PATH_DIRECTION.WEST] = getWest(delta);
		neighbors[PATH_DIRECTION.SOUTH] = getSouth(delta);
		neighbors[PATH_DIRECTION.SOUTH_WEST] = getSouthWest(delta);
		neighbors[PATH_DIRECTION.SOUTH_EAST] = getSouthEast(delta);
		neighbors[PATH_DIRECTION.EAST] = getEast(delta);

		return neighbors;
	}

	private Zone getNorth(Zone delta)
	{
		if(delta.id.y == 0) return null;
		return _grid.TheGrid[(int)delta.id.y - 1][(int)delta.id.x];
	}

	private Zone getNorthEast(Zone delta)
	{
		if(delta.id.y == 0 || delta.id.x >= _grid.TheGrid[(int)delta.id.y].Count-1) return null;

		if(_grid.TheGrid[(int)delta.id.y - 1][(int)delta.id.x].ZoneType == ZONE_TYPE.BLOCK || _grid.TheGrid[(int)delta.id.y][(int)delta.id.x + 1].ZoneType == ZONE_TYPE.BLOCK)
		{
			return null;
		}

		return _grid.TheGrid[(int)delta.id.y - 1][(int)delta.id.x+1];
	}

	private Zone getNorthWest(Zone delta)
	{
		if(delta.id.y == 0 || delta.id.x == 0) return null;

		if (_grid.TheGrid[(int)delta.id.y - 1][(int)delta.id.x].ZoneType == ZONE_TYPE.BLOCK || _grid.TheGrid[(int)delta.id.y][(int)delta.id.x - 1].ZoneType == ZONE_TYPE.BLOCK)
		{
			return null;
		}

		return _grid.TheGrid[(int)delta.id.y - 1][(int)delta.id.x - 1];
	}

	private Zone getWest(Zone delta)
	{
		if(delta.id.x == 0) return null;
		return _grid.TheGrid[(int)delta.id.y][(int)delta.id.x-1];
	}

	private Zone getSouth(Zone delta)
	{
		if(delta.id.y >= _grid.TheGrid.Count-1) return null;
		return _grid.TheGrid[(int)delta.id.y + 1][(int)delta.id.x];
	}

	private Zone getSouthWest(Zone delta)
	{
		if(delta.id.y >= _grid.TheGrid.Count-1 || delta.id.x == 0) return null;

		if (_grid.TheGrid[(int)delta.id.y + 1][(int)delta.id.x].ZoneType == ZONE_TYPE.BLOCK || _grid.TheGrid[(int)delta.id.y][(int)delta.id.x - 1].ZoneType == ZONE_TYPE.BLOCK)
		{
			return null;
		}

		return _grid.TheGrid[(int)delta.id.y + 1][(int)delta.id.x - 1];
	}

	private Zone getSouthEast(Zone delta)
	{
		if(delta.id.y >= _grid.TheGrid.Count-1 || delta.id.x >= _grid.TheGrid[(int)delta.id.y].Count-1) return null;

		if (_grid.TheGrid[(int)delta.id.y + 1][(int)delta.id.x].ZoneType == ZONE_TYPE.BLOCK || _grid.TheGrid[(int)delta.id.y][(int)delta.id.x + 1].ZoneType == ZONE_TYPE.BLOCK)
		{
			return null;
		}

		return _grid.TheGrid[(int)delta.id.y + 1][(int)delta.id.x + 1];
	}

	private Zone getEast(Zone delta)
	{
		if(delta.id.x >= _grid.TheGrid[(int)delta.id.y].Count-1) return null;
		return _grid.TheGrid[(int)delta.id.y][(int)delta.id.x + 1];
	}

	
}