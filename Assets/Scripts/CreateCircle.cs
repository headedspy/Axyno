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

	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Проверка за правилно селектирани обекти и създаване на окръжност
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		
		// Проверка за правилно селектирани точка, линия и ъгъл
		List<GameObject> lines = GetObjects("Line", true);
		List<GameObject> points = GetObjects("Point", true);
		List<GameObject> angles = GetObjects("Angle", true);
		
		if(lines.Count != 1){
			ReportMessage("ERROR: Select one line", 3);
			return;
		}
		if(points.Count != 1){
			ReportMessage("ERROR: Select one point", 3);
			return;
		}
		if(angles.Count != 1){
			ReportMessage("ERROR: Select one angle", 3);
			return;
		}
		
		GameObject line1 = lines[0];
		GameObject point1 = points[0];
		GameObject angle = angles[0];
		
		if(!point1.GetComponent<PointObject>().lines.Contains(line1)){
			ReportMessage("ERROR: Line is not connected", 3);
			return;
		}
		if(!line1.GetComponent<LineObject>().connectedAngles.Contains(angle)){
			ReportMessage("ERROR: Angle in not connected", 3);
			return;
		}
		
		// Изчисляване на брой точки от окръжността
		int pointsCount = Mathf.RoundToInt(2 * Mathf.PI * line1.GetComponent<LineObject>().GetLength() / (pointPrefab.transform.localScale.x * 2f));
		
		// Изчисляване на ъгъла между всеки две точки
		float angleOfRotation = 360f / pointsCount;
		
		
		// Намиране на необходимите останали обекти от чертежа
		GameObject line2 = angle.GetComponent<AngleObject>().line1 == line1 ? angle.GetComponent<AngleObject>().line2 : angle.GetComponent<AngleObject>().line1;
		
		GameObject point2 = line1.GetComponent<LineObject>().point1 == point1 ? line1.GetComponent<LineObject>().point2 : line1.GetComponent<LineObject>().point1;
		GameObject point3 = line2.GetComponent<LineObject>().point1 == point1 ? line2.GetComponent<LineObject>().point2 : line2.GetComponent<LineObject>().point1;
		
		// Изчисляване на перпендикулярния вектор на равнината, описана от селектираните обекти
		Vector3 axis = Vector3.Cross(point3.transform.position - point1.transform.position, point2.transform.position - point1.transform.position);
		
		GameObject lastPoint = point2;

		// Създаване на точките
		for(int i=1; i<pointsCount; i++){
			GameObject point = Instantiate(pointPrefab, point2.transform.position, Quaternion.identity, GetTaskTransform());
			point.name = "Point";
			
			// Завъртане на новосъздадената точка с изчисления ъгъл около перпендикулярния вектор
			point.transform.RotateAround(point1.transform.position, axis, angleOfRotation * i);
			
			// Свързване на предходните две точки
			BuildLine(lastPoint, point);
			
			lastPoint = point;
		}
		
		// Свързване на последните две точки
		BuildLine(lastPoint, point2);
		
		// Деселектиране на обектите
		angle.GetComponent<CreatedObject>().SelectClick();
		point1.GetComponent<CreatedObject>().SelectClick();
		line1.GetComponent<CreatedObject>().SelectClick();
	}
}
