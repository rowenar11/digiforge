using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour 
{
	private Animator _animator;

	private bool _initted = false;

	public GameObject shellCasings;
	public GameObject bulletHole;

	public void init()
	{
		if(!_initted)
		{
			Debug.Log("here");
			_animator = this.GetComponent<Animator>();
			_initted = true;
		}
		else
		{
			Debug.Log("nope noep");
		}
	}

	private RaycastHit hit;

	// Update is called once per frame
	void Update () 
	{
		if(_initted)
		{
			var objectPos = Camera.main.WorldToScreenPoint(transform.position);
			var dir = Input.mousePosition - objectPos;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg));
			
			if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
			{
				_animator.SetInteger("Fire", 1);

				if(Random.Range(0,3) == 2)
				{
					GameObject shellCasing = (GameObject) Instantiate(shellCasings);
					ShellCasings sC = shellCasing.GetComponent<ShellCasings>();
					sC.Init(gameObject.transform.position);
				}

				RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
				Debug.DrawRay(transform.position, transform.right*8, Color.green);
				if(hit.collider != null)
				{
					GameObject bulletWallSpecks = (GameObject)Instantiate(bulletHole);
					bulletWallSpecks.GetComponent<BulletHoles>().Init(hit.collider.gameObject, hit.point);
				}

				//_initted = false;
			}
			else
			{
				_animator.SetInteger("Fire", 0);
			}

			//Debug.Log(_animator.GetInteger("Fire"));
		}
	}


}
