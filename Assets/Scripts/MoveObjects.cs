using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : Tool {

	public override void Initiate(){
		List<GameObject> selected = GetObjects("", true);
		float movement = 0.1f;
		Vector3 moveWith;
		
		if(gameObject.name[0] == '-')movement *= -1;
		
		if(gameObject.name[1] == 'X'){
			moveWith = new Vector3(movement, 0f, 0f);
		}else if(gameObject.name[1] == 'Y'){
			moveWith = new Vector3(0f, movement, 0f);
		}else{
			moveWith = new Vector3(0f, 0f, movement);
		}
		
		
		foreach(GameObject obj in selected){
			obj.transform.position += moveWith;
		}
	}
	
	public void Deselect(){
		foreach(GameObject obj in GetObjects("", true)){
			obj.GetComponent<CreatedObject>().SelectClick();
		}
	}
}
