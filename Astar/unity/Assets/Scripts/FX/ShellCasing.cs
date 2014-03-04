using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShellCasing : MonoBehaviour
{
	private bool _onGround = false;
	private bool _animate = false;
	private bool _init = false;

	public float _xVelocity = 0;
	public float _yVelocity = 0;

	private int _dragType;
	private float _drag = 0;

	private float _rVelocity = 0;

	public float maxMove;

	private Vector3 origin;

	public void Init(Vector3 startingPos,Sprite shellCasingSprite)
	{
		if (!_init)
		{
			_init = true;

			maxMove = Random.Range(6, 11) * 0.1f;
			origin = gameObject.transform.position = startingPos;

			this.GetComponent<SpriteRenderer>().sprite = shellCasingSprite;

			int xV = Random.Range(3, 10);
			_xVelocity = (xV - 5)*1f;

			int yV = Random.Range(3, 10);
			_yVelocity = (yV - 5)*1f;

			int rV = Random.Range(3, 10);
			_rVelocity = (rV - 5) * 0.1f;

			_animate = true;
		}
	}
	
	void Update () 
	{
		if(_animate)
		{
			gameObject.transform.position =
				new Vector3(gameObject.transform.position.x+(_xVelocity * Time.deltaTime),gameObject.transform.position.y + (_yVelocity * Time.deltaTime),gameObject.transform.position.z);

			Vector3 diff = origin - gameObject.transform.position;

			gameObject.transform.Rotate(new Vector3(0,0,100 * Time.deltaTime));

			if((_xVelocity==0 && _yVelocity==0) || (Mathf.Abs(diff.x) > maxMove || Mathf.Abs(diff.y) > maxMove))
			{
				_animate = false;
				StartCoroutine(erase());
			}
		}
	}

	private IEnumerator erase()
	{
		yield return new WaitForSeconds(0.75f);
		Dispose();
	}

	public void Dispose()
	{
		gameObject.SetActive(false);
		_animate = _init = false;
		MainTurret.instance.TurretDude.ShellCasingFactory.ReturnToPool(this);
	}
}
