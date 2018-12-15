using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateButton : MonoBehaviour {

	public GameObject task;

	public bool isReset = false;

	public string position = "";

	private bool isDown = false;

	private Vector3 rotateAround;

	public void ButtonDown(){
		isDown = true;
	}

	public void ButtonUp(){
		isDown = false;
	}

	void Start(){
		switch (position){
		case "U":
			rotateAround = Vector3.right;
			break;

		case "L":
			rotateAround = Vector3.up;
			break;

		case "R":
			rotateAround = Vector3.down;
			break;

		case "D":
			rotateAround = Vector3.left;
			break;
		}
	}

	void Update(){
		if (isDown) {
			if (isReset)
				task.transform.rotation = Quaternion.AngleAxis (0, rotateAround);
			else
				task.transform.Rotate (rotateAround * 90f * Time.deltaTime, Space.World);
		}
	}
}
