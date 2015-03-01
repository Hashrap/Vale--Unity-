using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public GameObject player;
	public Player p1;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
		p1 = player.GetComponent<Player>();
		Vector2 pushVec = Random.insideUnitCircle;
		rigidbody.AddForce(pushVec.x*20,0.0f,pushVec.y*20,ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Vector3.Distance(player.transform.position, this.transform.position) < p1.pickupRange)
		{
			rigidbody.AddForce((player.transform.position-this.transform.position).normalized*100, ForceMode.Acceleration);
		}
	}
}
