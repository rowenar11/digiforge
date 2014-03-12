using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PayAttentionController : MonoBehaviour 
{
	private bool _init = false;

	public List<Sprite> memes; 
	public GameObject _sprite;

	void Start()
	{
		init();
	}

	private void init()
	{
		if (!_init)
		{
			_init = true;

			_sprite.GetComponent<SpriteRenderer>().sprite = memes[Random.Range(0, memes.Count-1)];
			_sprite.transform.localScale = new Vector3(2f,2f,2f);
		}
		else
		{
			Debug.LogWarning("NOPE");
		}
	}

}
