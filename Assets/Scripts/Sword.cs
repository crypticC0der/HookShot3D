using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
	// Start is called before the first frame update
	public float rot=-20;
	public float spd = 60;
	const float rotmax = 60;
	public float r=1.6F;
	public GameObject plyr;
	public GameObject pref;
	bool swiping=false;
	bool lswipe=false;
	float dir = -1;
	Vector3 ppos;

	void createSwipe(){
		Vector3 dpos = transform.position-ppos;
		float dist = Mathf.Sqrt(Mathf.Pow(dpos.x,2)+Mathf.Pow(dpos.y,2)+(Mathf.Pow(dpos.z,2)));
		GameObject g = Instantiate(pref) as GameObject;
		g.transform.localScale = new Vector3(0.05F,0.05F,dist*2F);
		Vector3 v =(transform.position-plyr.transform.position - dpos/2)*(r-.1F);
		g.transform.position = v+plyr.transform.position;
		g.transform.rotation = Quaternion.LookRotation(dpos,new Vector3(-dpos.y,dpos.x,0));
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0)){
				swiping = true;
				dir*=-1;
				if(lswipe){createSwipe();}
				ppos= transform.position;
				transform.GetChild(0).localEulerAngles*=-1;
		}
		if (swiping && ((rot < rotmax && rot > -rotmax)|| (lswipe!=swiping && lswipe==false))){rot+=spd*dir*Time.deltaTime;}else{swiping=false;rot=dir*rotmax;}
		float b = rot/1.5f;
		float a = rot;
		transform.localEulerAngles=new Vector3(90-(dir*b),rot,0);
		a = a*Mathf.PI/180;
		b = b*Mathf.PI/180;
		transform.localPosition = new Vector3(Mathf.Sin(a),Mathf.Sin(dir*b),Mathf.Cos(a))*r;
		if (lswipe && lswipe!=swiping){createSwipe();}
		if (!lswipe && lswipe!=swiping){ppos=transform.position;}
		lswipe=swiping;
	}
}
