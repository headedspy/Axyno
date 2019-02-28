//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: CreateCircle.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Създаване на окръжност с даден център, радиус и посока
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCircle : CreateLine {
	
	public GameObject pointPrefab;
	public GameObject circlePrefab;

	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Проверка за правилно селектирани обекти и създаване на окръжност
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		List<GameObject> lines = GetObjects("Line", true);
		List<GameObject> points = GetObjects("Point", true);
		List<GameObject> angles = GetObjects("Angle", true);
		
		if(lines.Count != 1){
			ReportMessage("ERROR: Select one line");
			return;
		}
		if(points.Count != 1){
			ReportMessage("ERROR: Select one point");
			return;
		}
		if(angles.Count != 1){
			ReportMessage("ERROR: Select one angle");
			return;
		}
		
		GameObject line1 = lines[0];
		GameObject point1 = points[0];
		GameObject angle = angles[0];
		
		foreach(GameObject circle in GetObjects("Circle", false)){
			if(circle.GetComponent<CircleObject>().Check(point1, line1, angle)){
				ReportMessage("ERROR: Circle already exists");
				return;
			}
		}
		
		if(!point1.GetComponent<PointObject>().lines.Contains(line1)){
			ReportMessage("ERROR: Line is not connected");
			return;
		}
		if(!line1.GetComponent<LineObject>().connectedAngles.Contains(angle)){
			ReportMessage("ERROR: Angle in not connected");
			return;
		}
		
		int pointsCount = Mathf.RoundToInt(2 * Mathf.PI * line1.GetComponent<LineObject>().GetLength() / (pointPrefab.transform.localScale.x * 2f));
		
		float angleOfRotation = 360f / pointsCount;
		
		GameObject line2 = angle.GetComponent<AngleObject>().line1 == line1 ? angle.GetComponent<AngleObject>().line2 : angle.GetComponent<AngleObject>().line1;
		
		GameObject point2 = line1.GetComponent<LineObject>().point1 == point1 ? line1.GetComponent<LineObject>().point2 : line1.GetComponent<LineObject>().point1;
		GameObject point3 = line2.GetComponent<LineObject>().point1 == point1 ? line2.GetComponent<LineObject>().point2 : line2.GetComponent<LineObject>().point1;
		
		Vector3 axis = Vector3.Cross(point3.transform.position - point1.transform.position, point2.transform.position - point1.transform.position);
		
		GameObject lastPoint = point2;

		GameObject circleObject = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity, GetTaskTransform());
		circleObject.name = "Circle";
		circleObject.GetComponent<CircleObject>().SetObjects(point1, line1, angle);
		
		for(int i=1; i<pointsCount; i++){
			GameObject point = Instantiate(pointPrefab, point2.transform.position, Quaternion.identity, GetTaskTransform());
			point.name = "Point";
			
			point.transform.RotateAround(point1.transform.position, axis, angleOfRotation * i);
			
			GameObject line = BuildLine(lastPoint, point);
			
			circleObject.GetComponent<CircleObject>().AddLine(line);
			
			lastPoint = point;
		}
		
		circleObject.GetComponent<CircleObject>().AddLine(BuildLine(lastPoint, point2));
		
		
		angle.GetComponent<CreatedObject>().SelectClick();
		point1.GetComponent<CreatedObject>().SelectClick();
		line1.GetComponent<CreatedObject>().SelectClick();

		Vibrate();
		
		GameObject anglePoint = line2.GetComponent<LineObject>().point1 == point1 ? line2.GetComponent<LineObject>().point2 : line2.GetComponent<LineObject>().point1;
		AddCommand("CIRCLE_"+point1.GetComponent<PointObject>().GetText()+"_"+point2.GetComponent<PointObject>().GetText()+"_"+anglePoint.GetComponent<PointObject>().GetText());
	}
}
