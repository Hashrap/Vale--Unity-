using UnityEngine;
using System.Collections;

public class DefaultBow : Weapon {

	public int minDamage;
	public int chargeDamage;
	public int sweetspotDamageBonus;
	
	public float minSpeed;
	public float chargeSpeed;
	public float sweetspotSpeedBonus;

	protected override void Start()
	{
		timer = 0.0f;
		isChanneling = false;
		base.Start();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	protected override void useBasicAttack()
	{
		Instantiate(attack, this.transform.position, this.transform.rotation);
		base.useBasicAttack();
	}

	protected override void usePowerAttack()
	{
		Quaternion rot = this.transform.rotation;
		rot.SetLookRotation(Vector3.down, this.transform.forward);
		
		int damage = (int)(minDamage+Mathf.Clamp(timer/minSweet,0,1)*chargeDamage);
		float speed = minSpeed+Mathf.Clamp(timer/minSweet,0,1)*chargeSpeed;
		
		if(timer >= minSweet) {
			if(timer <= maxSweet)
			{
				GetComponent<AudioSource>().PlayOneShot(m2sweetClip);
				damage += sweetspotDamageBonus;
				speed += sweetspotSpeedBonus;
			}
			else
				GetComponent<AudioSource>().PlayOneShot(m2wildClip);
			
		}
		else { GetComponent<AudioSource>().PlayOneShot(m2weakClip); }
		
		GameObject temp = (GameObject)Instantiate(powerAttack, this.transform.position, rot);
		temp.GetComponent<powerShot>().setSpeed(speed);
		temp.GetComponent<attackBase>().setDamage(damage);

		base.usePowerAttack();
	}
}