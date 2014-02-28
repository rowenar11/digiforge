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

	private bool found = false;
	private List<Zone> path;

	private Grid _grid = Main.instance.Grid;

	private Zone _source;
	private Zone _dest;
	private Zone _current;

	public List<Zone> FindPath(Zone source,Zone dest)
	{
		opened = new List<Zone>();
		closed = new List<Zone>();

		Debug.LogWarning("");
		Debug.LogWarning("FindPath() "+source.id+" : "+dest.id);
		_current = source;

		iteratePath(_current);
		return path;
	}

	public void Step()
	{
		Debug.LogWarning(" *|* ");
		iteratePath(_current);
	}

	private void iteratePath(Zone currentZone)
	{
		opened.Add(currentZone);

		Dictionary<PATH_DIRECTION,Zone> neighbors = getNieghbors(currentZone);
		Debug.Log(" Neighbors : " + neighbors);
		foreach(KeyValuePair<PATH_DIRECTION,Zone> kvp in neighbors)
		{
			Debug.LogError("NEIGHBOR : "+kvp.Key+" "+kvp.Value+" : "+kvp.Value.id);
			kvp.Value.setNeighborState();
		}

		calculateScores(neighbors);

		Zone nextZone = currentZone;

		found = true;
		if(!found) iteratePath(nextZone);
	}

	private void calculateScores(Dictionary<PATH_DIRECTION,Zone> neighbors)
	{
		Debug.LogWarning("calculateScores() ");

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
		if(delta.id.y < 1) return null;
		return _grid.TheGrid[(int)delta.id.y - 1][(int)delta.id.x];
	}

	private Zone getNorthEast(Zone delta)
	{
		if(delta.id.y < 1 || delta.id.x >= _grid.TheGrid[(int)delta.id.y].Count) return null;
		return _grid.TheGrid[(int)delta.id.y - 1][(int)delta.id.x+1];
	}

	private Zone getNorthWest(Zone delta)
	{
		if(delta.id.y < 1 || delta.id.x < 0) return null;
		return _grid.TheGrid[(int)delta.id.y - 1][(int)delta.id.x - 1];
	}

	private Zone getWest(Zone delta)
	{
		if(delta.id.x < 0) return null;
		return _grid.TheGrid[(int)delta.id.y][(int)delta.id.x-1];
	}

	private Zone getSouth(Zone delta)
	{
		if(delta.id.y >= _grid.TheGrid.Count) return null;
		return _grid.TheGrid[(int)delta.id.y + 1][(int)delta.id.x];
	}

	private Zone getSouthWest(Zone delta)
	{
		if(delta.id.y >= _grid.TheGrid.Count || delta.id.x < 0) return null;
		return _grid.TheGrid[(int)delta.id.y + 1][(int)delta.id.x - 1];
	}

	private Zone getSouthEast(Zone delta)
	{
		if(delta.id.y >= _grid.TheGrid.Count || delta.id.x >= _grid.TheGrid[(int)delta.id.y].Count) return null;
		return _grid.TheGrid[(int)delta.id.y + 1][(int)delta.id.x + 1];
	}

	private Zone getEast(Zone delta)
	{
		if(delta.id.x >= _grid.TheGrid[(int)delta.id.y].Count) return null;
		return _grid.TheGrid[(int)delta.id.y][(int)delta.id.x + 1];
	}

	
}