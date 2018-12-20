using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour {

	public GameObject menu;

	private bool isOn = false;

	void Start(){
		menu.SetActive (false);
	}

	public void PressDown(){
		menu.SetActive (!isOn);
		isOn = !isOn;
		gameObject.GetComponent<Renderer> ().material.color = new Color (0.5f, 0.5f, 0.5f);
	}

	public void PressUp(){
		gameObject.GetComponent<Renderer> ().material.color = new Color (0f, 0f, 0f);
	}
}
