using UnityEngine;
using System.Collections;

public class Fuck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


//	void OnCollisionEnter(Collision collision)
//	{
//		Debug.Log("Collided with someone");
//	}
//
//
//	void OnTriggerEnter(Collider other)
//	{
//		Debug.Log("TRIGGER!");
//	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("1");
	}

	void OnCollisionExit2D(Collision2D other)
	{
		Debug.Log("2");
	}

	void OnCollisionStay2D(Collision2D other)
	{
		Debug.Log("3");
	}


}
