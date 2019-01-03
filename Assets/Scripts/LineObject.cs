using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObject : CreatedObject {
	public GameObject point1, point2;
	
	private bool isTransparent = false;
	
	private List<GameObject> connectedAngles;
	
	public void Start(){
		connectedAngles = new List<GameObject>();
	}
	
	public void SetPoints(GameObject p1, GameObject p2){
		this.point1 = p1;
		this.point2 = p2;
		
		p1.GetComponent<PointObject>().Connect(gameObject);
		p2.GetComponent<PointObject>().Connect(gameObject);
	}
	
	public float GetLength(){
		return Vector3.Distance(point1.transform.position, point2.transform.position);
	}
	
	public void OnDestroy(){
		point1.GetComponent<PointObject>().Disconnect(gameObject);
		point2.GetComponent<PointObject>().Disconnect(gameObject);

		if(point1.GetComponent<PointObject>().lines.Count == 0)Destroy(point1);
		if(point2.GetComponent<PointObject>().lines.Count == 0)Destroy(point2);
		
		foreach(GameObject angle in connectedAngles){
			Destroy(angle);
		}
	}
	
	public override void ChangeColor(Color c){
		if(isTransparent)gameObject.GetComponent<Renderer>().material.color = new Color(c.r, c.g, c.b, 0.5f);
		else gameObject.GetComponent<Renderer>().material.color = c;
	}
	
	public void SetTransparency(bool state){
		isTransparent = state;
	}
	
	public void ConnectAngle(GameObject angle){
		connectedAngles.Add(angle);
	}
	
	public void DisconnectAngle(GameObject angle){
		connectedAngles.Remove(angle);
	}
}
