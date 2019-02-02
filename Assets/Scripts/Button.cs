using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public GameObject menu;

	private bool isOn = false;
	
	public bool isTool = false;
	
	public static bool isMenuOpen = false;

	public void PressDown(){
		if(!isTool){
			StartCoroutine(PlayAnimation(!isOn));
			isOn = !isOn;
			gameObject.GetComponent<Renderer> ().material.color = new Color (0.5f, 0.5f, 0.5f);
		}else{
			if(!isMenuOpen){
				isOn = true;
				isMenuOpen = true;
				StartCoroutine(PlayAnimation(true));
			}else{
				if(isOn){
					isOn = false;
					isMenuOpen = false;
					StartCoroutine(PlayAnimation(false));
				}
			}
		}
	}

	public void PressUp(){
		if(isOn && isTool && isMenuOpen){
			gameObject.GetComponent<Renderer> ().material.color = new Color (0.5f, 0.5f, 0.5f);
		}else{
			gameObject.GetComponent<Renderer> ().material.color = new Color (0f, 0f, 0f);
		}
	}
	
	private IEnumerator PlayAnimation(bool isOpen){
		float scale = 1f;
		if(isTool)scale = 3f;
		
		if(isOpen){
			menu.SetActive(isOpen);
			for(float i=0f; i<scale; i+=0.15f){
				menu.transform.localScale = new Vector3(menu.transform.localScale.x, i, menu.transform.localScale.z);
				yield return new WaitForSeconds(0.01f);
			}
		}else{
			for(float i=scale; i>0f; i-=0.15f){
				menu.transform.localScale = new Vector3(menu.transform.localScale.x, i, menu.transform.localScale.z);
				yield return new WaitForSeconds(0.01f);
			}
			menu.SetActive(isOpen);
		}
	}
}
