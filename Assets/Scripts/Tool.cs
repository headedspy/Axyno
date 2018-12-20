﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour {

	public abstract void Initiate();
	
	protected List<GameObject> GetObjects(string name, bool selected){
		List<GameObject> list = new List<GameObject>();
		
		Transform task = GetTaskTransform();
		
		foreach(Transform child in task){
			if(child.gameObject.name == name || name == ""){
				if(selected){
					if(child.gameObject.GetComponent<CreatedObject>().isSelected){
						list.Add(child.gameObject);
					}
				}else{
					list.Add(child.gameObject);
				}
			}
		}
		return list;
	}
	
	//To-do
	protected void ReportMessage(string message, int type){ //1-info 2-warning 3-error
		Debug.Log(message);
	}
	
	protected Transform GetTaskTransform(){
		return GameObject.Find("Task").transform;
	}
	
	protected void Vibrate(){
		Handheld.Vibrate ();
	}
	
}