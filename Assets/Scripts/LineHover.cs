using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHover : ExpandLine {
	private Transform task;
	private GameObject line;
	
	private GameObject newPoint = null;
	
	
	public void SetObjects(Transform taskTransform, GameObject newLine){
		task = taskTransform;
		line = newLine;
	}
	
	public void Update(){
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit)){
			if(hit.collider.gameObject == gameObject){
				Destroy(newPoint);
				newPoint = Instantiate(pointPrefab, hit.point, Quaternion.identity, task);
				newPoint.name = "Point";
			}
		}
		
		if(Input.GetMouseButtonDown(0) && newPoint != null) ExtendToPoint();
	}
	
	private void ExtendToPoint(){
		GameObject closestPoint = Vector3.Distance(newPoint.transform.position, line.GetComponent<LineObject>().point1.transform.position) < Vector3.Distance(newPoint.transform.position, line.GetComponent<LineObject>().point2.transform.position)
		? line.GetComponent<LineObject>().point1 : line.GetComponent<LineObject>().point2;
		
		BuildLine(newPoint, closestPoint);
		newPoint.GetComponent<CreatedObject>().SelectClick();
		
		Destroy(gameObject);
	}
}
