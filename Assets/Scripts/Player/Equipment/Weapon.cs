using UnityEngine;
using System.Collections;

abstract public class Weapon : MonoBehaviour {
	public static Player player;
	public GameObject attack;
	public GameObject powerAttack;

	protected float reload;
	protected float reloadTimer;
	public float getReloadTimer() { return reloadTimer; }
	public void setReloadTimer(float f) { reloadTimer = f; }

	protected float cooldown;
	protected float cooldownTimer;
	public float getCooldownTimer() { return cooldownTimer; }
	public void setCooldownTimer(float f) { cooldownTimer = f; }

	public int powerCost;
	public float timeFactor;
	public float moveFactor;
	protected float recoverTime;

	public float channelTime;
	public float minSweet;
	public float maxSweet;
	
	protected float timer;
	protected bool isChanneling;
	protected bool isRecovering;
	public bool IsChanneling() { return isChanneling; }
	public bool IsRecovering() { return isRecovering; }
	
	public Texture powerBar;
	public Texture sweetspot;
	public GUISkin skin;
	
	public AudioClip m1Clip;
	public AudioClip m2weakClip;
	public AudioClip m2sweetClip;
	public AudioClip m2wildClip;

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		recoverTime = powerAttack.GetComponentsInChildren<attackBase>(true)[0].recoverTime;
		cooldown = powerAttack.GetComponentsInChildren<attackBase>(true)[0].cooldown;
		reload = attack.GetComponentsInChildren<attackBase>(true)[0].cooldown;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	protected virtual void Update()
	{
		if(cooldownTimer > 0)
			cooldownTimer -= Time.deltaTime;
		if(reloadTimer > 0)
			reloadTimer -= Time.deltaTime;

		if(isChanneling)
		{
			channel();
			if(!Input.GetMouseButton(1) || timer > channelTime)
				usePowerAttack();
		}

		if(isRecovering)
		{
			recover();
			if (timer >=  recoverTime)
				isRecovering = false;
		}
	}

	protected virtual void channel()
	{
		timer += (Time.deltaTime / Time.timeScale);
		Time.timeScale = Mathf.Clamp(1-(timer*4), timeFactor, 1);
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		player.speed = Mathf.Lerp(
			player.baseSpeed,
			player.baseSpeed * moveFactor,
			timer * 4);
	}

	protected virtual void recover()	
	{
		timer += (Time.deltaTime / Time.timeScale);
		Time.timeScale = Mathf.Clamp(timeFactor+(timer*4), timeFactor, 1);
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		player.speed = Mathf.Lerp(
			player.baseSpeed * moveFactor,
			player.baseSpeed,
			timer * 4);
	}

	protected virtual void useBasicAttack()
	{
		reloadTimer = reload;
	}

	protected virtual void usePowerAttack()
	{
		cooldownTimer = cooldown;
		reloadTimer = reload;
		isChanneling = false;
		isRecovering = true;
		timer = 0.0f;
	}

	public virtual bool beginAttack()
	{
		if(reloadTimer > 0)
			return false;
		useBasicAttack();
		return true;
	}

	public virtual bool beginPowerChanneling()
	{
		if(isChanneling || cooldownTimer > 0 || reloadTimer > 0)
			return false;
		timer = 0.0f;
		isChanneling = true;
		return true;
	}

	protected virtual void OnGUI()
	{
		GUI.skin = skin;
		if(isChanneling)
		{
			Vector3 pos = Camera.main.WorldToScreenPoint(this.transform.position);
			GUI.Box(new Rect(pos.x-52, Screen.height - pos.y - 52, 104, 14),"",skin.FindStyle("boundingBox"));
			GUI.Box(new Rect(pos.x-50, Screen.height - pos.y - 50, 100, 10),"");
			GUI.Box(new Rect(pos.x-50+(100*(minSweet)/channelTime),
			                 Screen.height - pos.y - 50, 100*((maxSweet-minSweet)/channelTime), 10)
			        ,"",skin.FindStyle("sweetspot"));
			GUI.Box(new Rect(pos.x-50+(100*((1/channelTime)*timer)),
			                 Screen.height - pos.y - 50, 3, 10)
			        ,"",skin.FindStyle("sweetspot"));
		}
	}
}
