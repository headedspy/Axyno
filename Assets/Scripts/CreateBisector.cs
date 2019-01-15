using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateBisector : CreateLine{

	public GameObject pointPrefab;
	public GameObject anglePrefab;

	public void Initiate(){
		List<GameObject> angles = GetObjects("Angle", true);
		
		if(angles.Count < 1){
			ReportMessage("ERROR: Select at least one angle object", 3);
		}else{
			foreach(GameObject angle in angles){
				GameObject line1 = angle.GetComponent<AngleObject>().line1;
				GameObject line2 = angle.GetComponent<AngleObject>().line2;
				
				line1.GetComponent<LineObject>().DisconnectAngle(angle);
				line2.GetComponent<LineObject>().DisconnectAngle(angle);
				
				
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
				
				float ac = line1.GetComponent<LineObject>().GetLength();
				float ab = line2.GetComponent<LineObject>().GetLength();
				float cb = Vector3.Distance(c.transform.position, b.transform.position);
				
				float cl = (ac*cb)/(ab+ac);
				
				GameObject point = Instantiate(pointPrefab, c.transform.position, Quaternion.identity, GetTaskTransform());
				point.name = "Point";
				
				point.transform.LookAt(b.transform);
				point.transform.Translate(Vector3.forward * cl, Space.Self);
				
				GameObject newLine = BuildLine(a, point);
				
				
				angle.GetComponent<AngleObject>().Connect(line1, newLine);
				angle.GetComponent<AngleObject>().UpdateAngle(line1, newLine);
				
				angle.GetComponent<CreatedObject>().SelectClick();
				
				GameObject createdAngle = Instantiate(anglePrefab, newLine.transform.position, Quaternion.identity, GetTaskTransform());
				createdAngle.name = "Angle";
				
				createdAngle.GetComponent<AngleObject>().Connect(line2, newLine);
				createdAngle.GetComponent<AngleObject>().UpdateAngle(newLine, line2);
				
				
				
				
				List<GameObject> lines = new List<GameObject>(c.GetComponent<PointObject>().lines);
				
				foreach(GameObject lineb in b.GetComponent<PointObject>().lines){
					if(!lines.Contains(lineb)){
						lines.Add(lineb);
					}
				}
				
				GameObject cbLine = null;
				
				foreach(GameObject connectedLine in c.GetComponent<PointObject>().lines){
					if(connectedLine.GetComponent<LineObject>().point2 == b || connectedLine.GetComponent<LineObject>().point1 == b){
						cbLine = connectedLine;
						break;
					}
				}
				
				if(cbLine != null){
					GameObject clLine = BuildLine(c, point);
					GameObject lbLine = BuildLine(b, point);
					
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
					
					for(int i=0; i<switchList.Count; i += 3){
						switchList[i].GetComponent<AngleObject>().SwitchLine(switchList[i+1], switchList[i+2]);
					}
					
					Destroy(cbLine);
				}
			}
		}
	}
}
