using UnityEngine;
using System.Collections;

public class Enemy: MonoBehaviour {

	private static GameObject target;
	private NavMeshAgent navAgent;
	public float speed;
	
	public GameObject[] attackPrefabs;
	private attackBase[] atks;
	private float[] cooldowns;
	private float[] maxCooldowns;
	private int atk;

	private bool attacking;
	private bool backswing;
	private float atkTimer;
	public float globalCDMax;
	private float globalCD;
	public float aggroRange;
	public float preferredRange;
	public float avoidRange;

	public int health;
	private bool dead;
	private SpawnMob spawner = null;
	private int spawnID;
	public void setSpawner(SpawnMob scrpt) { spawner = scrpt; }
	public void setID(int i) { spawnID = i; }
	public int getID() { return spawnID; }
	
	public int hpDrop;
	public int tpDrop;
	public int valueMax;
	public int valueMin;
	public GameObject healthDrop;
	public GameObject techDrop;
	public GameObject coinDrop;

	void Awake() {
		attacking = false;
		backswing = false;
		dead = false;
		globalCD = 0.0f;
		atkTimer = 0.0f;
	}

	// Use this for initialization
	void Start () {
		atks = new attackBase[attackPrefabs.Length];
		maxCooldowns = new float[attackPrefabs.Length];
		cooldowns = new float[attackPrefabs.Length];
		for(int i = 0; i < attackPrefabs.Length; i++)
		{
			atks[i] = attackPrefabs[i].GetComponentsInChildren<attackBase>(true)[0];
			maxCooldowns[i] = atks[i].cooldown;
			cooldowns[i] = 0.0f;
		}

		target = GameObject.FindWithTag("Player");
		navAgent = this.gameObject.GetComponent<NavMeshAgent>();
		navAgent.speed = speed;
	}

	void Update () {
		//Increment relevent timers
		if(atkTimer > 0) { atkTimer -= Time.deltaTime; }
		if(globalCD > 0) { globalCD -= Time.deltaTime; }
		for(int i = 0; i < cooldowns.Length; i++)
		{
			if(cooldowns[i] > 0)
				cooldowns[i] -= Time.deltaTime;
		}

		//Dead?
		if (health <= 0)
		{
			if(!dead)
				die();
			return;
		}

		//In the middle of an attack?
		if(attacking)
		{
			if (!backswing && atkTimer < atks[atk].recoverTime)
			{
				attack ();
				backswing = true;
			}
			if (atkTimer <= 0)
			{
				backswing = false;
				attacking = false;
				navAgent.updatePosition = true;
			}

			return;
		}

		//How far from the player are we?
		float distance = Vector3.Distance(transform.position, target.transform.position);
		atk = selectAttack(distance);
		
		//Too far to care
		if (distance > aggroRange) { return; }

		//Too close for comfort
		else if(distance < avoidRange)
		{
			//No abilities available to try and discourage attackers
			if(globalCD > 0 || atk == -1)
			{
				//Run away!
				if(!navAgent.updatePosition)
					navAgent.updatePosition = true;
				Vector3 goalVec = transform.position - target.transform.position;
				goalVec.Normalize();
				navAgent.SetDestination(transform.position + goalVec);
				return;
			}
			//Defensive attack!
			else if (atks[atk].attackRange > distance)
				prepAttack(distance);
		}
		//No abilities off cooldown/in range, and the unit isn't as close as it would like
		else if (atk == -1 && distance > preferredRange)
		{
			//Move into attack position
			if(!navAgent.updatePosition)
				navAgent.updatePosition = true;
			navAgent.SetDestination(target.transform.position);
		}
		//Attack!
		else if(globalCD <= 0.0f && atk != -1)
			prepAttack(distance);
	}

	private void prepAttack(float distance)
	{
		//Face target
		if(navAgent.updatePosition)
			navAgent.updatePosition = false;
		navAgent.SetDestination(target.transform.position);
		
		//Aim
		Quaternion targetRotation = Quaternion.LookRotation(
			target.transform.position - transform.position);
		float offset = Quaternion.Angle(transform.rotation, targetRotation);
		if(offset < Mathf.Clamp(16.0f-(distance*1.5f),2,15))
		{
			//Fire
			navAgent.Stop(false);
			atkTimer = atks[atk].getAtkTime();
			attacking = true;
			if(atks[atk].channel != null)
			{
				GameObject t = (GameObject)Instantiate(atks[atk].channel, this.transform.position, this.transform.rotation);
				t.transform.parent = this.transform;
			}
		}
	}
	
	private int selectAttack(float distance)
	{
		int selection = -1;
		for(int i = 0; i < atks.Length; i++)
		{
			if(cooldowns[i] <= 0 && distance < atks[i].attackRange)
			{
				if(selection == -1)
					selection = i;
				else if(selection == -1 || atks[i].attackRange < atks[selection].attackRange)
					selection = i;
			}
		}
		return selection;
	}

	private void attack()
	{
		if(true)
		{
			GameObject temp = (GameObject) Instantiate(
				attackPrefabs[atk], this.transform.position, this.transform.rotation);
			temp.GetComponentInChildren<attackBase>().setSource(this.gameObject);
			cooldowns[atk] = maxCooldowns[atk];
			//temp.transform.parent = this.transform;
		}
		globalCD = globalCDMax;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "pBullet" || other.tag == "pAttack")
		{
			attackBase atk = other.gameObject.GetComponent<attackBase>();
			health -= atk.getDamage();
			Vector3 knockback = this.transform.position - other.transform.position;
			if(other.tag == "pAttack")
				knockback = this.transform.position - target.transform.position;
			knockback.y = 0;
			GetComponent<Rigidbody>().AddForce(knockback.normalized * atk.getForce(), ForceMode.Impulse);
		}
	}
	
	private void die()
	{
		dead = true;
		navAgent.Stop(false);
		if(Random.Range(0,100) < hpDrop)
		{
			Instantiate(healthDrop, transform.position, transform.rotation);
		}
		if(Random.Range(0,100) < tpDrop)
		{
			Instantiate(techDrop, transform.position, transform.rotation);
		}
		for(int i = 0; i < Random.Range(valueMin, valueMax+1); i++)
		{
			Instantiate(coinDrop, transform.position, transform.rotation);
		}
		Destroy (this.gameObject, 1.0f);
	}

	void OnDestroy()
	{
		if (spawner != null)
		{
			spawner.removeMob(spawnID);
		}
	}
}
