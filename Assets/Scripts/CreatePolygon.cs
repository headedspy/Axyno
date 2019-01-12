using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePolygon : CreateLine {
	private static int sidesCount = 3;
	public GameObject number;
	public GameObject pointPrefab;

	public override void Initiate(){
		Vector3 pos = new Vector3(0.5f, 1f, 0.5f);
		Vector3 offset = new Vector3(0f, 0f, 1f);
		float side = 0.5f;
		
		float angleSize = 360f / sidesCount;
		
		GameObject point = Instantiate(pointPrefab, pos + offset, Quaternion.identity, GetTaskTransform());
		point.name = "Point";
		GameObject firstPoint = point;
		
		for(int i=0; i<sidesCount-1; i++){
			pos = Quaternion.Euler(0, -angleSize, 0) * pos;
			
			GameObject newPoint = Instantiate(pointPrefab, pos + offset, Quaternion.identity, GetTaskTransform());
			newPoint.name = "Point";
			
			BuildLine(point, newPoint);
			
			point = newPoint;
		}
		
		BuildLine(firstPoint, point);
	}
	
	public void ChangeNumberOfSides(int amount){
		sidesCount += amount;
		if(sidesCount < 3)sidesCount = 3;
		if(sidesCount > 12)sidesCount = 12;
		
		number.GetComponent<TextMesh>().text = sidesCount.ToString();
	}
}
