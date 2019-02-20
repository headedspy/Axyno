//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: LineObject.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Дефиниране на различни методи, обуславящи
// обект от типа линия
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObject : CreatedObject {
	public GameObject point1, point2;
	public string type = "SOLID";
	
	private bool isTransparent = false;
	
	public List<GameObject> connectedAngles;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Start
	// Бива извикана при създаването на обекта. Инициализира списъка 
	// за свързаните ъгли
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Start(){
		if(connectedAngles == null)connectedAngles = new List<GameObject>();
		
		transform.GetChild(1).gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: SetPoints
	// Свързва точките за линията
	// ПАРАМЕТРИ:
	// - GameObject p1 : Едната точка
	// - GameObject p2 : Втората точка
	//------------------------------------------------------------------------
	public void SetPoints(GameObject p1, GameObject p2){
		this.point1 = p1;
		this.point2 = p2;
		
		// Свързва се и линията за самите точки
		p1.GetComponent<PointObject>().Connect(gameObject);
		p2.GetComponent<PointObject>().Connect(gameObject);
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: GetLength
	// Връща дълживана на линията
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public float GetLength(){
		// Изчислява и връща разсточнието между двеете точки на линията
		return Vector3.Distance(point1.transform.position, point2.transform.position);
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: OnDestroy
	// Преди изтриването на линията, тя бива разкачена от всички точки и ъгли
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void OnDestroy(){
		if(point1 != null && point2 != null){
			point1.GetComponent<PointObject>().Disconnect(gameObject);
			point2.GetComponent<PointObject>().Disconnect(gameObject);

			// Ако точката е част от единична линия, то и линията бива изтрита
			if(point1.GetComponent<PointObject>().lines.Count == 0)Destroy(point1);
			if(point2.GetComponent<PointObject>().lines.Count == 0)Destroy(point2);
			
			// Изтриват се и всички ъгли, свързани към линията
			if(connectedAngles.Count > 0){
				foreach(GameObject angle in connectedAngles){
					Destroy(angle);
				}
			}
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: ChangeColor
	// Променя цвета на линията без това да премахне типа ѝ
	// ПАРАМЕТРИ:
	// - Color c : Цвета, който ще бъде зададен
	//------------------------------------------------------------------------
	public override void ChangeColor(Color c){
		GameObject lineMesh = gameObject.transform.GetChild(1).gameObject;
		
		// Ако линията е била прозрачна, то прозрачността се запазва след промяна на цвета
		if(isTransparent)lineMesh.GetComponent<Renderer>().material.color = new Color(c.r, c.g, c.b, 0.5f);
		else lineMesh.GetComponent<Renderer>().material.color = c;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: SetTransparency
	// Задава помощен параметър дали линията е прозрачна
	// ПАРАМЕТРИ:
	// - bool state : Стойност за прозрачността
	//------------------------------------------------------------------------
	public void SetTransparency(bool state){
		isTransparent = state;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: ConnectAngle
	// Свързване на ъгъл за правата
	// ПАРАМЕТРИ:
	// - GameObject angle : Ъгъла, който ще бъде свързан
	//------------------------------------------------------------------------
	public void ConnectAngle(GameObject angle){
		connectedAngles.Add(angle);
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: DisconnectAngle
	// Разкачване на ъгъл от правата
	// ПАРАМЕТРИ:
	// - GameObject angle : Ъгъла, който ще бъде Разкачен
	//------------------------------------------------------------------------
	public void DisconnectAngle(GameObject angle){
		connectedAngles.Remove(angle);
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: UpdatePosition
	// Промяна на позицията на линията между две позиции в пространството
	// ПАРАМЕТРИ:
	// - Vector3 point1 : Първите координати на линията
	// - Vector3 point2 : Вторите координати на линията
	//------------------------------------------------------------------------
	public void UpdatePosition(Vector3 point1, Vector3 point2){
		GameObject lineMesh = gameObject.transform.GetChild(1).gameObject;
		
		// Изчислява се разстоянието между двете точки
		float distance = Vector3.Distance(point1, point2);
		
		// Нулиране на размера на линията
		lineMesh.transform.localScale = new Vector3(0.055639f, 0f, 0.055639f);
		
		// Правата се поставя на координатите на първата точка
		lineMesh.transform.position = point1;
		
		// Насочване на Z-вектора на линията към втората точка
		lineMesh.transform.LookAt(point2, lineMesh.transform.up * -1);
		
		// Завъртане на линията на 90 градуса, така че Y-вектора вече да "гледа" към втората точка
		lineMesh.transform.Rotate(Vector3.left * 90f, Space.Self);
		
		// Транслиране на обекта до средната позиция между двете точки
		lineMesh.transform.Translate(Vector3.down * (distance/2), Space.Self);
		
		// Задаване на правилната дължина на правата (разстоянието върху две за всяка половинка)
		lineMesh.transform.localScale += new Vector3(0f, distance/2, 0f);
		
		// Преизчисляване на свързаните ъгли спрямо новите координати на линията
		foreach(GameObject connectedAngle in connectedAngles){
			connectedAngle.GetComponent<AngleObject>().UpdateAngle(connectedAngle.GetComponent<AngleObject>().line1, connectedAngle.GetComponent<AngleObject>().line2);
		}
		
		// Оразмеряване наново и позициониране на текста на правата
		Transform lineText = gameObject.transform.GetChild(0);
		lineText.gameObject.transform.localScale = new Vector3(-0.067107f, 0.067107f, 0.067107f);
		lineText.position = lineMesh.transform.position;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: AddText
	// Дава репрезентативна стойност на линията
	// ПАРАМЕТРИ:
	// - string s : Стойността, което линията ще има
	//------------------------------------------------------------------------
	public void AddText(string s){
		gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = s;
	}
}
