using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeFace : CreateLine {
	
	public GameObject pointPrefab;
	public GameObject menu;
	
	private static GameObject cPoint;
	
	private static Vector3 newPos;
	
	public override void Initiate(){
		List<GameObject> lines = GetObjects("Line", true);
		
		if(lines.Count <= 2){
			ReportMessage("ERROR: Select at lest 3 lines", 3);
			return;
		}
		
		if(!HasLoop(lines)){
			ReportMessage("ERROR: Select lines that form a loop", 3);
			return;
		}
		
		//To-do re-code this shit maybe
		Vector3 testPos = new Vector3(0f, 0f, 0f);
		foreach(GameObject line in lines){
			testPos += line.GetComponent<LineObject>().point1.transform.position;
			testPos += line.GetComponent<LineObject>().point2.transform.position;
		}
		testPos /= ((float)lines.Count * 2f);
		
		cPoint = Instantiate(pointPrefab, testPos, Quaternion.identity, GetTaskTransform());
		
		
		menu.SetActive(true);
		newPos = Vector3.zero;
	}
	
	private bool HasLoop(List<GameObject> lines){
		List<GameObject> points = GetPoints(lines, true);
		int count = lines.Count;
		
			
		while(points.Count != 0){
			GameObject topPoint = points[0];
			points.Remove(topPoint);
				
			if(points.Contains(topPoint)){
				points.Remove(topPoint);
			}else{
				return false;
			}
		}
		return true;
	}
	
	public void Confirm(bool isExtrude){
		menu.SetActive(false);
		
		Dictionary<GameObject, GameObject> ortho = new Dictionary<GameObject, GameObject>();
		
		
		List<GameObject> points = GetPoints(GetObjects("Line", true), false);
		
		if(isExtrude){
			foreach(GameObject point in points){
				GameObject extrudedPoint = Instantiate(pointPrefab, new Vector3(point.transform.position.x + newPos.x,
													 point.transform.position.y + newPos.y,
													 point.transform.position.z + newPos.z), Quaternion.identity, GetTaskTransform());
													 
				extrudedPoint.name = "Point";
				BuildLine(extrudedPoint, point);
				
				ortho.Add(point, extrudedPoint);
			}
			
			List<GameObject> lines = GetObjects("Line", true);
			
			foreach(GameObject line in lines){
				BuildLine(ortho[line.GetComponent<LineObject>().point1],
						  ortho[line.GetComponent<LineObject>().point2]);
			}
			
			Destroy(cPoint);
		}else{
			foreach(GameObject point in points){
				BuildLine(cPoint, point);
			}
		}
		
		foreach(GameObject line in GetObjects("Line", true)){
			line.GetComponent<CreatedObject>().SelectClick();
		}
	}
	
	private List<GameObject> GetPoints(List<GameObject> lines, bool addTwice){
		List<GameObject> points = new List<GameObject>();
		
		foreach(GameObject line in lines){
			GameObject p1 = line.GetComponent<LineObject>().point1;
			GameObject p2 = line.GetComponent<LineObject>().point2;
			
			if(!points.Contains(p1) || addTwice)points.Add(p1);
			if(!points.Contains(p2) || addTwice)points.Add(p2);
		}
		
		return points;
	}
	
	public void ChangePos(string axis){
		float amount = 0.1f;
		
		if(axis[0] == '-')amount *= -1;
		
		if(axis[1] == 'X'){
			cPoint.transform.position += new Vector3(amount, 0f, 0f);
			newPos.x += amount;
		}else if(axis[1] == 'Y'){
			cPoint.transform.position += new Vector3(0f, amount, 0f);
			newPos.y += amount;
		}else if(axis[1] == 'Z'){
			cPoint.transform.position += new Vector3(0f, 0f, amount);
			newPos.z += amount;
		}
	}
	
}
