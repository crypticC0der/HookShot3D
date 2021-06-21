using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
	public static Player Alf;
	public Transform[] bars;
	//public GameObject sord;
	//public GameObject particle;
	
    void Start()
    {
		Alf = new Player();
		Alf.weapon = new Weapon();
		Alf.weapon.damage=100;
		Alf.weapon.radSpd=1280;
		Alf.weapon.max_rot=60;
		//Alf.weapon.part_interval=1F;
		//Alf.weapon.self=sord;
		//Alf.weapon.particle=particle;
		//Alf.weapon.particleDMG=0.5F;
		Alf.weapon.critChance=0.01F;
		Alf.maxHealth=50;
		Alf.fuelRegen=0.025F;
		Alf.maxFuel =0.5F;
		Alf.health=Alf.maxHealth;
		Alf.fuel=Alf.maxFuel;
		Alf.self=gameObject;
		Alf.armor=0;
		Alf.boosting=false;
		Alf.boostMod=4;
		Alf.contactDMG=.5F;
		Alf.bars=bars;
		Alf.UpdateBars();
    }

    // Update is called once per frame
    void LateUpdate()
    {
		Alf.UpdateFuel();
	}
}
