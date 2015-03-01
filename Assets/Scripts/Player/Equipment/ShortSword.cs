using UnityEngine;
using System.Collections;

public class ShortSword : Weapon {
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	protected override void useBasicAttack()
	{
		/*GameObject temp = (GameObject)*/Instantiate(attack, this.transform.position, this.transform.rotation);
		//temp.transform.parent = this.transform;
		base.useBasicAttack();
	}

	protected override void usePowerAttack()
	{
		/*GameObject temp = (GameObject)*/Instantiate(powerAttack, this.transform.position, this.transform.rotation);
		//temp.transform.parent = this.transform;
		base.usePowerAttack();
	}
}
