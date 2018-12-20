using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : Tool {

	public override void Initiate(){
		Color c = gameObject.GetComponent<Renderer>().material.color;
		
		foreach(GameObject obj in GetObjects("", true)){
			obj.GetComponent<CreatedObject>().ChangeColor(c);
			obj.GetComponent<CreatedObject>().SelectClick();
		}
	}
}
