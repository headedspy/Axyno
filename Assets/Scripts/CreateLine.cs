using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLine : Tool {

	public GameObject linePrefab;

	public override void Initiate(){
		List<GameObject> points = GetObjects("Point", true);
		
		if(points.Count != 2){
			ReportMessage("2 points must be selected", 3);
		}else{
			BuildLine(points[0], points[1]);
			
			foreach(GameObject point in points){
				point.GetComponent<PointObject>().SelectClick();
			}
		}
	}
	
	protected GameObject BuildLine(GameObject point1, GameObject point2){
		foreach(GameObject connectedLine in point1.GetComponent<PointObject>().lines){
			if(connectedLine.GetComponent<LineObject>().point1 == point2 || connectedLine.GetComponent<LineObject>().point2 == point2){
				ReportMessage("Line already exists", 3);
				return null;
			}
		}
		
		float distance = Vector3.Distance(point1.transform.position, point2.transform.position);
		GameObject line = Instantiate(linePrefab, point1.transform.position, Quaternion.identity, GetTaskTransform());
		
		line.transform.LookAt(point2.transform, line.transform.up * -1); //why the 2nd part tho?
		line.transform.Rotate(Vector3.left * 90f, Space.Self);
		line.transform.Translate(Vector3.down * (distance/2), Space.Self);
		line.transform.localScale += new Vector3(0f, distance/2, 0f);
		line.name = "Line";
		
		line.GetComponent<LineObject>().SetPoints(point1, point2);
		
		//Unity2018 bug *shrug face*
		line.GetComponent<Collider>().enabled = false;
		line.GetComponent<Collider>().enabled = true;
		//dont ask
		
		return line;
	}
}
