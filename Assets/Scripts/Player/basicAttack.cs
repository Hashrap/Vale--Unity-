using UnityEngine;
using System.Collections;

public class basicAttack : MonoBehaviour {
	public float speed;
	public float lifetime;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		lifetime -= Time.deltaTime;
		transform.Translate(speed * transform.forward * Time.deltaTime, Space.World);

		if(lifetime < 0)
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Enemy" && this.gameObject.tag == "pBullet")
		{
			Destroy(this.gameObject);
		}
		if(other.tag == "Player" && this.gameObject.tag == "eBullet")
		{
			Destroy(this.gameObject);
		}
	}
}
