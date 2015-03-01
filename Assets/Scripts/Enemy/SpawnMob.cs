
using UnityEngine;
using System.Collections;

public class SpawnMob : MonoBehaviour {

	public int iterations;
	public float iterationDelay;
	public int mobsPerIteration;
	private int iterationCount;
	private int mobCount;

	public GameObject mob;
	private GameObject[] spawned;

	private Vector3 target;
	public float radius;
	public void setTarget(Vector3 pos) { target = pos; }

	private float timer;

	// Use this for initialization
	void Start () {
		iterationCount = 0;
		mobCount = 0;
		timer = iterationDelay;
		spawned = new GameObject[iterations*mobsPerIteration];
		target = this.transform.position;
	}

	public void removeMob(int id)
	{
		spawned[id] = null;
	}
	
	// Update is called once per frame
	void Update () {
		if(iterationCount == iterations && mobCount == iterationCount * mobsPerIteration)
		{
			bool empty = true;
			for(int i = 0; i < spawned.Length; i++)
			{
				if(spawned[i] != null)
					empty = false;
			}
			if(empty)
				Destroy(this.gameObject);
		}
		else if(timer > 0)
			timer -= Time.deltaTime;
		else if(iterationCount < iterations && timer <= 0)
		{
			for (int i = 0; i < mobsPerIteration; i++)
			{
				Vector2 offset = Random.insideUnitCircle.normalized;
				offset *= radius;
				Vector3 dest = target;
				dest.x += offset.x;
				dest.z += offset.y;

				spawned[mobCount] = (GameObject)Instantiate(mob, dest, this.transform.rotation);
				mobCount++;
			}
			iterationCount++;
			timer = iterationDelay;
		}
	}

	void OnDestroy()
	{

	}
}
