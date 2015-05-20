using UnityEngine;

using System.Collections;



public class wave : MonoBehaviour	
{
	public int segments;
	public float startRadius;
	private float radius;
	public float endRadius;
	public float duration;
	public float fade;
	private float elapsed;
	private float alpha;
	LineRenderer line;
	SphereCollider col;

	public int r;
	public int g;
	public int b;

	void Start ()	
	{
		line = gameObject.GetComponent<LineRenderer>();
		col = gameObject.GetComponent<SphereCollider>();

		line.SetVertexCount (segments + 1);
		line.useWorldSpace = false;

		elapsed = 0.0f;
		alpha = 1;
		radius = startRadius;
		CreatePoints ();
	}

	void Update()
	{
		elapsed += Time.deltaTime;
		radius = startRadius + ((elapsed / duration)*endRadius-startRadius);
		if(elapsed <= duration)
			col.radius = radius;
		else
		{
			if(this.gameObject.GetComponent<Collider>() != null)
				Destroy(this.gameObject.GetComponent<Collider>());
			alpha -= Time.deltaTime*(1/fade);
			Color color = new Color(r/255.0f,g/255.0f,b/255.0f,alpha);
			line.SetColors(color,color);
		}
		if(elapsed > duration + fade)
			Destroy (this.gameObject);
		CreatePoints();
	}
	
	void CreatePoints ()
	{
		float x;
		float y = 0.5f;
		float z;
		
		float angle = 0f;
		
		for (int i = 0; i < (segments + 1); i++)

		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle);
			z = Mathf.Cos (Mathf.Deg2Rad * angle);
			line.SetPosition (i,new Vector3(x,y,z) * radius);
			angle += (360f / segments);
		}	
	}	
}