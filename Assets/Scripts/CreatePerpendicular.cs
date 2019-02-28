//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: CreatePerpendicular.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Разделяне на линия на няколко равни части
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePerpendicular : CreateLine {

	public GameObject pointPrefab;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Проверява дали са селектирани една линия и една точка и вика
	//	BuildPerpendicular с тях. Иначе връща грешка до потребителя
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		List<GameObject> points = GetObjects("Point", true);
		List<GameObject> lines = GetObjects("Line", true);
		if(points.Count != 1 || lines.Count != 1){
			ReportMessage("Select one point and one line");
		}else{
			BuildPerpendicular(points[0], lines[0]);
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: BuildPerpendicular
	// Проверява дали са селектирани една линия и една точка и вика
	//	BuildPerpendicular с тях. Иначе връща грешка до потребителя
	// ПАРАМЕТРИ:
	// - GameObject point : Точката, от която ще бъде спуснат перпендикуляра
	// - GameObject line : Линията, към която ще бъде спуснат перпендикуляра
	//------------------------------------------------------------------------
	private void BuildPerpendicular(GameObject point, GameObject line){
		float xa = line.GetComponent<LineObject>().point1.transform.position.x;
		float ya = line.GetComponent<LineObject>().point1.transform.position.y;
		float za = line.GetComponent<LineObject>().point1.transform.position.z;
		float xb = line.GetComponent<LineObject>().point2.transform.position.x;
		float yb = line.GetComponent<LineObject>().point2.transform.position.y; 
		float zb = line.GetComponent<LineObject>().point2.transform.position.z;
		float xo = point.transform.position.x;
		float yo = point.transform.position.y;
		float zo = point.transform.position.z;
		
		float lambda = (xo*xb-xa*xb+xa*xa-xa*xo-ya*yb+yo*yb+ya*ya-ya*yo-za*zb+zo*zb+za*za-za*zo)/
				  (xb*xb-2*xa*xb+xa*xa+yb*yb-2*ya*yb+ya*ya+zb*zb-2*za*zb+za*za);
		
		GameObject newPoint = Instantiate(pointPrefab, new Vector3(xa+lambda*(xb-xa), 
																   ya+lambda*(yb-ya), 
																   za+lambda*(zb-za)), Quaternion.identity, GetTaskTransform());
		newPoint.name = "Point";
		
		BuildLine(newPoint, point);
		
		line.GetComponent<LineObject>().SelectClick();
		point.GetComponent<PointObject>().SelectClick();
		
		GameObject pointOne = line.GetComponent<LineObject>().point1;
		GameObject pointTwo = line.GetComponent<LineObject>().point2;
		
		if(IsBetween(pointTwo.transform.position, pointOne.transform.position, newPoint.transform.position)){
			Material mat = line.transform.GetChild(1).gameObject.GetComponent<Renderer>().material;
			
			GameObject newLine1 = BuildLine(pointOne, newPoint);
			newLine1.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = mat;
			
			GameObject newLine2 = BuildLine(newPoint, pointTwo);
			newLine2.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = mat;
			
			List<GameObject> connectedAngles = new List<GameObject>(line.GetComponent<LineObject>().connectedAngles);
			
			List<GameObject> switchList = new List<GameObject>();
			
			
			foreach(GameObject connectedAngle in connectedAngles){
				GameObject closerLine = Vector3.Distance(connectedAngle.transform.position, newLine1.transform.position) > Vector3.Distance(connectedAngle.transform.position, newLine2.transform.position) ? newLine2 : newLine1;
				
				switchList.Add(connectedAngle);
				switchList.Add(line);
				switchList.Add(closerLine);
			}
			
			for(int i=0; i<switchList.Count; i += 3){
				switchList[i].GetComponent<AngleObject>().SwitchLine(switchList[i+1], switchList[i+2]);
			}
			
			Destroy(line);
		}else{
			if(Vector3.Distance(point.transform.position, pointOne.transform.position) < Vector3.Distance(point.transform.position, pointTwo.transform.position)){
				BuildLine(newPoint, pointOne);
			}else{
				BuildLine(newPoint, pointTwo);
			}
		}
		
		Vibrate();
		
		NamePoints();
		AddCommand("PERP_"+point.GetComponent<PointObject>().GetText()+"_"+newPoint.GetComponent<PointObject>().GetText()+"_"+line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText()+"_"+line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText());
	}
}
