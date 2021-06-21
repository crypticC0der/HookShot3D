using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals 
{
	public static float Dot(Vector3 a,Vector3 b){
		return a.x*b.x + a.y*b.y + a.z*b.z;
	}
	
	public static float Pythag(Vector3 v){
		return Mathf.Sqrt(v.x*v.x + v.y*v.y + v.z*v.z);
	}

	public static float Pos(float x){
		if (x<0){
			return -x;
		}else{
			return x;
		}
	}
}
