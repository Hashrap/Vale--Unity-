using UnityEngine;
using System.Collections;

public class attackBase : MonoBehaviour {
	private GameObject source;
	public GameObject getSource() { return source; }
	public void setSource(GameObject s) { source = s; }

	public int damage;
	public int getDamage(){ return damage; }
	public void setDamage(int d){ damage = d; }

	public float force;
	public float getForce(){ return force; }
	public void setForce(float f){ force = f; }

	public GameObject channel;
	public float attackPoint;
	public float recoverTime;
	public float getAtkTime() { return attackPoint + recoverTime; }

	public float cooldown;

	public float attackRange;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
