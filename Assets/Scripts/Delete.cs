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
		foreach(GameObject obj in GetObjects("", true)){
			
			// Ако обекта е точка, първо се изтриват всички линии, свързани към нея 
			
			if(obj.name == "Point"){
				AddPointName(obj.GetComponent<PointObject>().GetText());
			// Ако обекта е линия, първо се изтриват всички ъгли, свързани към нея
			}else if(obj.name == "Line"){	
				foreach(GameObject angle in obj.GetComponent<LineObject>().connectedAngles){
					Destroy(obj);
				}
			}
			
			// Селектираният обект се изтрива
			Destroy(obj);
		}
	}
}
