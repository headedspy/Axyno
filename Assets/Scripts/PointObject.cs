using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : CreatedObject {

	public List<GameObject> lines;
	
	public void Start(){
		if(lines == null)lines = new List<GameObject>();
	}

	public bool ConnectedTo(GameObject givenLine){
		foreach(GameObject line in lines){
			if(givenLine == line){
				return true;
			}
		}
		return false;
	}
	
	public void Connect(GameObject givenLine){
		lines.Add(givenLine);
	}
	
	public void Disconnect(GameObject givenLine){
		lines.Remove(givenLine);
	}
	
	public void AddText(string s){
		gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = s;
	}
	
	public string GetText(){
		return gameObject.transform.GetChild(0).GetComponent<TextMesh>().text;
	}
}
