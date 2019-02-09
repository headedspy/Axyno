using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleObject : MonoBehaviour {

	private List<GameObject> lines = null;
	
	private GameObject centerPoint;
	private GameObject radiusLine;
	private Quaternion angleRotation;
	
	public void SetObjects(GameObject center, GameObject radius, GameObject angle){
		centerPoint = center;
		radiusLine = radius;
		angleRotation = angle.transform.rotation;
	}
	
	public void AddLine(GameObject line){
		if(lines == null)lines = new List<GameObject>();
		lines.Add(line);
	}
	
	public void OnDestroy(){
		foreach(GameObject line in lines){
			Destroy(line);
		}
	}
	
	public bool Check(GameObject center, GameObject radius, GameObject angle){
		return center == centerPoint && radius == radiusLine;// && angle.transform.rotation == angleRotation;
		Debug.Log(angle);
		Debug.Log(angleRotation);
	}
}
