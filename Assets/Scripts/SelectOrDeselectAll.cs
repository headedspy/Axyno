using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOrDeselectAll : Tool {

	public bool isDeselect = false;
	
	public override void Initiate(){
		if(isDeselect){
			foreach(GameObject obj in GetObjects("", true)){
				obj.GetComponent<CreatedObject>().SelectClick();
			}
		}else{
			foreach(GameObject obj in GetObjects("", false)){
				if(!obj.GetComponent<CreatedObject>().isSelected){
					obj.GetComponent<CreatedObject>().SelectClick();
				}
			}
		}
	}
}
