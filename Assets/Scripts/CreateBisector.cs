//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: CreateBisector.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Построяване на ъглополовяща
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateBisector : CreateLine{

	public GameObject pointPrefab;
	public GameObject anglePrefab;

	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Проверява за правилно селектирани обекти и построява ъглополовяща
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Initiate(){
		
		// Проверка за селектиран поне един ъгъл
		List<GameObject> angles = GetObjects("Angle", true);
		
		if(angles.Count < 1){
			ReportMessage("ERROR: Select at least one angle object");
		}else{
			foreach(GameObject angle in angles){
				
				// Запазват се линиите на всеки ъгъл
				GameObject line1 = angle.GetComponent<AngleObject>().line1;
				GameObject line2 = angle.GetComponent<AngleObject>().line2;
				
				// Ъгълът бива разкачен от линиите му
				line1.GetComponent<LineObject>().DisconnectAngle(angle);
				line2.GetComponent<LineObject>().DisconnectAngle(angle);
				
				// Запазват се трите точки, съставляващи свързаните линии
				GameObject c = null, b = null, a = null;
				
				if(line1.GetComponent<LineObject>().point1 == line2.GetComponent<LineObject>().point1){
					a = line1.GetComponent<LineObject>().point1;
					c = line1.GetComponent<LineObject>().point2;
					b = line2.GetComponent<LineObject>().point2;
				}else if(line1.GetComponent<LineObject>().point1 == line2.GetComponent<LineObject>().point2){
					a = line1.GetComponent<LineObject>().point1;
					c = line1.GetComponent<LineObject>().point2;
					b = line2.GetComponent<LineObject>().point1;
				}else if(line1.GetComponent<LineObject>().point2 == line2.GetComponent<LineObject>().point1){
					a = line1.GetComponent<LineObject>().point2;
					c = line1.GetComponent<LineObject>().point1;
					b = line2.GetComponent<LineObject>().point2;
				}else if(line1.GetComponent<LineObject>().point2 == line2.GetComponent<LineObject>().point2){
					a = line1.GetComponent<LineObject>().point2;
					c = line1.GetComponent<LineObject>().point1;
					b = line2.GetComponent<LineObject>().point1;
				}
				
				// Изчисляват се размерите на линиите
				float ac = line1.GetComponent<LineObject>().GetLength();
				float ab = line2.GetComponent<LineObject>().GetLength();
				float cb = Vector3.Distance(c.transform.position, b.transform.position);
				
				// Намиране на дължината от точката до т.C
				float cl = (ac*cb)/(ab+ac);
				
				// Създаване на новата точка на позицията на т.B
				GameObject point = Instantiate(pointPrefab, c.transform.position, Quaternion.identity, GetTaskTransform());
				point.name = "Point";
				
				// Транслиране на точката по BC на съответното разстояние
				point.transform.LookAt(b.transform);
				point.transform.Translate(Vector3.forward * cl, Space.Self);
				
				// Построяване на самата ъглополовяща
				GameObject newLine = BuildLine(a, point);
				
				// Ъпдейтване на размерите на ъгъла и свързването му за новите линии
				angle.GetComponent<AngleObject>().Connect(line1, newLine);
				angle.GetComponent<AngleObject>().UpdateAngle(line1, newLine);
				
				// Деселектиране на ъгъла
				angle.GetComponent<CreatedObject>().SelectClick();
				
				// Създаване на втория ъгъл
				GameObject createdAngle = Instantiate(anglePrefab, newLine.transform.position, Quaternion.identity, GetTaskTransform());
				createdAngle.name = "Angle";
				
				// Всързване и оразмеряване на втория ъгъл
				createdAngle.GetComponent<AngleObject>().Connect(line2, newLine);
				createdAngle.GetComponent<AngleObject>().UpdateAngle(newLine, line2);
				
				// Запазване на всички линии свързани за т.C и т.B
				List<GameObject> lines = new List<GameObject>(c.GetComponent<PointObject>().lines);
				foreach(GameObject lineb in b.GetComponent<PointObject>().lines){
					if(!lines.Contains(lineb)){
						lines.Add(lineb);
					}
				}
				
				// Откриване на линията между т.C и т.B
				GameObject cbLine = null;
				foreach(GameObject connectedLine in c.GetComponent<PointObject>().lines){
					if(connectedLine.GetComponent<LineObject>().point2 == b || connectedLine.GetComponent<LineObject>().point1 == b){
						cbLine = connectedLine;
						break;
					}
				}
				
				// Ако има такава, построяват се нови све линии, които да я заменят
				if(cbLine != null){
					GameObject clLine = BuildLine(c, point);
					GameObject lbLine = BuildLine(b, point);
					
					// Запазват се вече съществуващите ъгли, свързани за линията между т.C и т.B
					// Както и линията, за която трябва да бъдат свързани
					List<GameObject> switchList = new List<GameObject>();
					
					foreach(GameObject connectedLine in lines){
						if(connectedLine != line1 && connectedLine != line2){
							foreach(GameObject connectedAngle in connectedLine.GetComponent<LineObject>().connectedAngles){
								GameObject closerLine = Vector3.Distance(connectedAngle.transform.position, clLine.transform.position) > Vector3.Distance(connectedAngle.transform.position, lbLine.transform.position) ? lbLine : clLine;
								
								switchList.Add(connectedAngle);
								switchList.Add(connectedLine);
								switchList.Add(closerLine);
							}
						}
					}
					
					// Сменят се свързаните линии на всеки ъгъл
					for(int i=0; i<switchList.Count; i += 3){
						switchList[i].GetComponent<AngleObject>().SwitchLine(switchList[i+1], switchList[i+2]);
					}
					
					// Изтрива се линията
					Destroy(cbLine);
				}
				
				NamePoints();
				
				// NAME THE NEW POINT YOU MONGOLIAN
				AddCommand("BISECTOR_"+c.GetComponent<PointObject>().GetText()+"_"+a.GetComponent<PointObject>().GetText()+"_"+b.GetComponent<PointObject>().GetText()+"_"+point.GetComponent<PointObject>().GetText());
			}
		}
	}
}
