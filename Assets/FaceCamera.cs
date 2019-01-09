using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

	void Update () {
		gameObject.transform.LookAt(Camera.main.transform);
	}
}
