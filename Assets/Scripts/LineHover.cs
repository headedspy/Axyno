//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: LineHover.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Помощен клас за деление/удължаване на линия
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHover : LineSplit {
	private Transform task;
	private GameObject line;
	
	private GameObject newPoint = null;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: SetObjects
	// Прехвърля трансформационния обект на чертежа както и линията 
	// от LineSplit класа
	// ПАРАМЕТРИ:
	// - Transform taskTransform : трансформационния обект на чертежа
	// - GameObject transferedLine : линията, която ще бъде 
	//								 разделяна / удължавана
	//------------------------------------------------------------------------
	public void SetObjects(Transform taskTransform, GameObject transferedLine){
		task = taskTransform;
		line = transferedLine;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Update
	// Функцията бива извиквана на всеки кадър.
	// Показва къде ще бъде създадена точка и следи за потвърждение
	// от потребителя
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Update(){
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit)){
			if(hit.transform.gameObject.transform.parent.gameObject == gameObject){
				Destroy(newPoint);
				newPoint = Instantiate(pointPrefab, hit.point, Quaternion.identity, task);
				newPoint.name = "TempPoint";
			}
		}
		
		if(Input.GetMouseButtonDown(0) && newPoint != null) ExtendToPoint();
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: ExtendToPoint
	// Функцията Удължава/разделя линията до определената точка
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	private void ExtendToPoint(){
		string location;
		newPoint.name = "Point";
		
		if(IsBetween(line.GetComponent<LineObject>().point1.transform.position, line.GetComponent<LineObject>().point2.transform.position, newPoint.transform.position)){
			location = "IN";
			
			Material mat = line.transform.GetChild(1).gameObject.GetComponent<Renderer>().material;
			
			GameObject newLine = BuildLine(line.GetComponent<LineObject>().point1, newPoint);
			GameObject newLine2 = BuildLine(line.GetComponent<LineObject>().point2, newPoint);
			
			newLine.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = mat;
			newLine2.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = mat;
			
			List<GameObject> connectedAngles = new List<GameObject>(line.GetComponent<LineObject>().connectedAngles);
			
			foreach(GameObject connectedAngle in connectedAngles){
				GameObject closerNewLine = Vector3.Distance(connectedAngle.transform.position, newLine.transform.position) > Vector3.Distance(connectedAngle.transform.position, newLine2.transform.position) ? newLine2 : newLine;
				
				connectedAngle.GetComponent<AngleObject>().SwitchLine(line, closerNewLine);
			}
			
			Destroy(line);
			
		}else{
			location = "OUT";
			
			GameObject closestPoint = Vector3.Distance(newPoint.transform.position, line.GetComponent<LineObject>().point1.transform.position) < Vector3.Distance(newPoint.transform.position, line.GetComponent<LineObject>().point2.transform.position)
			? line.GetComponent<LineObject>().point1 : line.GetComponent<LineObject>().point2;
			
			BuildLine(newPoint, closestPoint);
			
			newPoint.GetComponent<CreatedObject>().SelectClick();
		}
		
		line.transform.GetChild(1).gameObject.GetComponent<Collider>().enabled = true;
		
		Destroy(gameObject);
		
		NamePoints();
		
		Vibrate();
		
		AddCommand("EXPAND_" + line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() + "_" + line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() + "_" + location + "_" + newPoint.GetComponent<PointObject>().GetText());
	}
}
