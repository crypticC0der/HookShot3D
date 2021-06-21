using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
	public Transform camt;
	Vector3 lmousepos;
    // Start is called before the first frame update
    void Start()
    {
		Cursor.visible=false;
    }
	
	bool preped=false;
    // Update is called once per frame
    void Update()
    {
		Vector3 mpos=Input.mousePosition;
		if (preped){
			Vector3 diff = mpos-lmousepos;
			diff*=Time.deltaTime;
			diff.x*=120;
			diff.y*=-60;
			transform.Rotate(Vector3.up*diff.x);
			camt.Rotate(Vector3.right*diff.y);
			float dot = Globals.Dot(camt.localEulerAngles,Vector3.right);
			if (dot>60 && dot<180){
				camt.localEulerAngles*=60/dot;	
			}
			else if(dot<300 && dot>180){
				camt.localEulerAngles*=300/dot;
			}
		}
		preped=true;
		lmousepos=mpos;
    }
}
