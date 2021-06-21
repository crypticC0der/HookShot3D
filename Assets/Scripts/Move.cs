using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	public float lamda;
	ParticleSystem.EmissionModule em;
	Camera cam;
	GameObject hitObj;
	public Transform camt;
	Rigidbody rb;
	bool hooked=false;
	GameObject hook;
	GameObject line;
	bool connected;
	Vector3 hookPoint;
	Vector3 hookVel;
    // Start is called before the first frame update
    void Start()
    {
		em = (GetComponent<ParticleSystem>() as ParticleSystem).emission;
		rb=GetComponent<Rigidbody>() as Rigidbody;
		cam=Camera.main;
    }

	void FixedUpdate(){
		rb.AddForce(Physics.gravity*5f,ForceMode.Acceleration);
	}
    // Update is called once per frame
    void Update()
    {
        
		float h=Input.GetAxis("Horizontal");
		float v=Input.GetAxis("Vertical");
		Vector3 vec=transform.forward*v + transform.right*h;
		vec/=Mathf.Sqrt(2);
		rb.AddForce(vec*24);	

		Vector3 mpos = Input.mousePosition;
		mpos.z=1;
		RaycastHit rh;
		bool hit = Physics.Raycast(camt.position,camt.forward*.9f + camt.up*.1f,out rh,50,1<<7,QueryTriggerInteraction.UseGlobal);
		if(hit){
			if (hitObj==null){
				hitObj = Instantiate(Resources.Load("Hit") as GameObject);
			}
			if(Input.GetMouseButtonDown(1)){
				if(hooked){
					Destroy(hook);
					Destroy(line);
				}else{
					line = Instantiate(Resources.Load("line") as GameObject);	
					hook = Instantiate(Resources.Load("hook") as GameObject);	
					hook.transform.position = transform.position;
					hookVel = rh.point-transform.position;
					hookVel=hookVel.normalized*200;
				}
				connected=false;
				hookPoint=rh.point;	
				hooked=!hooked;
			}
			hitObj.transform.position=rh.point;
		}else{
			Destroy(hitObj);
			hitObj=null;
			if(Input.GetMouseButtonDown(1)){
				Destroy(hook);
				Destroy(line);
				connected=false;
				hooked=false;
			}
		}

		if(hooked){
			if(connected){
				hook.transform.position=hookPoint;
				Vector3 acc=hookPoint-transform.position;
				acc*=lamda;
				rb.AddForce(2*acc);
				Vector3 hand = transform.position+transform.right*.5f;
				line.transform.position = (hand+hook.transform.position)/2;
				Vector3 dist=hook.transform.position-hand;
				line.transform.localScale=new Vector3(0.1f,0.1f,Globals.Pythag(dist));
				line.transform.LookAt(hand);
			}else{
				hook.transform.position+=hookVel*Time.deltaTime;
				connected = Globals.Dot(hookVel,hookPoint-hook.transform.position)<0;
			}
		}
		PlayerControl.Alf.boosting =(Input.GetAxis("Boost")>0 && PlayerControl.Alf.fuel>0) && (PlayerControl.Alf.boosting||PlayerControl.Alf.maxFuel/2<PlayerControl.Alf.fuel);
		em.enabled=PlayerControl.Alf.boosting;
		if (PlayerControl.Alf.boosting){
			PlayerControl.Alf.fuel -=Time.deltaTime;
			if (PlayerControl.Alf.fuel<0){
				PlayerControl.Alf.fuel=0;
				PlayerControl.Alf.boosting=false;
			}
			PlayerControl.Alf.UpdateBars();
			rb.AddForce(50*camt.forward);				
		}
    }
}
