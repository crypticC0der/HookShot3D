using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
		change = r.material.color.a;
    }
	float change = 0;
	public float speed=0.5F;
	Renderer r;
    // Update is called once per frame
	// fades away the object, and then deletes it
    void Update()
    {
        if (r.material.color.a>0){
			r.material.color -= new Color(0,0,0,change*speed*Time.deltaTime);
		}else{
			GameObject.Destroy(gameObject);
		}
    }
}
