using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity{
	public float armor;
	public float health;
	public float maxHealth;
	public float contactDMG;
	public GameObject self;	

	void Die(){
		GameObject.Destroy(self);
		PlayerControl.Alf.fuel=PlayerControl.Alf.maxFuel;
	}


	public void TakeDamge(float damage){
		damage-=armor;
		if (damage<0){
			damage=0;
		}
		health-=damage;
		if(health<=0){
			Die();	
		}
	}
}

public class ArmedEntity:Entity{
	public Weapon weapon;
}

public class Player:ArmedEntity{
	public float maxFuel;
	public float fuelRegen; 
	public float fuel;
	public bool boosting;
	public float boostMod;
	public Transform[] bars;
	
	public float GetContactDMG(){
		float ret = weapon.GetDMG()*contactDMG;
		if (boosting){
			ret*=boostMod;
		}
		return ret;
	}

	void StatToBar(Transform bar,float current,float max){
		bar.localScale = new Vector3(3.5f*current/max,.25f,1);
		bar.localPosition = new Vector3((bar.localScale.x-3.5f)/2,0,0);
	}
	
	public void UpdateBars(){
	    StatToBar(bars[0],health,maxHealth); 
        StatToBar(bars[1],fuel,maxFuel); 
	    bars[1].localPosition*=-1;
	}

	public void UpdateFuel(){
		if(fuel<maxFuel){
			fuel+=fuelRegen*Time.deltaTime;
			if(fuel>maxFuel){
				fuel=maxFuel;
			}
			UpdateBars();
		}
	}
}

public class Weapon{
	public float damage;
	public float radSpd;
	public float max_rot;
	//public float part_interval;
	public GameObject self;
	//public GameObject particle;
	//public float particleDMG;
	public float critChance;
	
	public float GetDMG(){
		if (Random.value<=critChance){
			return damage*10;
		}
		return damage;
	}
	//public float GetParticleDMG(){
	//	return GetDMG()*particleDMG;
	//}
}
