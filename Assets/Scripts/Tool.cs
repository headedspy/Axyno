//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: Tool.cs
// НАСЛЕДЕН ОТ: ActionsManager
// ЦЕЛ НА КЛАСА: Създаване на помощни методи, използвани от
// наследяващите класове
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour {
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Бива извиквана при натискане на съответния бутон на инструмента
	// Презаписва се от всеки наследяващ метод.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public abstract void Initiate();
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: GetObjects
	// Връща списък със всички обекти в чертежа. Може да бъде филтриран 
	// по тип и дали обекта е селектиран
	// ПАРАМЕТРИ:
	// - string name : Избор само на обекти с това име 
	//                 (празен string за всички обекти)
	// - bool selected : Избор само на селектирани обекти
	//------------------------------------------------------------------------
	protected List<GameObject> GetObjects(string name, bool selected){
		List<GameObject> list = new List<GameObject>();
		
		Transform task = GetTaskTransform();
		
		foreach(Transform child in task){
			if(child.gameObject.name == name || name == ""){
				if(selected){
					if(child.gameObject.GetComponent<CreatedObject>().isSelected){
						list.Add(child.gameObject);
					}
				}else{
					list.Add(child.gameObject);
				}
			}
		}
		
		return list;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: ReportMessage
	// Изписва съобщение до потребителя
	// ПАРАМЕТРИ:
	// - string name : Съобщението, което ще бъде изведено
	//------------------------------------------------------------------------
	protected void ReportMessage(string message){
		GameObject.Find("OutputText").GetComponent<TextMesh>().text = message;
		
		StartCoroutine(CleanMessage(message));
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: CleanMessage
	// Изчаква пет секунди и изчиства съобщението до потребителя
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	private IEnumerator CleanMessage(string message){
		yield return new WaitForSeconds(5);
		if(GameObject.Find("OutputText").GetComponent<TextMesh>().text == message)
			GameObject.Find("OutputText").GetComponent<TextMesh>().text = "";
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: GetTaskTransform
	// Връща Transform обекта на чертежа, позволявайки всеки нов обект 
	// да бъде закачен като дъщерен за него.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	protected Transform GetTaskTransform(){
		return GameObject.Find("Task").transform;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Vibrate
	// Включва вибрацията на мобилното устройство за къс период от време
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	protected void Vibrate(){
		Handheld.Vibrate ();
	}
	
	private static List<string> deletedPoints = null;
	
	
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: AddPointName
	// Добавя името на изтрита точка към списъка за бъдещо използване
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void AddPointName(string name){
		if(!deletedPoints.Contains(name))
			deletedPoints.Add(name);
	}
	
	private static char nextPointName = 'a';
	private static int prefix = 0;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: NamePoints
	// Иненуване на всички безименни точки на чертежа
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	protected void NamePoints(){
		List<GameObject> points = GetObjects("Point", false);
		List<GameObject> circles = GetObjects("Circle", false);
		
		foreach(GameObject circle in circles){
			List<GameObject> lines = circle.GetComponent<CircleObject>().lines;
			
			foreach(GameObject line in lines){
				GameObject point1 = line.GetComponent<LineObject>().point1;
				GameObject point2 = line.GetComponent<LineObject>().point2;
				
				if(points.Contains(point1))points.Remove(point1);
				if(points.Contains(point2))points.Remove(point2);
			}
		}
		
		foreach(GameObject point in points){
			if(point.GetComponent<PointObject>().GetText() == ""){
				
				if(deletedPoints == null){
					deletedPoints = new List<string>();
				}
				
				if(deletedPoints.Count > 0){
					string pointName = deletedPoints[0];
					deletedPoints.Remove(pointName);
					
					point.GetComponent<PointObject>().AddText(pointName);
					
					continue;
				}
				
				string prefixString;
				
				if(prefix==0)prefixString = "";
				else prefixString = prefix.ToString();
				
				point.GetComponent<PointObject>().AddText(nextPointName.ToString() + prefixString);
				
				if(nextPointName == 'z'){
					nextPointName = 'a';
					prefix++;
				}
				nextPointName++;
			}
		}
	}
}
