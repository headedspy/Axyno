using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolButton : MonoBehaviour {

	public string toolName = "";
	
	public bool isSelected = false;
	
	public void Press(){
		if(!isSelected)Select();
		else Deselect();
	}
	
	private void Select(){
		isSelected = true;
		GameObject.Find("Head").GetComponent<Info>().ChangeTool(toolName);
		gameObject.GetComponent<Renderer> ().material.color = new Color (0.8f, 0.8f, 0.8f);
		Handheld.Vibrate ();
	}
	
	private void Deselect(){
		isSelected = false;
		GameObject.Find("Head").GetComponent<Info>().ResetTool();
		gameObject.GetComponent<Renderer> ().material.color = new Color (0f, 0f, 0f);
	}
}
