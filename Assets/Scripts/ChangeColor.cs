//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: ChangeColor.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Смяна на цвета на материала на обекта
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : ActionsManager {

	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Взима цвета на бутона, който е натиснат и го прилага на всички
	// селектирани обекти
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		// Запазване на цвета на натиснатия бутон
		Color c = gameObject.GetComponent<Renderer>().material.color;
		
		// Ако няма селектирани обеки връща грешка до портебителя
		List<GameObject> objects = GetObjects("", true);
		if(objects.Count == 0){
			ReportMessage("ERROR: Select at least one object");
			return;
		}
		
		// Смяна на цвета на всеки селектиран обект и деселектирането му
		foreach(GameObject obj in objects){
			GameObject manipulativeObject = obj;
			if(obj.name == "Line"){
				manipulativeObject = obj.transform.GetChild(1).gameObject;
			}
			
			Color oldColor = manipulativeObject.GetComponent<Renderer>().material.color;
			
			obj.GetComponent<CreatedObject>().ChangeColor(c);
			obj.GetComponent<CreatedObject>().SelectClick();
			
			string objPoints = "";
			
			if(obj.GetComponent<PointObject>() != null){
				objPoints += obj.GetComponent<PointObject>().GetText();
			}else if(obj.GetComponent<LineObject>() != null){
				objPoints += obj.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() + "_";
				objPoints += obj.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText();
			}else if(obj.GetComponent<AngleObject>() != null){
				GameObject l1 =  obj.GetComponent<AngleObject>().line1;
				GameObject l2 =  obj.GetComponent<AngleObject>().line2;
				GameObject centerPoint = obj.GetComponent<AngleObject>().point;
				
				GameObject firstPoint = l1.GetComponent<LineObject>().point1 == centerPoint ? l1.GetComponent<LineObject>().point2 : l1.GetComponent<LineObject>().point1;
				GameObject secondPoint = l2.GetComponent<LineObject>().point1 == centerPoint ? l2.GetComponent<LineObject>().point2 : l2.GetComponent<LineObject>().point1;
				
				objPoints += firstPoint.GetComponent<PointObject>().GetText()+"_"+centerPoint.GetComponent<PointObject>().GetText()+"_"+secondPoint.GetComponent<PointObject>().GetText();
			}
			
			AddCommand("COLOR_"+oldColor.r+","+oldColor.g+","+oldColor.b+"_"+c.r+","+c.g+","+c.b+"_"+objPoints);
		}
	}
}
