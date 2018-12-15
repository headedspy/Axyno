using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatedObject : MonoBehaviour {

	public bool isSelected = false;

	public void ChangeColor(Color c){
		gameObject.GetComponent<Renderer>().material.color = c;
	}

	public void AddText(string s){
		gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = s;
	}

	private void Select(){
		isSelected = true;
		gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Outline");
	}

	private void Deselect(){
		isSelected = false;
        gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Legacy Shaders/Transparent/Bumped Diffuse");
    }
	
	public void SelectClick(){
		if(GameObject.Find("Head").GetComponent<Info>().tool == "Select"){
			if(isSelected)Deselect();
			else Select();
		}
	}
}
