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
		
		GameObject createdAngle = Instantiate(anglePrefab, line1.transform.position, Quaternion.identity, GetTaskTransform());
		createdAngle.name = "Angle";
		
		createdAngle.GetComponent<AngleObject>().Connect(line1, line2);
		createdAngle.GetComponent<AngleObject>().UpdateAngle(line1, line2);
		
		line1.GetComponent<LineObject>().SelectClick();
		line2.GetComponent<LineObject>().SelectClick();
	}
}
