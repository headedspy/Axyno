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
		List<GameObject> lines = GetObjects("Line", true);
		if(lines.Count == 0){
			ReportMessage("ERROR: Select at least one line");
			return;
		}
		
		foreach(GameObject line in lines){
			string pointsNames = "";

			float lineLength = line.GetComponent<LineObject>().GetLength();
			float subDistance = lineLength / numberOfDivisions;

			List<GameObject> createdPoints = new List<GameObject>();
			createdPoints.Add(line.GetComponent<LineObject>().point2);

			for(int i=1; i<numberOfDivisions; i++){
				GameObject tempPoint = Instantiate(pointPrefab, line.GetComponent<LineObject>().point2.transform.position, line.transform.GetChild(1).rotation, GetTaskTransform());
				tempPoint.name = "Point";
				
				tempPoint.transform.Translate(Vector3.up * subDistance * i, Space.Self);
				
				createdPoints.Add(tempPoint);
			}
			
			createdPoints.Add(line.GetComponent<LineObject>().point1);
			
			Material mat = line.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
			
			List<GameObject> connectedAngles = new List<GameObject>(line.GetComponent<LineObject>().connectedAngles);
			
			GameObject firstLine = null;
			GameObject lastLine = null;
			
			List<GameObject> newPoints = new List<GameObject>();
			
			for(int i=0; i<numberOfDivisions; i++){
				
				GameObject newLine = BuildLine(createdPoints[i], createdPoints[i+1]);
				newLine.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat;
				
				newPoints.Add(createdPoints[i]);
				
				if(i == 0)firstLine = newLine;
				else if(i == numberOfDivisions-1)lastLine = newLine;
			}
			
			NamePoints();
			
			for(int i=1; i<createdPoints.Count-1; i++){
				pointsNames += createdPoints[i].GetComponent<PointObject>().GetText() + "_";
			}
			
			List<GameObject> switchList = new List<GameObject>();
			
			foreach(GameObject connectedAngle in connectedAngles){
				GameObject closerLine = Vector3.Distance(connectedAngle.transform.position, firstLine.transform.position) > Vector3.Distance(connectedAngle.transform.position, lastLine.transform.position) ? lastLine : firstLine;
				
				switchList.Add(connectedAngle);
				switchList.Add(line);
				switchList.Add(closerLine);
			}
			
			for(int i=0; i<switchList.Count; i += 3){
				switchList[i].GetComponent<AngleObject>().SwitchLine(switchList[i+1], switchList[i+2]);
			}
	
			AddCommand("SPLIT_" + line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() + "_" + line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() + "_" + pointsNames.Substring(0, pointsNames.Length-1));
	
			Destroy(line);
			
			Vibrate();
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
		
		if(newCount < 2)newCount = 2;
		else if(newCount > 10)newCount = 10;
		
		numberOfDivisions = newCount;
		
		textObject.GetComponent<TextMesh>().text = newCount.ToString();
	}
}
