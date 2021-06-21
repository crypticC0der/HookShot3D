using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	float jumpforce=15;
	bool grounded=false;
	Vector3 impvec;
	public Transform shot;
	bool jump=true;
	ParticleSystem.EmissionModule em;
	float aim =50;
	Camera cam;
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
		float h=Input.GetAxis("Horizontal");
		float v=Input.GetAxis("Vertical");
		Vector3 vec=transform.forward*v + transform.right*h;
		vec/=Mathf.Sqrt(2);
		if (grounded||hooked){
			rb.AddForce(vec*60);	
		}else{
			rb.AddForce(vec*40);	
		}
		if(hooked && connected){
			hook.transform.position=hookPoint;
			Vector3 acc=hookPoint-transform.position;
			rb.AddForce(5*acc);
		}
		if (PlayerControl.Alf.boosting){
			PlayerControl.Alf.fuel -=Time.deltaTime;
			if (PlayerControl.Alf.fuel<0){
				PlayerControl.Alf.fuel=0;
				PlayerControl.Alf.boosting=false;
			}
			PlayerControl.Alf.UpdateBars();
			rb.AddForce(75*camt.forward);				
		}
		if (Input.GetAxis("Jump")>0 && jump){
			jump=false;
			rb.AddForce(impvec*jumpforce,ForceMode.Impulse);
		}
		rb.AddForce(Physics.gravity*5f,ForceMode.Acceleration);
	}
    // Update is called once per frame
    void Update()
    {
		Vector3 mpos = Input.mousePosition;
		mpos.z=1;
		RaycastHit rh;
		bool hit = Physics.Raycast(camt.position,camt.forward*.95f + camt.up*.05f,out rh,50,1<<7,QueryTriggerInteraction.UseGlobal);
		if(hit){
			aim=20;
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
		}else{
			aim=50;
			if(Input.GetMouseButtonDown(1)){
				Destroy(hook);
				Destroy(line);
				connected=false;
				hooked=false;
			}
		}

		if(hooked){
			if(connected){
				Vector3 hand = transform.position+transform.right*.5f;
				line.transform.position = (hand+hook.transform.position)/2;
				Vector3 dist=hook.transform.position-hand;
				line.transform.localScale=new Vector3(0.05f,0.05f,Globals.Pythag(dist));
				line.transform.LookAt(hand);
			}else{
				hook.transform.position+=hookVel*Time.deltaTime;
				connected = Globals.Dot(hookVel,hookPoint-hook.transform.position)<0;
			}
		}
		PlayerControl.Alf.boosting =(Input.GetAxis("Boost")>0 && PlayerControl.Alf.fuel>0) && (PlayerControl.Alf.boosting||PlayerControl.Alf.maxFuel/2<PlayerControl.Alf.fuel);
		em.enabled=PlayerControl.Alf.boosting;

		for (int i=0;i<4;i++){
			Transform child = shot.GetChild(i);
			Vector3 aimVec = new Vector3(Mathf.Sin(i*Mathf.PI/2),Mathf.Cos(i*Mathf.PI/2)) * aim;
			aimVec = aimVec -child.localPosition;
			child.localPosition+=aimVec*Time.deltaTime*4;
		}

    }

	void OnCollisionStay(Collision col){
		if(Mathf.Sqrt(col.impulse.x * col.impulse.x + col.impulse.z *col.impulse.z)<col.impulse.y){
			grounded=true;
		}
	}

	void OnCollisionExit(Collision col){
		grounded=false;
	}

	void OnCollisionEnter(Collision col){
		if(Mathf.Sqrt(col.impulse.x * col.impulse.x + col.impulse.z *col.impulse.z)<col.impulse.y){
			impvec=col.impulse.normalized;
			jump=true;
		}
	}
}
