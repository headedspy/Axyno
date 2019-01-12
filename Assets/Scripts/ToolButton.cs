using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolButton : Tool {

	public string toolName = "";
	
	public bool isSelected = false;
	
	public List<GameObject> otherTools;
	
	public void Start(){
		if(otherTools == null)otherTools = new List<GameObject>();
	}
	
	public override void Initiate(){
		if(!isSelected)Select();
		else{
			if(GameObject.Find("Head").GetComponent<Info>().tool == "Select"){
				foreach(GameObject selectedObject in GetObjects("", true)){
					selectedObject.GetComponent<CreatedObject>().SelectClick();
				}
			}
			Deselect();
		}
	}
	
	private void Select(){
		foreach(GameObject otherTool in otherTools){
			if(otherTool.GetComponent<ToolButton>().isSelected == true){
				otherTool.GetComponent<ToolButton>().Initiate();
			}
		}
		isSelected = true;
		GameObject.Find("Head").GetComponent<Info>().ChangeTool(toolName);
		gameObject.GetComponent<Renderer>().material.color = new Color (0.8f, 0.8f, 0.8f);
	}
	
	private void Deselect(){
		isSelected = false;
		GameObject.Find("Head").GetComponent<Info>().ResetTool();
		gameObject.GetComponent<Renderer>().material.color = new Color (0f, 0f, 0f);
	}
}
