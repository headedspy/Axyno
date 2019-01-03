using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandLine : CreateLine {
	
	public GameObject pointPrefab;

	public override void Initiate(){
		List<GameObject> lines = GetObjects("Line", true);
		
		if(lines.Count != 1){
			ReportMessage("ERROR: Select one line", 3);
		}else{
			GameObject line = lines[0];
			
			GameObject A = line.GetComponent<LineObject>().point1;
			GameObject B = line.GetComponent<LineObject>().point2;
			
			float length = line.GetComponent<LineObject>().GetLength();
			
			float lambda = 8f;
			
			GameObject endPointOne = Instantiate(pointPrefab, new Vector3(Calculate(B.transform.position.x, A.transform.position.x, length, lambda), Calculate(B.transform.position.y, A.transform.position.y, length, lambda), Calculate(B.transform.position.z, A.transform.position.z, length, lambda)), Quaternion.identity);
			GameObject endPointTwo = Instantiate(pointPrefab, new Vector3(Calculate(B.transform.position.x, A.transform.position.x, length, -8f), Calculate(B.transform.position.y, A.transform.position.y, length, -8f), Calculate(B.transform.position.z, A.transform.position.z, length, -8f)), Quaternion.identity);
		
			GameObject newLine = BuildLine(endPointOne, endPointTwo);
			
			line.GetComponent<CreatedObject>().SelectClick();
			
			Destroy(newLine.GetComponent<LineObject>());
			
			newLine.AddComponent<LineHover>();
			
			newLine.GetComponent<LineHover>().SetObjects(GetTaskTransform(), line);
			
			newLine.transform.localScale -= new Vector3(0.08f, 0f, 0.08f);
			
			newLine.GetComponent<LineHover>().pointPrefab = pointPrefab;
			newLine.GetComponent<LineHover>().linePrefab = linePrefab;
			
			Destroy(endPointOne);
			Destroy(endPointTwo);
			
			newLine.GetComponent<Renderer>().material.color = Color.red;
		}
	}
	
	private float Calculate(float pointOne, float pointTwo, float length, float lambda){
		return pointOne + (pointOne - pointTwo) / length * lambda;
	}
}
