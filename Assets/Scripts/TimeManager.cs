using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	static public void setTimeScale(float scale)
	{
		Time.timeScale = scale;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
}
