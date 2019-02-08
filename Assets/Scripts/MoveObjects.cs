using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : CreateLine {
	
	private static bool isForced = false;
	public static char axis = 'X';
	public bool isRotation = false;
	
	public override void Initiate(){
		List<GameObject> selected = GetObjects("", true);
		float movement = 0.1f;
		Vector3 moveWith = Vector3.zero;
		Vector3 rotateWith = Vector3.zero;
		Vector3 midPoint = Vector3.zero;
		
		if(isRotation){
			if(axis == 'X'){
				rotateWith = new Vector3(5f, 0f, 0f);
			}else if(axis == 'Y'){
				rotateWith = new Vector3(0f, 5f, 0f);
			}else if(axis == 'Z'){
				rotateWith = new Vector3(0f, 0f, 5f);
			}
			
			if(gameObject.name == "Minus") rotateWith *= -1;
			
		}else{
			if(gameObject.name[0] == '-')movement *= -1;
		
			if(gameObject.name[1] == 'X'){
				moveWith = new Vector3(movement, 0f, 0f);
			}else if(gameObject.name[1] == 'Y'){
				moveWith = new Vector3(0f, movement, 0f);
			}else{
				moveWith = new Vector3(0f, 0f, movement);
			}
		}
		
		
		if(isForced){
			foreach(GameObject obj in selected){
				if(isRotation)obj.transform.Rotate(rotateWith, Space.World);
				else obj.transform.position += moveWith;
			}
		}else{
			List<GameObject> points = GetObjects("Point", true);
			List<GameObject> lines = GetObjects("Line", true);
			List<GameObject> angles = GetObjects("Angle", true);
			
			foreach(GameObject angle in angles){
				GameObject line1 = angle.GetComponent<AngleObject>().line1;
				GameObject line2 = angle.GetComponent<AngleObject>().line2;
				
				if(!lines.Contains(line1))lines.Add(line1);
				if(!lines.Contains(line2))lines.Add(line2);
			}
			
			foreach(GameObject line in lines){
				GameObject point1 = line.GetComponent<LineObject>().point1;
				GameObject point2 = line.GetComponent<LineObject>().point2;
				
				if(!points.Contains(point1))points.Add(point1);
				if(!points.Contains(point2))points.Add(point2);
			}
			
			
			if(!isRotation){
				foreach(GameObject point in points){
					point.transform.position += moveWith;
					
					foreach(GameObject line in point.GetComponent<PointObject>().lines){
						GameObject otherPoint = line.GetComponent<LineObject>().point1 == point ? line.GetComponent<LineObject>().point2 : line.GetComponent<LineObject>().point1;
						
						line.GetComponent<LineObject>().UpdatePosition(point.transform.position, otherPoint.transform.position);
					}
				}
				foreach(GameObject line in lines){
					GameObject point1 = line.GetComponent<LineObject>().point1;
					GameObject point2 = line.GetComponent<LineObject>().point2;
					
					line.GetComponent<LineObject>().UpdatePosition(point1.transform.position, point2.transform.position);
				}
				
				foreach(GameObject angle in angles){
					GameObject line1 = angle.GetComponent<AngleObject>().line1;
					GameObject line2 = angle.GetComponent<AngleObject>().line2;
					
					angle.GetComponent<AngleObject>().UpdateAngle(line1, line2);
				}

			// movement
			}else{
				foreach(GameObject point in points){
					midPoint += point.transform.position;
				}
				midPoint /= points.Count;
				
				foreach(GameObject point in points){
					point.transform.RotateAround(midPoint, rotateWith, 5f);
				}
				
				foreach(GameObject line in lines){
					GameObject point1 = line.GetComponent<LineObject>().point1;
					GameObject point2 = line.GetComponent<LineObject>().point2;
					
					foreach(GameObject connectedLine in point1.GetComponent<PointObject>().lines){
						GameObject connectedPoint1 = connectedLine.GetComponent<LineObject>().point1;
						GameObject connectedPoint2 = connectedLine.GetComponent<LineObject>().point2;
						
						connectedLine.GetComponent<LineObject>().UpdatePosition(connectedPoint1.transform.position, connectedPoint2.transform.position);
					}
					foreach(GameObject connectedLine in point2.GetComponent<PointObject>().lines){
						GameObject connectedPoint1 = connectedLine.GetComponent<LineObject>().point1;
						GameObject connectedPoint2 = connectedLine.GetComponent<LineObject>().point2;
						
						connectedLine.GetComponent<LineObject>().UpdatePosition(connectedPoint1.transform.position, connectedPoint2.transform.position);
					}
				}
			}
		}
	}
	
	public void Deselect(){
		foreach(GameObject obj in GetObjects("", true)){
			obj.GetComponent<CreatedObject>().SelectClick();
		}
	}
	
	public void ChangeMethod(){
		isForced = !isForced;
	}
	
	public void ChangeAxis(){
		axis = gameObject.name[0];
	}
}
