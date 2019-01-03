using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : Tool {

	public override void Initiate(){
		foreach(GameObject obj in GetObjects("", true)){
			if(obj.name == "Point"){
				foreach(GameObject line in obj.GetComponent<PointObject>().lines){
					Destroy(line);
				}
				Destroy(obj);
			}else{				
				Destroy(obj);
			}
		}
	}
}
