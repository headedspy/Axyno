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
		// Спуска се лъч от центъра на екрана напред
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit)){
			if(hit.transform.gameObject.transform.parent.gameObject == gameObject){
				// Ако лъчът удари помощната линия се създава временна точка на мястото да колизията
				Destroy(newPoint);
				newPoint = Instantiate(pointPrefab, hit.point, Quaternion.identity, task);
				newPoint.name = "TempPoint";
			}
		}
		
		// Ако потребителя потвърди и има създадена временна точка се извиква ExtendToPoint метода
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
		// Именуваме новата точка подходящо, за да бъде откриваема
		newPoint.name = "Point";
		
		// Ако временната точка се намира в линията
		if(IsBetween(line.GetComponent<LineObject>().point1.transform.position, line.GetComponent<LineObject>().point2.transform.position, newPoint.transform.position)){
			location = "IN";
			
			// Запазва се материала на линията
			Material mat = line.transform.GetChild(1).gameObject.GetComponent<Renderer>().material;
			
			// Създават се две нови линии на мястото на старата, като материала се пренася
			GameObject newLine = BuildLine(line.GetComponent<LineObject>().point1, newPoint);
			GameObject newLine2 = BuildLine(line.GetComponent<LineObject>().point2, newPoint);
			
			newLine.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = mat;
			newLine2.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = mat;
			
			Destroy(line);
			
		}else{
			// Ако временната точка се намира извън линията
			location = "OUT";
			
			// Изчислява се по-близката точка от линията до новосъздадената временна точка
			GameObject closestPoint = Vector3.Distance(newPoint.transform.position, line.GetComponent<LineObject>().point1.transform.position) < Vector3.Distance(newPoint.transform.position, line.GetComponent<LineObject>().point2.transform.position)
			? line.GetComponent<LineObject>().point1 : line.GetComponent<LineObject>().point2;
			
			// Построява се линия между по-близката точка от линията и новата точка
			BuildLine(newPoint, closestPoint);
			
			// Селектираме новата точка понеже след потвърждението от потребителя, тя ще бъде селектирана
			newPoint.GetComponent<CreatedObject>().SelectClick();
		}
		
		// Включваме обратно колизията на линията
		line.transform.GetChild(1).gameObject.GetComponent<Collider>().enabled = true;
		
		//Премахваме спомагателната права
		Destroy(gameObject);
		
		NamePoints();
		
		AddCommand("EXPAND_" + line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() + "_" + line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() + "_" + location + "_" + newPoint.GetComponent<PointObject>().GetText());
	}
}
