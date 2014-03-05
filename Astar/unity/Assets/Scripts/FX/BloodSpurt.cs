using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodSpurt : MonoBehaviour {

	public List<Sprite> BloodSpurts;

	public float framesPerSecond=10;

	private SpriteRenderer _spriteRenderer;

	private bool _animate = true;
	private bool _init = false;

	private bool _playOnce = true;

	private int iterations = 0;

	public void Init(Vector3 position)
	{
		if (!_init)
		{
			_init = true;
			gameObject.transform.position = position;
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}
	}

	void Update()
	{
		if (_animate)
		{
			int index = (int) (Time.timeSinceLevelLoad*framesPerSecond);
			index = index%BloodSpurts.Count;
			if(index == 0)
			{
				if(iterations == 1 && _playOnce)
				{
					Dispose();
				}
				iterations++;
			}

			_spriteRenderer.sprite = BloodSpurts[index];
		}
	}

	public void Dispose()
	{
		iterations = 0;
		gameObject.SetActive(false);
		_animate = _init = false;
		MainTurret.instance.TurretDude.BloodFactory.ReturnToPool(this);
	}
}
