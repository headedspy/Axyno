using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePerpendicular : CreateLine {

	public GameObject pointPrefab;

	public override void Initiate(){
		List<GameObject> points = GetObjects("Point", true);
		List<GameObject> lines = GetObjects("Line", true);
		
		if(points.Count != 1 || lines.Count != 1){
			ReportMessage("Select one point and one line", 3);
		}else{
			BuildPerpendicular(points[0], lines[0]);
		}
	}
	
	private void BuildPerpendicular(GameObject point, GameObject line){
		//hm, might switch to vector3 instead? (okay it fuks it for some reason might check why in a l8r iteration)
		float xa = line.GetComponent<LineObject>().point1.transform.position.x;
		float ya = line.GetComponent<LineObject>().point1.transform.position.y;
		float za = line.GetComponent<LineObject>().point1.transform.position.z;
		float xb = line.GetComponent<LineObject>().point2.transform.position.x;
		float yb = line.GetComponent<LineObject>().point2.transform.position.y; 
		float zb = line.GetComponent<LineObject>().point2.transform.position.z;
		float xo = point.transform.position.x;
		float yo = point.transform.position.y;
		float zo = point.transform.position.z;
		
		float l = (xo*xb-xa*xb+xa*xa-xa*xo-ya*yb+yo*yb+ya*ya-ya*yo-za*zb+zo*zb+za*za-za*zo)/
				  (xb*xb-2*xa*xb+xa*xa+yb*yb-2*ya*yb+ya*ya+zb*zb-2*za*zb+za*za);
		
		GameObject newPoint = Instantiate(pointPrefab, new Vector3(xa+l*(xb-xa), ya+l*(yb-ya), za+l*(zb-za)), Quaternion.identity, GetTaskTransform());
		
		newPoint.name = "Point";
		BuildLine(newPoint, point);
		
		GameObject pointOne = line.GetComponent<LineObject>().point1;
		GameObject pointTwo = line.GetComponent<LineObject>().point2;
		line.GetComponent<LineObject>().SelectClick();
		point.GetComponent<PointObject>().SelectClick();
		
		if(IsBetween(pointTwo.transform.position, pointOne.transform.position, newPoint.transform.position)){
			Destroy(line);
			BuildLine(pointOne, newPoint);
			BuildLine(newPoint, pointTwo);
		}else{
			if(Vector3.Distance(point.transform.position, pointOne.transform.position) < Vector3.Distance(point.transform.position, pointTwo.transform.position)){
				BuildLine(newPoint, pointOne);
			}else{
				BuildLine(newPoint, pointTwo);
			}
		}
	}
	
	private bool IsBetween (Vector3 A , Vector3 B , Vector3 C) {
			return Vector3.Dot( (B-A).normalized , (C-B).normalized ) < 0f && Vector3.Dot( (A-B).normalized , (C-A).normalized ) < 0f;
	}
}
