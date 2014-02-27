using UnityEngine;
using System.Collections;

public class Touchable : MonoBehaviour 
{
	protected int dragFingerIndex = -1;

	private bool tapEnabled = false;
	public void enableTap()
	{
		if (!tapEnabled)
		{
			FingerGestures.OnTap += onTapCheck;
			tapEnabled = true;
		}
	}
	public void disableTap()
	{
		if (tapEnabled)
		{
			FingerGestures.OnTap -= onTapCheck;
			tapEnabled = false;
		}
	}
	private void onTapCheck(UnityEngine.Vector2 fingerPos)
	{
		GameObject selection = PickObject( fingerPos );
        if( !selection || selection != gameObject )
        {
	        
        }
        else
        {
	        OnTap(fingerPos);
        }
	}

	protected virtual void OnTap(UnityEngine.Vector2 fingerPos)
	{
		
	}

	private bool doubleTapEnabled = false;
	public void enableDoubleTap()
	{
		if (!doubleTapEnabled)
		{
			doubleTapEnabled = true;
			FingerGestures.OnFingerDoubleTap += OnDoubleTapCheck;
		}
	}
	public void disableDoubleTap()
	{
		if (doubleTapEnabled)
		{
			doubleTapEnabled = false;
			FingerGestures.OnFingerDoubleTap -= OnDoubleTapCheck;
		}
	}
	private void OnDoubleTapCheck(int fingerIndex, Vector2 fingerPos)
	{
		GameObject selection = PickObject(fingerPos);
		if (!selection || selection != gameObject)
		{

		}
		else
		{
			OnDoubleTap(fingerIndex,fingerPos);
		}
	}
	protected virtual void OnDoubleTap(int fingerIndex, Vector2 fingerPos)
	{
		
	}

	private bool findDown = false;
	public void enableFingerDown()
	{
		if (!findDown)
		{
			findDown = true;
			FingerGestures.OnFingerDown += OnFingerDown;
		}
	}
	public void disableFingerDown()
	{
		if (findDown)
		{
			FingerGestures.OnFingerDown -= OnFingerDown;
			findDown = false;
		}
	}
	protected virtual void OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		
	}

	private bool fingerUpEnabled = false;
	public void enableFingerUp()
	{
		if(!fingerUpEnabled)
		{
			fingerUpEnabled = true;
			FingerGestures.OnFingerUp += OnFingerUp;
		}
	}
	public void disableFingerUp()
	{
		if (fingerUpEnabled)
		{
			fingerUpEnabled = false;
			FingerGestures.OnFingerUp -= OnFingerUp;
		}
	}
	protected virtual void OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		
	}

	private bool draggingAndShit=false;
	public void enableDrag()
	{
		if (!draggingAndShit)
		{
			draggingAndShit = true;
			FingerGestures.OnFingerDragBegin += DragBeginCheck;
			FingerGestures.OnFingerDragMove += DragMoveCheck;
			FingerGestures.OnFingerDragEnd += DragEndCheck; 
		}
	}
	public void disableDrag()
	{
		if(draggingAndShit)
		{
			draggingAndShit = false;
			FingerGestures.OnFingerDragBegin -= DragBeginCheck;
			FingerGestures.OnFingerDragMove -= DragMoveCheck;
			FingerGestures.OnFingerDragEnd -= DragEndCheck; 
		}
	}
	private void DragBeginCheck(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
		GameObject selection = PickObject(startPos);
		if (selection == gameObject)
		{
			dragFingerIndex = fingerIndex;
			DragBegin(fingerIndex, fingerPos, startPos);
		}
	}

	protected virtual void DragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
		
	}

	private void DragMoveCheck(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
		if (fingerIndex == dragFingerIndex)
		{
			DragMove(fingerIndex, fingerPos, delta);
		}
	}
	protected virtual void DragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
		gameObject.transform.position = new Vector3(GetWorldPos(fingerPos).x, GetWorldPos(fingerPos).y,gameObject.transform.position.z);
	}
	
	protected void DragEndCheck(int fingerIndex, Vector2 fingerPos)
	{
		// we make sure that this event comes from the finger that is dragging our dragObject
		if (fingerIndex == dragFingerIndex)
		{
			// reset our drag finger index
			dragFingerIndex = -1;
			DragEnd(fingerIndex, fingerPos);
		}
	}
	protected virtual void DragEnd(int fingerIndex, Vector2 fingerPos)
	{
		
	}

	// Convert from screen-space coordinates to world-space coordinates on the Z = 0 plane
	public static Vector3 GetWorldPos(Vector2 screenPos)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPos);

		// we solve for intersection with z = 0 plane
		float t = -ray.origin.z / ray.direction.z;

		return ray.GetPoint(t);
	}

	// Return the GameObject at the given screen position, or null if no valid object was found
	public static GameObject PickObject(Vector2 screenPos)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
			return hit.collider.gameObject;

		return null;
	}

	private bool mouseOvrEnabled=false;
	public void enableMouseOvr()
	{
		if(!mouseOvrEnabled) mouseOvrEnabled = true;
	}
	public void disableMouseOvr()
	{
		if(mouseOvrEnabled) mouseOvrEnabled = false;
	}

	protected virtual void OnMouseOvr()
	{
		
	}
	void OnMouseOver()
	{
		if(mouseOvrEnabled) OnMouseOvr();
	}

	protected virtual void OnMouseOff()
	{

	}
	void OnMouseExit()
	{
		if(mouseOvrEnabled) OnMouseOff();
	}

	void OnDestroy()
	{
		disableTap();
		disableDoubleTap();
		disableFingerDown();
		disableFingerUp();
		disableDrag();
	}
}
