//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: ChangeColor.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Смяна на цвета на материала на обекта
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : Tool {

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
			ReportMessage("ERROR: Select at least one object", 3);
			return;
		}
		
		// Смяна на цвета на всеки селектиран обект и деселектирането му
		foreach(GameObject obj in objects){
			obj.GetComponent<CreatedObject>().ChangeColor(c);
			obj.GetComponent<CreatedObject>().SelectClick();
		}
	}
}
