using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationEffect : MonoBehaviour {
	
	public GameObject camera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		gameObject.transform.eulerAngles = new Vector3(-camera.transform.rotation.x*3, -camera.transform.rotation.y*3, 0f);
	}
}
