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
		
		// Вика BuildSegment ако е селектирана една линия, иначе връща грешка
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
		
		// Изключва колизията на линията за да не пречи на колизията с новата линия
		line.transform.GetChild(1).gameObject.GetComponent<Collider>().enabled = false;
		
		// Запазване на двете точки, съставляващи линията
		GameObject A = line.GetComponent<LineObject>().point1;
		GameObject B = line.GetComponent<LineObject>().point2;
		
		// Изчисляване на дължината на линията и избиране на дължината на новата линия
		float length = line.GetComponent<LineObject>().GetLength();
		float offset = 10f;
		
		// Създаване на точки за товата линия с позиция като точките на линията
		GameObject endPointOne = Instantiate(pointPrefab, A.transform.position, Quaternion.identity);
		GameObject endPointTwo = Instantiate(pointPrefab, B.transform.position, Quaternion.identity);
		
		// Транслиране на новите точки навън от линията на определено разстояние
		endPointOne.transform.Translate((A.transform.position - B.transform.position) * offset);
		endPointTwo.transform.Translate((B.transform.position - A.transform.position) * offset);
		
		// Построяване на новата линия
		GameObject newLine = BuildLine(endPointOne, endPointTwo);
		
		// Изтриване на компонента за обект линия от новосъздадената права
		Destroy(newLine.GetComponent<LineObject>());
		
		// Добавяне на LineHover компонента към новата линия, който ще осъществява самата функционалност
		newLine.AddComponent<LineHover>();
		newLine.GetComponent<LineHover>().pointPrefab = pointPrefab;
		newLine.GetComponent<LineHover>().linePrefab = linePrefab;
		newLine.GetComponent<LineHover>().SetObjects(GetTaskTransform(), line);
		
		// Промяна на дебелината и цвета на линията
		newLine.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.red;
		newLine.transform.GetChild(1).gameObject.transform.localScale -= new Vector3(0.05f, 0f, 0.05f);
		
		// Изтриване на новосъздадените точки
		Destroy(endPointOne);
		Destroy(endPointTwo);
		
		// Деселектиране на линията
		line.GetComponent<CreatedObject>().SelectClick();
	}
}
