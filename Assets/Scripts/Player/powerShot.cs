using UnityEngine;
using System.Collections;

public class powerShot : MonoBehaviour {

	private float speed;
	public void setSpeed(float s) { speed = s; }
	public float getSpeed() { return speed; }

	public float lifetime;

	[SerializeField] Transform trail;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		lifetime -= Time.deltaTime;
		if(lifetime < 0) { Destroy(this.gameObject); }
		transform.Translate(speed * this.transform.up * (Time.deltaTime / Time.timeScale), Space.World);
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "obstacle")
		{
			Destroy(this.gameObject);
		}
	}
	void OnDestroy()
	{
		if (trail != null && trail.parent != null)
			trail.parent = null;
	}
}
