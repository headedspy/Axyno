//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: CircleObject.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Дефиниране на различни методи, обуславящи
// помощния обект от типа окръжност
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleObject : MonoBehaviour {

	private List<GameObject> lines = null;
	
	private GameObject centerPoint;
	private GameObject angleRotation;
	private Vector3 rotation;
	private float radiusLength;
	private float delta = 1f;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: AddPointName
	// Запазва стойностите и обектите уникално идентифициращи окръжността
	// ПАРАМЕТРИ:
	// - GameObject center : Точката-център на окръжността
	// - GameObject radius : Линията-радиус на окръжността
	// - GameObject angle : Ъгълът-ротация на окръжността
	//------------------------------------------------------------------------
	public void SetObjects(GameObject center, GameObject radius, GameObject angle){
		centerPoint = center;
		angleRotation = angle;
		rotation = angle.transform.eulerAngles;
		radiusLength = radius.GetComponent<LineObject>().GetLength();
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: AddLine
	// Добавя линия, която е част от крговата форма
	// ПАРАМЕТРИ:
	// - GameObject line : Самата линия
	//------------------------------------------------------------------------
	public void AddLine(GameObject line){
		if(lines == null)lines = new List<GameObject>();
		lines.Add(line);
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: OnDestroy
	// При изтриване на окръжността се изтриват всичките ѝ линии
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void OnDestroy(){
		foreach(GameObject line in lines){
			Destroy(line);
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Check
	// Проверка дали тази окръжност е обусловена чрез дадени център, 
	// радиус и ротация
	// ПАРАМЕТРИ:
	// - GameObject center : Точката-център за проверка
	// - GameObject radius : Линията-радиус за проверка
	// - GameObject angle : Ъгълът-ротация за проверка
	//------------------------------------------------------------------------
	public bool Check(GameObject center, GameObject radius, GameObject angle){
		return center == centerPoint && 
		radius.GetComponent<LineObject>().GetLength() < radiusLength + delta && 
		radius.GetComponent<LineObject>().GetLength() > radiusLength - delta &&
		(angle == angleRotation || rotation == angle.transform.eulerAngles);
	}
}
