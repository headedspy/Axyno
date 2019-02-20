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
		// Инициализира се списъкът със свързаните линии
		if(lines == null)lines = new List<GameObject>();
		
		/*
		// Взимат се всички collider-и, които са на разстояние по-малко или равно на радиуса на сферата, репрезентираща точката
		Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, gameObject.transform.localScale.x);
		
		List<GameObject> transferedLines = new List<GameObject>();
		
		// Ако collider-а принаднежи на точка, то правите биват свързани към текущата точка
		foreach(Collider collider in hitColliders){
			if(collider.name == "Point" && collider.gameObject != gameObject){
				foreach(GameObject line in collider.gameObject.GetComponent<PointObject>().lines){
					transferedLines.Add(line);
				}
				
				// Разкачане на линиите, свързани към точката
				foreach(GameObject line in transferedLines){
					collider.gameObject.GetComponent<PointObject>().Disconnect(line);
					lines.Add(line);
				}
				
				// Премахване на втората точка
				Destroy(collider.gameObject);
			}
		}
		*/
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
		// Задаване на текста на дъщерния обект на точката
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
		// Връщане на текста на дъщерния обект на точката
		return name;
	}
}
