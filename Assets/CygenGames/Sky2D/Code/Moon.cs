using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour {

	public Sky Sky;
	public float timeOfDay;
	// Use this for initialization
	void Start () {
		Color color = GetComponent<Renderer>().material.color;
		color.a = 0f;
		GetComponent<Renderer>().material.color = color;

	}
	
	// Update is called once per frame
	void Update () {
	
		 timeOfDay = Sky.TimeOfDay;

		if (timeOfDay < 6 || timeOfDay > 18)
		{
			Color color = GetComponent<Renderer>().material.color;
			if (color.a < 1f) 
			{
				color.a += 0.01f;
			}
			GetComponent<Renderer>().material.color = color;
		}
		else
		{
			Color color = GetComponent<Renderer>().material.color;
			if (color.a > 0f) 
			{
				color.a -= 0.01f;
			}
			GetComponent<Renderer>().material.color = color;
		}
	}
}
