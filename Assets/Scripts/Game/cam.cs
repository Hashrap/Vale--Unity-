using UnityEngine;
using System.Collections;

public class cam : MonoBehaviour {
	private GameObject target;

	public float xOffset;
	public float yOffset;
	public float zOffset;

	public float maxRange;

	private float halfScreenX;
	private float halfScreenZ;

	// Use this for initialization
	void Start () {
		target = GameObject.FindWithTag("Player");

		halfScreenX = Screen.width/2;
		halfScreenZ = Screen.height/2;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 pPos = target.transform.position;
		pPos += new Vector3(xOffset, yOffset, zOffset);
		transform.position = pPos;
		transform.LookAt(target.transform);

		float mX = Input.mousePosition.x-halfScreenX;
		float mZ = Input.mousePosition.y-halfScreenZ;

		float offsetX = mX/halfScreenX * maxRange;
		float offsetZ = mZ/halfScreenZ * maxRange;

		transform.Translate(offsetX, 0.0f, offsetZ, Space.World);
	}

	void OnGUI()
	{

	}
}
