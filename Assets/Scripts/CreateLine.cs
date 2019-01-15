//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: CreateLine.cs
// НАСЛЕДЕН ОТ: SubdivideLine, CreatePerpendicular, LineSplit
// ЦЕЛ НА КЛАСА: Построяване на обект линия между две дадени точки
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLine : Tool {
	
	public GameObject linePrefab;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Ако броя на селектираните точки от чертежа е две извиква BuildLine 
	// с тях, иначе изписва грешка до потребителя.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		// Списък със всички селектирани точки
		List<GameObject> points = GetObjects("Point", true);
		
		// Ако в списъка има повече или по-малко от два обекта изписва грешка
		if(points.Count != 2){
			ReportMessage("2 points must be selected", 3);
			return;
		}else{
			// Иначе построява линия между тях
			BuildLine(points[0], points[1]);
			
			// Разселектира точките
			foreach(GameObject point in points){
				point.GetComponent<PointObject>().SelectClick();
			}
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: BuildLine
	// Създава обект от типа линия между два игрални обекта от типа точка
	// и го връща.
	// Също така бива извиквана от класове, наследяващи CreateLine.
	// ПАРАМЕТРИ:
	// - GameObject point1 : Едната точка от бъдещата линия
	// - GameObject point2 : Другата точка от същата бъдеща линия
	//------------------------------------------------------------------------
	protected GameObject BuildLine(GameObject point1, GameObject point2){
		
		// Проверка дали линията вече не съществува
		foreach(GameObject connectedLine in point1.GetComponent<PointObject>().lines){
			if(connectedLine.GetComponent<LineObject>().point1 == point2 || connectedLine.GetComponent<LineObject>().point2 == point2){
				ReportMessage("Line already exists", 3);
				return null;
			}
		}
		
		// Инициализиране на обекта линия на позицията на първата точка
		GameObject line = Instantiate(linePrefab, point1.transform.position, Quaternion.identity, GetTaskTransform());
		line.name = "Line";
		
		// Изчисляване на разстоянието между двете точки
		float distance = Vector3.Distance(point1.transform.position, point2.transform.position);

		// Преместване на линията по средата между двете точки
		line.transform.LookAt(point2.transform, line.transform.up * -1);
		line.transform.Rotate(Vector3.left * 90f, Space.Self);
		line.transform.Translate(Vector3.down * (distance/2), Space.Self);
		
		// Оразмеряване на линията, така че да бъде равна на разстоянието между точките
		line.transform.localScale += new Vector3(0f, distance/2, 0f);
		
		// Свързване на линията с точките
		line.GetComponent<LineObject>().SetPoints(point1, point2);
		
		// Връща новата линия
		return line;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: IsBetween
	// Проверява дали определена точка в пространството е между други две
	// и връща булева променлива.
	// Бива извиквана от някои от наследяващите класове
	// ПАРАМЕТРИ:
	// - Vector3 A : Позицията на едната крайна точка
	// - Vector3 B : Позицията на другата крайна точка
	// - Vector3 C : Позицията на проверяваната точка
	//------------------------------------------------------------------------
	protected bool IsBetween (Vector3 A , Vector3 B , Vector3 C) {
		return Vector3.Dot( (B-A).normalized , (C-B).normalized ) < 0f && Vector3.Dot( (A-B).normalized , (C-A).normalized ) < 0f;
	}
}
