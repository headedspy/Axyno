//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: SubdivideLine.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Разделяне на линия на няколко равни части
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubdivideLine : CreateLine {
	
	public GameObject pointPrefab;
	public GameObject textObject;
	
	private static int numberOfDivisions = 2;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Разделя всяка селектирана линия на определен на определен 
	// брой равни по дължина части
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		
		// Ако няма селектирани линии изписва грешка до потребителя
		List<GameObject> lines = GetObjects("Line", true);
		if(lines.Count == 0){
			ReportMessage("ERROR: Select at least one line", 3);
			return;
		}
		
		// Разделяне на всяка селектирана линия
		foreach(GameObject line in lines){

			// Изчисляване на размера на линията, както и на всеки отделен сегмент
			float lineLength = line.GetComponent<LineObject>().GetLength();
			float subDistance = lineLength / numberOfDivisions;

			// Създаване на списък за всички нови точки и добавяне на втората точка на линията вътре
			List<GameObject> createdPoints = new List<GameObject>();
			createdPoints.Add(line.GetComponent<LineObject>().point2);

			// Създаване на точките по линията
			for(int i=1; i<numberOfDivisions; i++){
				// Създаване на нова точка на позицията на втората точка на линията
				GameObject tempPoint = Instantiate(pointPrefab, line.GetComponent<LineObject>().point2.transform.position, line.transform.rotation, GetTaskTransform());
				tempPoint.name = "Point";
				
				// Транслиране на новосъздадената точка по линията до съответното ѝ място
				tempPoint.transform.Translate(Vector3.up * subDistance * i, Space.Self);
				
				// Добавяне на новата точка в списъка
				createdPoints.Add(tempPoint);
			}

			
			// Добавяне на първата точка на линията в списъка
			createdPoints.Add(line.GetComponent<LineObject>().point1);
			
			// Запазване на материала на линията
			Material mat = line.GetComponent<Renderer>().material;
			
			// Запазване на всички ъгли, свързани с линията
			List<GameObject> connectedAngles = new List<GameObject>(line.GetComponent<LineObject>().connectedAngles);
			
			
			GameObject firstLine = null;
			GameObject lastLine = null;
			
			for(int i=0; i<numberOfDivisions; i++){
				
				// Създаване на нова линия между две точки със същия материал като оригиналната
				GameObject newLine = BuildLine(createdPoints[i], createdPoints[i+1]);
				newLine.GetComponent<Renderer>().material = mat;
				
				// Запазване на първата и последна новосъздадена линия
				if(i == 0)firstLine = newLine;
				else if(i == numberOfDivisions-1)lastLine = newLine;
			}
			
			// Създаване на спомагателен списък за свързването на ъглите към новите им линии
			List<GameObject> switchList = new List<GameObject>();
			
			
			foreach(GameObject connectedAngle in connectedAngles){
				// За всеки свързан ъгъл се намира по-близката линия - първата или последната от новосъздадените
				GameObject closerLine = Vector3.Distance(connectedAngle.transform.position, firstLine.transform.position) > Vector3.Distance(connectedAngle.transform.position, lastLine.transform.position) ? lastLine : firstLine;
				
				// В списъка се добавят елементи с вледния ред: ъгъл, стара линия, нова линия
				switchList.Add(connectedAngle);
				switchList.Add(line);
				switchList.Add(closerLine);
			}
			
			// Всеки ъгъл бива свързан към новата си линия вместо към оригиналната
			for(int i=0; i<switchList.Count; i += 3){
				switchList[i].GetComponent<AngleObject>().SwitchLine(switchList[i+1], switchList[i+2]);
			}
	
			// Оригиналната линия се унищожава
			Destroy(line);
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: ChangeCount
	// Променя броя на деления на линията
	// ПАРАМЕТРИ:
	// - int count : С колко да се промени (-1 или 1)
	//------------------------------------------------------------------------
	public void ChangeCount(int num){
		int newCount = numberOfDivisions + num;
		
		// Задават се ограничения
		if(newCount < 2)newCount = 2;
		else if(newCount > 10)newCount = 10;
		
		numberOfDivisions = newCount;
		
		// Новото число се показва в сцената
		textObject.GetComponent<TextMesh>().text = newCount.ToString();
	}
}
