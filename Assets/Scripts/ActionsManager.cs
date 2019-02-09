using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsManager : Tool {
	
	private static List<string> commands = null;

	// Use this for initialization
	void Start () {
		commands = new List<string>();
	}
	
	public override void Initiate(){
		if(commands.Count == 0){
			Debug.Log("NO MORE");
			return;
		}
		
		Revert(commands[commands.Count - 1]);
		commands.RemoveAt(commands.Count - 1);
	}
	
	// LINE_[p1Name]_[p2Name]
	// ANGLE_[p1Name]_[pCName]_[p2Name]
	
	public void AddCommand(string command){
		if(command != "LINE__"){
			commands.Add(command);
			Debug.Log(command);
		}
	}
	
	private void Revert(string command){
		string[] commandArray = command.Split('_');
		
		if(commandArray[0] == "LINE"){
			
			GameObject line = FindLine(commandArray[1], commandArray[2]);
			
			if(line != null){
				Destroy(line);
			}else{
				//ERR
			}
		}else if(commandArray[0] == "ANGLE"){
			
			GameObject angle = FindAngle(commandArray[1], commandArray[2], commandArray[3]);
			
			if(angle != null){
				Destroy(angle);
			}else{
				//ERR
			}
		}else if(commandArray[0] == "COLOR"){
			string[] fromColor = commandArray[1].Split(',');
			string[] toColor = commandArray[2].Split(',');
			
			string p1Name = commandArray[3];
			GameObject obj = null;
			
			if(commandArray.Length == 4){
				//point
				foreach(GameObject point in GetObjects("Point", false)){
					if(point.GetComponent<PointObject>().GetText() == commandArray[3]){
						obj = point;
						break;
					}
				}
			}else if(commandArray.Length == 5){
				//line
				obj = FindLine(commandArray[3], commandArray[4]);
				obj = obj.transform.GetChild(1).gameObject;
			}else if(commandArray.Length == 6){
				//angle
				obj = FindAngle(commandArray[3], commandArray[4], commandArray[5]);
			}
			
			obj.GetComponent<Renderer>().material.color = new Color(float.Parse(fromColor[0]), float.Parse(fromColor[1]), float.Parse(fromColor[2]));
		}else if(commandArray[0] == "STYLE"){
			GameObject line = FindLine(commandArray[3], commandArray[4]);
			
			if(!line.GetComponent<CreatedObject>().isSelected)line.GetComponent<CreatedObject>().SelectClick();
			
			
			if(commandArray[1] == "SOLID"){
				GameObject.Find("Solid").GetComponent<ChangeLineType>().Lock();
				GameObject.Find("Solid").GetComponent<ChangeLineType>().Initiate();
			}else if(commandArray[1] == "DASHED"){
				GameObject.Find("Dashed").GetComponent<ChangeLineType>().Lock();
				GameObject.Find("Dashed").GetComponent<ChangeLineType>().Initiate();
			}else if(commandArray[1] == "TRANSPARENT"){
				GameObject.Find("Transparent").GetComponent<ChangeLineType>().Lock();
				GameObject.Find("Transparent").GetComponent<ChangeLineType>().Initiate();
			}
		}else if(commandArray[0] == "POLY"){
			int sides = commandArray.Length - 1;
			
			List<string> pointNames = new List<string>();
			
			for(int i=1; i<=sides; i++){
				pointNames.Add(commandArray[i]);
			}
			
			foreach(string pointName in pointNames){
				Destroy(FindPoint(pointName));
			}
		}
	}
	
	private GameObject FindPoint(string name){
		GameObject foundPoint = null;
		
		foreach(GameObject point in GetObjects("Point", false)){
			if(point.GetComponent<PointObject>().GetText() == name){
				foundPoint = point;
				break;
			}
		}
		
		return foundPoint;
	}
	
	private GameObject FindLine(string firstPoint, string secondPoint){
		GameObject p1 = null;
		GameObject p2 = null;
		GameObject line = null;
		
		foreach(GameObject point in GetObjects("Point", false)){
			if(point.GetComponent<PointObject>().GetText() == firstPoint){
				p1 = point;
			}else if(point.GetComponent<PointObject>().GetText() == secondPoint){
				p2 = point;
			}
		}
		
		if(p1 != null && p2 != null){
			foreach(GameObject connectedLine in p1.GetComponent<PointObject>().lines){
				if((connectedLine.GetComponent<LineObject>().point1 == p1 && connectedLine.GetComponent<LineObject>().point2 == p2) || 
				   (connectedLine.GetComponent<LineObject>().point2 == p1 && connectedLine.GetComponent<LineObject>().point1 == p2)){
					   line = connectedLine;
					   break;
				   }
			}
		}else{
			//SOME ERROR WTF DDZ
		}
		
		return line;
	}
	
	private GameObject FindAngle(string firstPoint, string centerPoint, string secondPoint){
		GameObject l1 = null;
		GameObject l2 = null;
		GameObject angle = null;
		
		foreach(GameObject line in GetObjects("Line", false)){

			if((line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() == firstPoint && line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() == centerPoint) || 
			   (line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() == firstPoint && line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() == centerPoint)){
				l1 = line;
			}else if((line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() == secondPoint && line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() == centerPoint) || 
					 (line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() == secondPoint && line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() == centerPoint)){
						l2 = line;
			}
			
		}
			
		if(l1 != null && l2 != null){
			foreach(GameObject connectedAngle in l1.GetComponent<LineObject>().connectedAngles){
				if((connectedAngle.GetComponent<AngleObject>().line1 == l1 && connectedAngle.GetComponent<AngleObject>().line2 == l2) || 
				   (connectedAngle.GetComponent<AngleObject>().line1 == l2 && connectedAngle.GetComponent<AngleObject>().line2 == l1)){
					  angle = connectedAngle;
					  break;
				  }
			}
		}else{
			//ERR
		}
		
		return angle;
	}
}
