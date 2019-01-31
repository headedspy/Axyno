using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObject : CreatedObject {
	public GameObject point1, point2;
	
	private bool isTransparent = false;
	
	public List<GameObject> connectedAngles;
	
	public void Start(){
		if(connectedAngles == null)connectedAngles = new List<GameObject>();
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
		
		if(connectedAngles.Count > 0){
			foreach(GameObject angle in connectedAngles){
				Destroy(angle);
			}
		}
	}
	
	public override void ChangeColor(Color c){
		GameObject lineMesh = gameObject.transform.GetChild(1).gameObject;
		
		if(isTransparent)lineMesh.GetComponent<Renderer>().material.color = new Color(c.r, c.g, c.b, 0.5f);
		else lineMesh.GetComponent<Renderer>().material.color = c;
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
	
	public void UpdatePosition(Vector3 point1, Vector3 point2){
		
		GameObject lineMesh = gameObject.transform.GetChild(1).gameObject;
		
		float distance = Vector3.Distance(point1, point2);
		
		lineMesh.transform.localScale = new Vector3(0.055639f, 0f, 0.055639f);
		lineMesh.transform.position = point1;
		
		lineMesh.transform.LookAt(point2, lineMesh.transform.up * -1); //why the 2nd part tho?
		lineMesh.transform.Rotate(Vector3.left * 90f, Space.Self);
		lineMesh.transform.Translate(Vector3.down * (distance/2), Space.Self);
		lineMesh.transform.localScale += new Vector3(0f, distance/2, 0f);
		
		foreach(GameObject connectedAngle in connectedAngles){
			connectedAngle.GetComponent<AngleObject>().UpdateAngle(connectedAngle.GetComponent<AngleObject>().line1, connectedAngle.GetComponent<AngleObject>().line2);
		}
		
		Transform lineText = gameObject.transform.GetChild(0);
		
		lineText.gameObject.transform.localScale = new Vector3(-0.067107f, 0.067107f, 0.067107f);
		
		lineText.position = lineMesh.transform.position;
		
	}
	
	public void AddText(string text){
		gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = text;
	}
}
