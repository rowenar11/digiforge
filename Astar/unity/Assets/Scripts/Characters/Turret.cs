using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour 
{
	private Animator _animator;

	private bool _initted = false;

	public GameObject shellCasings;

	public GameObject bulletHoleFactoryObj;
	public BulletHoleFactory BulletHoleFactory;

	public GameObject shellCasingFactoryObj;
	public ShellCasingFactory ShellCasingFactory;

	public void init()
	{
		if(!_initted)
		{
			_animator = this.GetComponent<Animator>();
			_initted = true;

			GameObject bFact = (GameObject)Instantiate(bulletHoleFactoryObj);
			BulletHoleFactory = bFact.GetComponent<BulletHoleFactory>();
			BulletHoleFactory.init();

			GameObject sFact = (GameObject)Instantiate(shellCasingFactoryObj);
			ShellCasingFactory = sFact.GetComponent<ShellCasingFactory>();
			ShellCasingFactory.init();
		}
	}

	private RaycastHit hit;

	// Update is called once per frame
	void Update()
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
					ShellCasing shellCasing = ShellCasingFactory.GetShellCasing(gameObject.transform.position);
				}

				RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
				Debug.DrawRay(transform.position, transform.right*8, Color.green);
				if(hit.collider != null)
				{
					BulletHole bulletHole = BulletHoleFactory.GetBullet(hit.collider.gameObject,hit.point);
				}
			}
			else
			{
				_animator.SetInteger("Fire", 0);
			}
		}
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		//if(_zone != null && _zone.initted && other != null) _zone.OnTriggerEnter2D(other);
		if(other.gameObject.GetComponent<ZoneCollider>() != null)
		{
			ZoneCollider zc = other.gameObject.GetComponent<ZoneCollider>();
			zc.zone.setHeroState();
			Debug.Log("HERO AND STUFF");
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		//if(_zone != null && _zone.initted && other != null) _zone.OnTriggerExit2D(other);
//		Debug.Log("OnTriggerExit2D");
	}

	void OnTriggerStay2D(Collider2D other)
	{
		//if(_zone != null && other != null) _zone.OnTriggerExit2D(other);
//		Debug.Log("OnTriggerStay2D");
	}
}
