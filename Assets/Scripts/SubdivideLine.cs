using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubdivideLine : CreateLine {
	
	public GameObject textObject;
	public GameObject pointPrefab;
	
	private static int numberOfDivisions = 2;

	public override void Initiate(){
		foreach(GameObject line in GetObjects("Line", true)){

			float lineLength = Vector3.Distance(line.GetComponent<LineObject>().point1.transform.position, line.GetComponent<LineObject>().point2.transform.position);
			float subDistance = lineLength / numberOfDivisions;

			List<GameObject> createdPoints = new List<GameObject>();

			createdPoints.Add(line.GetComponent<LineObject>().point2);

			for(int i=1; i<numberOfDivisions; i++){
				GameObject tempPoint = Instantiate(pointPrefab, line.transform.position, line.transform.rotation, GetTaskTransform());
				tempPoint.transform.Translate(Vector3.down * lineLength / 2, Space.Self);
				tempPoint.name = "Point";
				createdPoints.Add(tempPoint);
					
				tempPoint.transform.Translate(Vector3.up * subDistance * i, Space.Self);
			}

			createdPoints.Add(line.GetComponent<LineObject>().point1);
			
			Material mat = line.GetComponent<Renderer>().material;
			
			line.GetComponent<CreatedObject>().SelectClick();

			for(int i=0; i<numberOfDivisions; i++){
				GameObject newLine = BuildLine(createdPoints[i], createdPoints[i+1]);
				newLine.GetComponent<Renderer>().material = mat;
			}
			Destroy(line);
		}
	}
	
	public void ChangeCount(int num){
		int newCount = numberOfDivisions + num;
		
		if(newCount < 2)newCount = 2;
		else if(newCount > 10)newCount = 10;
		
		numberOfDivisions = newCount;
		
		textObject.GetComponent<TextMesh>().text = newCount.ToString();
	}
}
