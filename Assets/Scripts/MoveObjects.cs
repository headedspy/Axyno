using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : CreateLine {
	
	private static bool isForced = false;

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
		
		
		if(isForced){
			foreach(GameObject obj in selected){
				obj.transform.position += moveWith;
			}
		}else{
			
			
			List<GameObject> points = GetObjects("Point", true);
			List<GameObject> lines = GetObjects("Line", true);
			List<GameObject> angles = GetObjects("Angle", true);
			
			foreach(GameObject point in points){
				point.transform.position += moveWith;
				List<GameObject> connectedLines = point.GetComponent<PointObject>().lines;
				
				foreach(GameObject connectedLine in connectedLines){
					if(connectedLine.GetComponent<LineObject>().point1 == point){
						connectedLine.GetComponent<LineObject>().UpdatePosition(connectedLine.GetComponent<LineObject>().point2.transform.position, point.transform.position);
					}else{
						connectedLine.GetComponent<LineObject>().UpdatePosition(connectedLine.GetComponent<LineObject>().point1.transform.position, point.transform.position);
					}
				}
				
			}
			
			foreach(GameObject line in lines){
				line.transform.position += moveWith;
				line.GetComponent<LineObject>().point1.transform.position += moveWith;
				line.GetComponent<LineObject>().point2.transform.position += moveWith;
				
				
				foreach(GameObject lineOne in line.GetComponent<LineObject>().point1.GetComponent<PointObject>().lines){
					if(lineOne != line){
						GameObject otherPoint = lineOne.GetComponent<LineObject>().point1 == line.GetComponent<LineObject>().point1 ? lineOne.GetComponent<LineObject>().point2 : lineOne.GetComponent<LineObject>().point1;
						lineOne.GetComponent<LineObject>().UpdatePosition(line.GetComponent<LineObject>().point1.transform.position, otherPoint.transform.position);
					}
				}
				
				foreach(GameObject lineTwo in line.GetComponent<LineObject>().point2.GetComponent<PointObject>().lines){
					if(lineTwo != line){
						GameObject otherPoint = lineTwo.GetComponent<LineObject>().point1 == line.GetComponent<LineObject>().point1 ? lineTwo.GetComponent<LineObject>().point2 : lineTwo.GetComponent<LineObject>().point1;
						lineTwo.GetComponent<LineObject>().UpdatePosition(line.GetComponent<LineObject>().point2.transform.position, otherPoint.transform.position);
					}
				}
			}
			
			//To-Do Finish for angles and sometimes lines wont render lmao y
		}
	}
	
	public void Deselect(){
		foreach(GameObject obj in GetObjects("", true)){
			obj.GetComponent<CreatedObject>().SelectClick();
		}
	}
}
