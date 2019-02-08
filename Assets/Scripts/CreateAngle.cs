//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: CreateAngle.cs
// НАСЛЕДЕН ОТ: SubdivideLine, CreatePerpendicular
// ЦЕЛ НА КЛАСА: Построяване на обект линия между две дадени точки
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAngle : ActionsManager {

	public GameObject anglePrefab;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Ако броя на селектираните линии от чертежа е две извиква BuildAngle 
	// с тях, иначе изписва грешка до потребителя.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		List<GameObject> lines = GetObjects("Line", true);
		
		if(lines.Count != 2){
			ReportMessage("2 lines must be selected", 3);
		}else{
			BuildAngle(lines[0], lines[1]);
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: BuildAngle
	// Създава ъгъл между две свързани линии
	// ПАРАМЕТРИ:
	// - GameObject line1 : Едната линия, съставляваща ъгъла
	// - GameObject line2 : Втората линия, съставляваща ъгъла
	//------------------------------------------------------------------------
	private void BuildAngle(GameObject line1, GameObject line2){
		// Проверка дали ъгъла вече съществува
		foreach(GameObject angle in GetObjects("Angle", false)){
			if((angle.GetComponent<AngleObject>().line1 == line1 && angle.GetComponent<AngleObject>().line2 == line2) ||
			   (angle.GetComponent<AngleObject>().line1 == line2 && angle.GetComponent<AngleObject>().line2 == line1)){
				   ReportMessage("Angle already exists", 3);
				   return;
			   }
		}
		
		// Създаване на обек от тип ъгъл на позицията на едната линия
		GameObject createdAngle = Instantiate(anglePrefab, line1.transform.position, Quaternion.identity, GetTaskTransform());
		createdAngle.name = "Angle";
		
		// Свързване на ъгъла към двете линии
		createdAngle.GetComponent<AngleObject>().Connect(line1, line2);
		
		// Задаване на определената позиция и форма на ъгъла
		createdAngle.GetComponent<AngleObject>().UpdateAngle(line1, line2);
		
		// Деселектиране на двете линии
		line1.GetComponent<LineObject>().SelectClick();
		line2.GetComponent<LineObject>().SelectClick();
		
		
		GameObject centerPoint = createdAngle.GetComponent<AngleObject>().point;
		GameObject point1 = line1.GetComponent<LineObject>().point1 == centerPoint ? line1.GetComponent<LineObject>().point2 : line1.GetComponent<LineObject>().point1;
		GameObject point2 = line2.GetComponent<LineObject>().point1 == centerPoint ? line2.GetComponent<LineObject>().point2 : line2.GetComponent<LineObject>().point1;
		
		
		AddCommand("ANGLE_"+point1.GetComponent<PointObject>().GetText()+"_"+centerPoint.GetComponent<PointObject>().GetText()+"_"+point2.GetComponent<PointObject>().GetText());
	}
}
