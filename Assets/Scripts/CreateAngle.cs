using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAngle : Tool {

	public GameObject anglePrefab;

	public override void Initiate(){
		List<GameObject> lines = GetObjects("Line", true);
		
		if(lines.Count != 2){
			ReportMessage("2 lines must be selected", 3);
		}else{
			BuildAngle(lines[0], lines[1]);
		}
	}
	
	private void BuildAngle(GameObject line1, GameObject line2){
		foreach(GameObject angle in GetObjects("Angle", false)){
			if((angle.GetComponent<AngleObject>().line1 == line1 && angle.GetComponent<AngleObject>().line2 == line2) ||
			   (angle.GetComponent<AngleObject>().line1 == line2 && angle.GetComponent<AngleObject>().line2 == line1)){
				   ReportMessage("Angle already exists", 3);
				   return;
			   }
		}
		
		GameObject point = null;
		
		if(line1.GetComponent<LineObject>().point1 == line2.GetComponent<LineObject>().point1){
			point = line1.GetComponent<LineObject>().point1;
		}else if(line1.GetComponent<LineObject>().point1 == line2.GetComponent<LineObject>().point2){
			point = line1.GetComponent<LineObject>().point1;
		}else if(line1.GetComponent<LineObject>().point2 == line2.GetComponent<LineObject>().point1){
			point = line1.GetComponent<LineObject>().point2;
		}else if(line1.GetComponent<LineObject>().point2 == line2.GetComponent<LineObject>().point2){
			point = line1.GetComponent<LineObject>().point2;
		}
		
		if(point != null){
			GameObject angle = Instantiate(anglePrefab, point.transform.position, Quaternion.identity, GetTaskTransform());
			
			GameObject p1 = line1.GetComponent<LineObject>().point1 == point ? line1.GetComponent<LineObject>().point2 : line1.GetComponent<LineObject>().point1;
			GameObject p2 = line2.GetComponent<LineObject>().point1 == point ? line2.GetComponent<LineObject>().point2 : line2.GetComponent<LineObject>().point1;
			
			angle.transform.LookAt((p1.transform.position + p2.transform.position) / 2);
			
			float oa = line1.GetComponent<LineObject>().GetLength();
			float ob = line2.GetComponent<LineObject>().GetLength();
			float ab = Vector3.Distance(p1.transform.position, p2.transform.position);
			
			float angleValue = Mathf.Acos( (oa*oa + ob*ob - ab*ab) / (2 * oa * ob) );  //Vector3.Angle mai imashe sm shit like dis
			
			angleValue *= Mathf.Rad2Deg;
			
			//32-full_torus
			angle.GetComponent<AngleObject>().BuildTorus( Mathf.RoundToInt( (4*angleValue) / 45) );
			
			angle.transform.localScale = new Vector3(1f, 0.12f, 1f);
			
			angle.name = "Angle";
			
			Vector3 side1 = p1.transform.position - point.transform.position;
			Vector3 side2 = p2.transform.position - point.transform.position;
			
			Vector3 yVector = Vector3.Cross(side1, side2);
			
			angle.transform.rotation = Quaternion.LookRotation(yVector) * Quaternion.Euler(90f, 0f, 0f);
			
			//um, okay thats here for reasons (angle does the moves thing ._.)
			angle.transform.position = point.transform.position;
			//
			
			angle.transform.LookAt(p2.transform, yVector);
			angle.transform.Rotate(Vector3.up * -90f);
			
			//connect to lines
			angle.GetComponent<AngleObject>().Connect(line1, line2);
			
			//deselect lines
			line1.GetComponent<LineObject>().SelectClick();
			line2.GetComponent<LineObject>().SelectClick();
		}else{
			ReportMessage("The lines don't have a common point", 3);
		}
	}
}
