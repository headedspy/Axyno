using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//change name of the class
public class ExpandLine : CreateLine {
	
	public GameObject pointPrefab;

	public override void Initiate(){
		List<GameObject> lines = GetObjects("Line", true);
		
		if(lines.Count != 1){
			ReportMessage("ERROR: Select one line", 3);
		}else{
			GameObject line = lines[0];
			
			//
			line.GetComponent<Collider>().enabled = false;
			
			GameObject A = line.GetComponent<LineObject>().point1;
			GameObject B = line.GetComponent<LineObject>().point2;
			
			float length = line.GetComponent<LineObject>().GetLength();
			float offset = 8f;
			
			GameObject endPointOne = Instantiate(pointPrefab, new Vector3(Calculate(B.transform.position.x, A.transform.position.x, length, offset), 
																		  Calculate(B.transform.position.y, A.transform.position.y, length, offset), 
																		  Calculate(B.transform.position.z, A.transform.position.z, length, offset)), 
																		  Quaternion.identity);
			
			GameObject endPointTwo = Instantiate(pointPrefab, new Vector3(Calculate(B.transform.position.x, A.transform.position.x, length, -offset), 
																		  Calculate(B.transform.position.y, A.transform.position.y, length, -offset), 
																		  Calculate(B.transform.position.z, A.transform.position.z, length, -offset)), 
																		  Quaternion.identity);
			
			GameObject newLine = BuildLine(endPointOne, endPointTwo);
			
			Destroy(newLine.GetComponent<LineObject>());
			
			newLine.AddComponent<LineHover>();
			newLine.GetComponent<LineHover>().pointPrefab = pointPrefab;
			newLine.GetComponent<LineHover>().linePrefab = linePrefab;
			newLine.GetComponent<LineHover>().SetObjects(GetTaskTransform(), line);
			
			newLine.GetComponent<Renderer>().material.color = Color.red;
			newLine.transform.localScale -= new Vector3(0.05f, 0f, 0.05f);
			
			Destroy(endPointOne);
			Destroy(endPointTwo);
			
			line.GetComponent<CreatedObject>().SelectClick();
		}
	}
	
	private float Calculate(float pointOne, float pointTwo, float length, float lambda){
		return pointOne + (pointOne - pointTwo) / length * lambda;
	}
}
