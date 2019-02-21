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
	public GameObject menu;
	
	private static GameObject centerPoint;
	
	private static Vector3 newPos;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Проверка за правилно селектирани линии и създаване на точка-маркер
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		
		// Проверка дали избраните обекти са линии, образуващи затворен контур
		List<GameObject> lines = GetObjects("Line", true);
		if(lines.Count <= 2){
			ReportMessage("ERROR: Select at lest 3 lines");
			return;
		}
		if(!HasLoop(lines)){
			ReportMessage("ERROR: Select lines that form a loop");
			return;
		}
		
		// Създава се точката-маркер в центъра на контура
		Vector3 testPos = new Vector3(0f, 0f, 0f);
		foreach(GameObject line in lines){
			testPos += line.GetComponent<LineObject>().point1.transform.position;
			testPos += line.GetComponent<LineObject>().point2.transform.position;
		}
		testPos /= ((float)lines.Count * 2f);
		
		centerPoint = Instantiate(pointPrefab, testPos, Quaternion.identity, GetTaskTransform());
		
		// Включва се второстепенното меню на инструмента
		menu.SetActive(true);
		
		// Изчистване на предходни отмествания
		newPos = Vector3.zero;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: HasLoop
	// Проверява нали дадения списък от линии образува затворен контур
	// ПАРАМЕТРИ:
	// - List<GameObject> lines : Списъка с линии
	//------------------------------------------------------------------------
	private bool HasLoop(List<GameObject> lines){
		
		// Взимат се всички точки между линиите, като всяка точка от контура бива добавена два пъти
		List<GameObject> points = GetPoints(lines, true);
		
		// Ако всяка точка я има два пъти в списъка, линиите образуват контур
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
		
		// Създаване на нов списък
		List<GameObject> points = new List<GameObject>();
		
		// Всяка точка от всяка линия бива добавена в списъка
		// Има проверка дали точката вече не е добавена;
		// addTwice = true за да бъде пропусната и списъка да съдържа два пъти точките от контура
		foreach(GameObject line in lines){
			GameObject p1 = line.GetComponent<LineObject>().point1;
			GameObject p2 = line.GetComponent<LineObject>().point2;
			
			if(!points.Contains(p1) || addTwice)points.Add(p1);
			if(!points.Contains(p2) || addTwice)points.Add(p2);
		}
		
		// Връщане на списъка
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
		// Отместване
		float amount = 0.1f;
		
		// Ако вектора е с отрицателна посока, промени отместването в другата посока
		if(axis[0] == '-')amount *= -1;
		
		// Спрямо посоката на вектора се променя позицията на точката-маркер
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
		//string pointNames = "";
		
		
		// Скриване на подменюто
		menu.SetActive(false);
		
		// Създаване на нов речник, съдържащ оригиналната точка и нейната ортографска проекция
		Dictionary<GameObject, GameObject> ortho = new Dictionary<GameObject, GameObject>();
		
		// Запазване на всички точки от контура без повтаряне в списък
		List<GameObject> points = GetPoints(GetObjects("Line", true), false);
		
		// Ако имаме само избутване
		if(isExtrude){
			foreach(GameObject point in points){
				// На всяка точка от контура ѝ създаваме ортографска проекция към точката-маркер
				GameObject extrudedPoint = Instantiate(pointPrefab, new Vector3(point.transform.position.x + newPos.x,
																				point.transform.position.y + newPos.y,
																				point.transform.position.z + newPos.z), Quaternion.identity, GetTaskTransform());
													 
				extrudedPoint.name = "Point";
				
				NamePoints();
				
				//pointsNames += extrudedPoint.GetComponent<PointObject>().GetText() + "_";
				
				// Добавяме двете точки в речника
				ortho.Add(point, extrudedPoint);
				
				// Построяваме линия между всяка точка и проекцията ѝ (стените на призмата)
				BuildLine(extrudedPoint, point);
			}
			
			// Построяваме ръбовете на избутаната форма като свързваме проекциите на точките по идентичен начин
			List<GameObject> lines = GetObjects("Line", true);
			foreach(GameObject line in lines){
				BuildLine(ortho[line.GetComponent<LineObject>().point1],
						  ortho[line.GetComponent<LineObject>().point2]);
			}
			
			// Изтриваме точката-маркер
			Destroy(centerPoint);
			
			//NamePoints();
			
			/*
			foreach(GameObject point in points){
				pointNames += ortho[point].GetComponent<PointObject>().GetText() + "_";
			}
			*/
			
			AddCommand("EXTRUDE_"+ortho.Count+"_PRISM");
			
		}else{
			centerPoint.name = "Point";
			NamePoints();
		// Ако имаме избутване и сливане
		
			int lines = 0;
		
			foreach(GameObject point in points){
				// Построяваме линия между всяка точка от контура и точката-маркер
				BuildLine(centerPoint, point);
				lines++;
			}
			// Правим точката-маркер на нормална точка
			
			AddCommand("EXTRUDE_"+lines.ToString()+"_PYRAMID");
			//pointNames += centerPoint.GetComponent<PointObject>().GetText() + "_";
		}
		
		// Деселектираме всички линии
		foreach(GameObject line in GetObjects("Line", true)){
			line.GetComponent<CreatedObject>().SelectClick();
		}
		
		//AddCommand("EXTRUDE_"+pointNames.Substring(0, pointNames.Length-1));
	}
}
