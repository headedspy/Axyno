using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleObject : MonoBehaviour {

	private List<GameObject> lines = null;
	
	private GameObject centerPoint;
	private GameObject angleRotation;
	private Vector3 rotation;
	private float radiusLength;
	private float delta = 1f;
	
	public void SetObjects(GameObject center, GameObject radius, GameObject angle){
		centerPoint = center;
		angleRotation = angle;
		rotation = angle.transform.eulerAngles;
		radiusLength = radius.GetComponent<LineObject>().GetLength();
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
		return center == centerPoint && 
		radius.GetComponent<LineObject>().GetLength() < radiusLength + delta && 
		radius.GetComponent<LineObject>().GetLength() > radiusLength - delta &&
		(angle == angleRotation || rotation == angle.transform.eulerAngles);
	}
}
