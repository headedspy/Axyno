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
		return Vector3.Distance(point1.transform.position, point2.transform.position);
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: OnDestroy
	// Преди изтриването на линията, тя бива разкачена от всички точки и ъгли
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void OnDestroy(){
		if(point1 != null){
			point1.GetComponent<PointObject>().Disconnect(gameObject);
			if(point1.GetComponent<PointObject>().lines.Count == 0)
				Destroy(point1);
		}
		if(point2 != null){
			point2.GetComponent<PointObject>().Disconnect(gameObject);
			if(point2.GetComponent<PointObject>().lines.Count == 0)
				Destroy(point2);
		}

		if(connectedAngles.Count > 0){
			foreach(GameObject angle in connectedAngles){
				Destroy(angle);
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
	public void UpdatePosition(GameObject point1, GameObject point2){
		GameObject lineMesh = gameObject.transform.GetChild(1).gameObject;
		
		float distance = Vector3.Distance(point1.transform.position, point2.transform.position);
		
		lineMesh.transform.localScale = new Vector3(0.055639f, 0f, 0.055639f);
		
		lineMesh.transform.position = point1.transform.position;
		
		lineMesh.transform.LookAt(point2.transform.position, lineMesh.transform.up * -1);
		
		lineMesh.transform.Rotate(Vector3.left * 90f, Space.Self);
		
		lineMesh.transform.Translate(Vector3.down * (distance/2), Space.Self);
		
		lineMesh.transform.localScale += new Vector3(0f, distance/2, 0f);
		
		foreach(GameObject connectedAngle in connectedAngles){
			connectedAngle.GetComponent<AngleObject>().UpdateAngle(connectedAngle.GetComponent<AngleObject>().line1, connectedAngle.GetComponent<AngleObject>().line2);
		}
		
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
