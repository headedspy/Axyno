using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : CreatedObject {

	public List<GameObject> lines;
	
	private void Start(){
		lines = new List<GameObject>();
	}

	public bool ConnectedTo(GameObject givenLine){
		foreach(GameObject line in lines){
			if(givenLine.GetInstanceID() == line.GetInstanceID()){
				return true;
			}
		}
		return false;
	}
	
	public void Connect(GameObject givenLine){
		lines.Add(givenLine);
	}
}
