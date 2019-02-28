//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: Delete.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Изтриване на обекти от чертежа
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : ActionsManager {
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Извиква метода за изтриване на игрален обект Destroy за
	// всеки селектиран обект от чертежа
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		List<GameObject> selectedObjects = GetObjects("", true);
		
		if(selectedObjects.Count == 0){
			ReportMessage("ERROR: Select at least one object");
			return;
		}
		
		foreach(GameObject obj in selectedObjects){
			if(obj.name == "Point"){
				AddPointName(obj.GetComponent<PointObject>().GetText());
			}else if(obj.name == "Line"){
				if(obj.GetComponent<LineObject>().point1.GetComponent<PointObject>().lines.Count == 1)
					AddPointName(obj.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText());
				if(obj.GetComponent<LineObject>().point2.GetComponent<PointObject>().lines.Count == 1)
					AddPointName(obj.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText());
				
				foreach(GameObject angle in obj.GetComponent<LineObject>().connectedAngles){
					Destroy(obj);
				}
			}
			Destroy(obj);
		}
		Vibrate();
	}
}
