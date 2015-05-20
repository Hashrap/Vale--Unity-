using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public Camera cam;

	public float speed;
	public float baseSpeed;
	public float turnSpeed;
	
	public int lives;
	private float hp;
	public int maxHP;
	public float hpRegen;
	private float tp;
	public void useTP(float amt) { tp -= amt; }
	public float getTP() { return tp; }
	public int maxTP;
	public float tpRegen;
	private bool dead;

	private int dough;
	public float pickupRange;

	private int weapon;
	public GameObject weapon1;
	public GameObject weapon2;
	private Weapon[] weapons;

	public AudioClip noMana;
	public AudioClip globalCooldown;
	public AudioClip cooldown;
	
	public AudioClip coinClip;
	public AudioClip healthClip;
	public AudioClip techClip;

	public AudioClip damageClip;
	public AudioClip deathClip;

	// Use this for initialization
	void Awake () {
		hp = maxHP;
		tp = maxTP;
		dead = false;
		baseSpeed = speed;
		weapons = new Weapon[2];
		weapon = 0;
		weapon1 = (GameObject)Instantiate (weapon1, this.transform.position, this.transform.rotation);
		weapon2 = (GameObject)Instantiate (weapon2, this.transform.position, this.transform.rotation);
		weapon1.transform.parent = this.transform;
		weapon2.transform.parent = this.transform;
	}

	void Start()
	{
		weapons[0] = weapon1.GetComponent<Weapon>();
		weapons[1] = weapon2.GetComponent<Weapon>();
	}

	void Update()
	{
		if (dead) { return; }
		if (hp <= 0) { dead = true; return; }
		else
			hp = Mathf.Clamp(hp + (hpRegen * Time.deltaTime),0,100);

		move();
		look();

		if(!weapons[weapon].IsChanneling() && Input.GetKeyDown(KeyCode.Q))
		{
			if(weapon == 0)
				weapon = 1;
			else
				weapon = 0;
		}

		else if(Input.GetMouseButton(0))
			weapons[weapon].beginAttack();	
		
		else if(Input.GetMouseButton(1))
		{
			if (tp < weapons[weapon].powerCost)
				return;
				//audio.PlayOneShot(noMana);
			if (weapons[weapon].beginPowerChanneling())
				tp -= weapons[weapon].powerCost;
			else
				return;
				//audio.PlayOneShot(cooldown);
		}
	}

	void move()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		
		Vector3 vec = new Vector3(h,0.0f,v);

		GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position+(speed * vec.normalized * Time.deltaTime));
	}

	void look()
	{
		//face the mouse cursor
		Plane playerPlane = new Plane(Vector3.up, transform.position);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float hitdist;

		if(playerPlane.Raycast(ray, out hitdist))
		{
			Vector3 targetPoint = ray.GetPoint(hitdist);
			Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed*Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "health")
		{
			GetComponent<AudioSource>().PlayOneShot(healthClip);
			hp += maxHP / 5;
			if(hp > maxHP){ hp = maxHP; }
			Destroy(other.gameObject);
		}
		else if (other.tag == "coin")
		{
			GetComponent<AudioSource>().PlayOneShot(coinClip);
			dough++;
			Destroy (other.gameObject);
		}
		else if (other.tag == "tp")
		{
			GetComponent<AudioSource>().PlayOneShot(techClip);
			tp += maxTP / 3;
			if(tp > maxTP){ tp = maxTP; }
			Destroy(other.gameObject);
		}
		else if (other.tag == "eAttack" || other.tag == "eBullet")
		{
			attackBase atk = other.gameObject.GetComponent<attackBase>();
			hp -= atk.getDamage();
			if(hp <= 0)
				GetComponent<AudioSource>().PlayOneShot(deathClip);
			else
			{
				GetComponent<AudioSource>().PlayOneShot(damageClip);
				Vector3 knockback = this.transform.position - atk.getSource().transform.position;
				knockback.y = 0;
				GetComponent<Rigidbody>().AddForce(knockback.normalized * atk.getForce(), ForceMode.Impulse);
			}
		}
	}

	void OnGUI()
	{
		GUI.Label(new Rect(5,10,50,20), hp+"/"+maxHP);
		GUI.Label(new Rect(5,40,50,20), tp+"/"+maxTP);
	}
}
