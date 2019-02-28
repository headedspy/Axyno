//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: LineSplitter.cs
// НАСЛЕДЕН ОТ: LineHover
// ЦЕЛ НА КЛАСА: Създаване на права, благодарение на която
// да се създават нови точки върху линия
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSplit : CreateLine {
	
	public GameObject pointPrefab;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Ако потребителя е селектирал една линия вика BuildSegment
	// с нея, иначе изписва грешка до потребителя
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		List<GameObject> lines = GetObjects("Line", true);
		if(lines.Count != 1){
			ReportMessage("ERROR: Select one line");
		}else{
			BuildSegment(lines[0]);
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: BuildSegment
	// Създава нова права през дадена линия, чрез която се създават
	// нови точки върху нея
	// ПАРАМЕТРИ:
	// - GameObject : Линията, през която ще минава правата
	//------------------------------------------------------------------------
	private void BuildSegment(GameObject line){
		line.transform.GetChild(1).gameObject.GetComponent<Collider>().enabled = false;
		
		GameObject A = line.GetComponent<LineObject>().point1;
		GameObject B = line.GetComponent<LineObject>().point2;
		
		float length = line.GetComponent<LineObject>().GetLength();
		float offset = 10f;
		
		GameObject endPointOne = Instantiate(pointPrefab, A.transform.position, Quaternion.identity);
		GameObject endPointTwo = Instantiate(pointPrefab, B.transform.position, Quaternion.identity);
		
		endPointOne.transform.Translate((A.transform.position - B.transform.position) * offset);
		endPointTwo.transform.Translate((B.transform.position - A.transform.position) * offset);
		
		GameObject newLine = BuildLine(endPointOne, endPointTwo);
		
		Destroy(newLine.GetComponent<LineObject>());
		
		newLine.AddComponent<LineHover>();
		newLine.GetComponent<LineHover>().pointPrefab = pointPrefab;
		newLine.GetComponent<LineHover>().linePrefab = linePrefab;
		newLine.GetComponent<LineHover>().SetObjects(GetTaskTransform(), line);
		
		newLine.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.red;
		newLine.transform.GetChild(1).gameObject.transform.localScale -= new Vector3(0.05f, 0f, 0.05f);
		
		Destroy(endPointOne);
		Destroy(endPointTwo);
		
		line.GetComponent<CreatedObject>().SelectClick();
	}
}
