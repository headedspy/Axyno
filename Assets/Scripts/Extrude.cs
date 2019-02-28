//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: Extrude.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Избутване на лице в пространството 
// (и компресирането му в точка)
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extrude : CreateLine {
	
	public GameObject pointPrefab;
	
	private static GameObject centerPoint;
	
	private static Vector3 newPos;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Проверка за правилно селектирани линии и създаване на точка-маркер
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		List<GameObject> lines = GetObjects("Line", true);
		if(lines.Count <= 2){
			ReportMessage("ERROR: Select at lest 3 lines");
			return;
		}
		if(!HasLoop(lines)){
			ReportMessage("ERROR: Select lines that form a loop");
			return;
		}
		
		Vector3 testPos = new Vector3(0f, 0f, 0f);
		foreach(GameObject line in lines){
			testPos += line.GetComponent<LineObject>().point1.transform.position;
			testPos += line.GetComponent<LineObject>().point2.transform.position;
		}
		testPos /= ((float)lines.Count * 2f);
		
		centerPoint = Instantiate(pointPrefab, testPos, Quaternion.identity, GetTaskTransform());
		
		newPos = Vector3.zero;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: HasLoop
	// Проверява нали дадения списък от линии образува затворен контур
	// ПАРАМЕТРИ:
	// - List<GameObject> lines : Списъка с линии
	//------------------------------------------------------------------------
	private bool HasLoop(List<GameObject> lines){
		List<GameObject> points = GetPoints(lines, true);
		
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
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: GetPoints
	// Връща всички точки на подаделите линии в списък
	// ПАРАМЕТРИ:
	// - List<GameObject> lines : Списъка с линии
	// - bool addTwice : Пропускане на проверката дали точката
	//					 вече не е добавена в списъка
	//------------------------------------------------------------------------
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
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: ChangePos
	// Променя позицията на точката-маркер
	// ПАРАМЕТРИ:
	// - string axis : Името на глобалния вектор, по който 
	//				   точката-маркер ще бъде преместена:
	//				   ("X", "-X", "Y", "-Y", "Z", "-Z")
	//------------------------------------------------------------------------
	public void ChangePos(string axis){
		
		if(centerPoint == null){
			ReportMessage("ERROR: MARKER POINT NOT FOUND");
			return;
		}
		
		float amount = 0.1f;
		
		if(axis[0] == '-')amount *= -1;
		
		if(axis[1] == 'X'){
			centerPoint.transform.position += new Vector3(amount, 0f, 0f);
			newPos.x += amount;
		}else if(axis[1] == 'Y'){
			centerPoint.transform.position += new Vector3(0f, amount, 0f);
			newPos.y += amount;
		}else if(axis[1] == 'Z'){
			centerPoint.transform.position += new Vector3(0f, 0f, amount);
			newPos.z += amount;
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Confirm
	// Избутване на формата до точката-маркер (и сливането ѝ)
	// ПАРАМЕТРИ:
	// - bool isExtrude : Обекта в призма или пирамида ще бъде трансформиран
	//					  (true - само избутване / false - избутване и сливане)
	//------------------------------------------------------------------------
	public void Confirm(bool isExtrude){
		Dictionary<GameObject, GameObject> ortho = new Dictionary<GameObject, GameObject>();
		
		List<GameObject> points = GetPoints(GetObjects("Line", true), false);
		
		if(points.Count <= 3){
			ReportMessage("ERROR: 3 points are not selected");
			return;
		}
		
		if(isExtrude){
			foreach(GameObject point in points){
				GameObject extrudedPoint = Instantiate(pointPrefab, new Vector3(point.transform.position.x + newPos.x,
																				point.transform.position.y + newPos.y,
																				point.transform.position.z + newPos.z), Quaternion.identity, GetTaskTransform());
													 
				extrudedPoint.name = "Point";
				
				NamePoints();
				
				ortho.Add(point, extrudedPoint);
				
				BuildLine(extrudedPoint, point);
			}
			
			List<GameObject> lines = GetObjects("Line", true);
			foreach(GameObject line in lines){
				BuildLine(ortho[line.GetComponent<LineObject>().point1],
						  ortho[line.GetComponent<LineObject>().point2]);
			}
			
			Destroy(centerPoint);
			
			AddCommand("EXTRUDE_"+ortho.Count+"_PRISM");
			
		}else{
			centerPoint.name = "Point";
			NamePoints();
		
			int lines = 0;
		
			foreach(GameObject point in points){
				BuildLine(centerPoint, point);
				lines++;
			}
			
			AddCommand("EXTRUDE_"+lines.ToString()+"_PYRAMID");
		}
		
		foreach(GameObject line in GetObjects("Line", true)){
			line.GetComponent<CreatedObject>().SelectClick();
		}
		
		Vibrate();
	}
}
