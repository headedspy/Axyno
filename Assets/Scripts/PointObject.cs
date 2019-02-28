//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: PointObject.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Дефиниране на различни методи, обуславящи
// обект от типа точка
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : CreatedObject {

	public List<GameObject> lines;
	
	public string name = null;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Start
	// Бива извикана при създаването на обекта. Проверява дали не
	// съвпада с друга точка и се слива в нея ако е така.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Start(){
		if(lines == null)lines = new List<GameObject>();
	}
	
	public void OnDestroy(){
		foreach(GameObject line in lines){
			Destroy(line);
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: ConnectedTo
	// Проверява дали точката е свързана към дадена линия
	// ПАРАМЕТРИ:
	// - GameObject givenLine : Линията, за която ще бъде проверявано
	//------------------------------------------------------------------------
	public bool ConnectedTo(GameObject givenLine){
		foreach(GameObject line in lines){
			if(givenLine == line){
				return true;
			}
		}
		return false;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Connect
	// Свързва дадената линия за точката
	// ПАРАМЕТРИ:
	// - GameObject givenLine : Линията, която ще бъде свързана
	//------------------------------------------------------------------------
	public void Connect(GameObject givenLine){
		if(!lines.Contains(givenLine))lines.Add(givenLine);
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Disconnect
	// Разкача дадената линия от точката
	// ПАРАМЕТРИ:
	// - GameObject givenLine : Линията, която ще бъде разкачена
	//------------------------------------------------------------------------
	public void Disconnect(GameObject givenLine){
		lines.Remove(givenLine);
		
		foreach(GameObject line in lines){
			if(line == null){
				lines.Remove(line);
			}
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: AddText
	// Дава име на точката
	// ПАРАМЕТРИ:
	// - string s : Името, което точката ще има
	//------------------------------------------------------------------------
	public void AddText(string s){
		gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = s;
		name = s;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: GetText
	// Връща името на точката
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public string GetText(){
		return name;
	}
}
