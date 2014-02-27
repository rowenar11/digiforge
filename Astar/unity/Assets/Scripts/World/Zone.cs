using UnityEngine;
using System.Collections;

public enum ZONE_TYPE
{
	zone1,
	zone2
}

public class Zone : MonoBehaviour 
{
	public Material ONE;
	public Material TWO;
	
	public void init(ZONE_TYPE type)
	{
		switch(type)
		{
			case ZONE_TYPE.zone1: this.gameObject.renderer.material = ONE; break;
			case ZONE_TYPE.zone2: this.gameObject.renderer.material = TWO; break;
		}
	}
}