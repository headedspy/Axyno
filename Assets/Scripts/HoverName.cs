using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverName : MonoBehaviour {
	
	public string toolName;
	public GameObject textObject;
	
	public void Update(){
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit))
			if(hit.transform.gameObject == gameObject)
				textObject.GetComponent<TextMesh>().text = toolName;
        else
			if(textObject.GetComponent<TextMesh>().text == toolName)
				textObject.GetComponent<TextMesh>().text = "";
	}
}
