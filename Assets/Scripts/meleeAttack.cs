using UnityEngine;
using System.Collections;

public class meleeAttack : MonoBehaviour {
	
	public GameObject hitbox;

	public float arc;
	public float range;
	public float animationTime;
	public float zOffset;
	private float time;

	[SerializeField] Transform trail;
	TrailArc trailAlt;

	private Quaternion finish;
	private Quaternion start;

	// Use this for initialization
	void Start () {
		//Position and scale the hitbox
		hitbox.transform.localScale.Set(
			hitbox.transform.localScale.x,
			hitbox.transform.localScale.y,
			range);
		Vector3 pos = new Vector3(0,0,0);
		pos.z = zOffset + range/2;
		hitbox.transform.localPosition = pos;

		//Set up the trail
		TrailRenderer trail = hitbox.GetComponent<TrailRenderer>();
		trailAlt = hitbox.GetComponent<TrailArc>();
		if(trail != null)
		{
			trail.startWidth = range * 0.9f;
			trail.endWidth = range * 0.8f;
		}

		//Set up the rotations
		this.transform.Rotate(0, arc/2, 0);
		finish = this.transform.rotation;
		this.transform.Rotate(0,-arc,0);
		start = this.transform.rotation;
		time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(arc >= 180) { altRotate(); }
		else { Rotate (); }

		time += Time.deltaTime / animationTime;

		if (time > 1 || transform.rotation == finish)
			Destroy(this.gameObject);
	}

	void Rotate()
	{
		transform.rotation = Quaternion.Slerp(start, finish, Mathf.Clamp01(time));
	}

	void altRotate()
	{
		transform.Rotate(Vector3.up, ((1/animationTime)*arc)*(Time.deltaTime / Time.timeScale));
	}

	void OnDestroy()
	{
		if (trail != null)
			trail.parent = null;
		if(trailAlt != null)
		{
			trailAlt.emit = false;
			trailAlt.transform.parent = null;
		}
	}
}
