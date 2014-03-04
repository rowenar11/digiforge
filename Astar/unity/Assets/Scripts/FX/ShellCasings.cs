using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShellCasings : MonoBehaviour
{
	public Sprite casing1;
	public Sprite casing2;
	public Sprite casing3;
	public Sprite casing4;
	public Sprite casing5;
	public Sprite casing6;
	public Sprite casing7;

	private List<Sprite> _casings;

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

	public void Init(Vector3 startingPos)
	{
		if (!_init)
		{
			maxMove = Random.Range(6, 11) * 0.1f;

			_init = true;

			_casings = new List<Sprite>() {casing1, casing2, casing3, casing4, casing5, casing6, casing7};

			origin = gameObject.transform.position = startingPos;

			int randomCasing = Random.Range(1, 7);
			this.GetComponent<SpriteRenderer>().sprite = _casings[randomCasing];

			int xV = Random.Range(3, 10);
			_xVelocity = (xV - 5)*0.008f;

			int yV = Random.Range(3, 10);
			_yVelocity = (yV - 5) * 0.008f;

			int rV = Random.Range(3, 10);
			_rVelocity = (rV - 5) * 0.1f;

			_animate = true;
		}
	}
	
	void Update () 
	{
		if(_animate)
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x + _xVelocity, gameObject.transform.position.y + _yVelocity, gameObject.transform.position.z);
			Vector3 diff = origin - gameObject.transform.position;

			//_yVelocity += 0.0016f;

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
		Destroy(gameObject);
	}
}
