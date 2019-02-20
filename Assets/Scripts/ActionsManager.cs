using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class ActionsManager : Tool {
	
	public static List<string> commands = null;

	// Use this for initialization
	
	public void Start() {
		if(commands == null){
			commands = new List<string>();
		}
	}
	
	public override void Initiate(){
		if(commands.Count == 0){
			Debug.Log("NO MORE");
			return;
		}
		string command = commands[commands.Count - 1];
		commands.RemoveAt(commands.Count - 1);
		Revert(command);
	}
	
	public void AddCommand(string command){
		
		if(command.Substring(0,4) == "LINE"){
			if(!Regex.IsMatch(command, "LINE_[a-z]_[a-z]", RegexOptions.IgnoreCase)){
				return;
			}
		}
		commands.Add(command);
		
		Debug.Log(command + ">" + commands.Count + "<");
	}
	
	private void Revert(string command){
		
		Debug.Log("REVERT>>>"+command);
		
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
		}else if(commandArray[0] == "CIRCLE"){
			string centerPoint = commandArray[1];
			string linePoint = commandArray[2];
			string anglePoint = commandArray[3];
			
			GameObject point = FindPoint(centerPoint);
			GameObject line = FindLine(centerPoint, linePoint);
			GameObject angle = FindAngle(linePoint, centerPoint, anglePoint);
			
			GameObject circleObject = null;
			
			foreach(GameObject circle in GetObjects("Circle", false)){
				if(circle.GetComponent<CircleObject>().Check(point, line, angle)){
					circleObject = circle;
				}
			}
			
			Destroy(circleObject);
		}else if(commandArray[0] == "PERP"){
			string oldPoint = commandArray[1];
			string newPoint = commandArray[2];
			string firstPoint = commandArray[3];
			string secondPoint = commandArray[4];
			
			Destroy(FindLine(oldPoint, newPoint));
			
			GameObject smallLine = FindLine(newPoint, secondPoint);
			
			Destroy(smallLine);
			
			GameObject line = FindLine(firstPoint, newPoint);
			
			GameObject pointF = FindPoint(firstPoint);
			GameObject pointN = FindPoint(newPoint);
			GameObject pointS = FindPoint(secondPoint);
			
			GameObject bigLine = FindLine(newPoint, secondPoint);
			
			pointS.GetComponent<PointObject>().Disconnect(bigLine);
			pointN.GetComponent<PointObject>().Disconnect(bigLine);
			pointN.GetComponent<PointObject>().Disconnect(line);
			
			line.GetComponent<LineObject>().SetPoints(pointF, pointS);
			
			line.GetComponent<LineObject>().UpdatePosition(pointF.transform.position, pointS.transform.position);
		}else if(commandArray[0] == "BISECTOR"){
			string pointOne = commandArray[1];
			string pointCenter = commandArray[2];
			string pointTwo = commandArray[3];
			string pointBisector = commandArray[4];
			
			GameObject lineOne = FindLine(pointOne, pointCenter);
			GameObject lineTwo = FindLine(pointTwo, pointCenter);
			
			GameObject lineBisector = FindLine(pointCenter, pointBisector);
			
			GameObject firstAngle = FindAngle(pointOne, pointCenter, pointBisector);
			
			GameObject secondAngle = FindAngle(pointBisector, pointCenter, pointTwo);
			
			firstAngle.GetComponent<AngleObject>().Connect(lineOne, lineTwo);
			firstAngle.GetComponent<AngleObject>().UpdateAngle(lineOne, lineTwo);
			
			Destroy(secondAngle);
			
			lineBisector.GetComponent<LineObject>().DisconnectAngle(firstAngle);
			Destroy(lineBisector);
			
			GameObject pointOneObj = FindPoint(pointOne);
			GameObject pointTwoObj = FindPoint(pointTwo);
			
			GameObject lineLeft = FindLine(pointOne, pointBisector);
			GameObject lineRight = FindLine(pointOne, pointBisector);
			
			lineLeft.GetComponent<LineObject>().SetPoints(pointOneObj, pointTwoObj);
			lineLeft.GetComponent<LineObject>().UpdatePosition(pointOneObj.transform.position, pointTwoObj.transform.position);
			
			GameObject pointBisectorObj = FindPoint(pointBisector);
			
			pointBisectorObj.GetComponent<PointObject>().Disconnect(lineLeft);
			
			Destroy(pointBisectorObj);
		}else if(commandArray[0] == "EXTRUDE"){
			int pointCount = int.Parse(commandArray[1]);
			
			if(commandArray[2] == "PRISM")pointCount *= 2;
			
			for(int i=0; i<pointCount; i++){
				Initiate();
			}
		}else if(commandArray[0] == "EXPAND"){
			if(commandArray[3] == "OUT"){
				Destroy(FindPoint(commandArray[4]));
			}else if(commandArray[3] == "IN"){
				GameObject line1 = FindLine(commandArray[1], commandArray[4]);
				GameObject line2 = FindLine(commandArray[2], commandArray[4]);
				
				Destroy(line2);
				
				GameObject newPoint1 = FindPoint(commandArray[1]);
				GameObject newPoint2 = FindPoint(commandArray[2]);
				
				line1.GetComponent<LineObject>().UpdatePosition(newPoint1.transform.position, newPoint2.transform.position);
			
				GameObject midPoint = FindPoint(commandArray[4]);
				
				midPoint.GetComponent<PointObject>().Disconnect(line1);
				midPoint.GetComponent<PointObject>().Disconnect(line2);
			
				Destroy(midPoint);
			}
			
			//FINISH THIS SHIT DEEBA
		}else if(commandArray[0] == "SPLIT"){
			GameObject line = FindLine(commandArray[1], commandArray[2]);
			
			int newPoints = commandArray.Length - 3;
			
			GameObject firstLine = FindLine(commandArray[2], commandArray[3]);
			
			//FindPoint(commandArray[3]).GetComponent<PointObject>().Disconnect(firstLine);
			//FindPoint(commandArray[2]).GetComponent<PointObject>().Disconnect(firstLine);
			
			FindPoint(commandArray[3]).GetComponent<PointObject>().Disconnect(firstLine);
			
			firstLine.GetComponent<LineObject>().UpdatePosition(FindPoint(commandArray[1]).transform.position, FindPoint(commandArray[2]).transform.position);
			firstLine.GetComponent<LineObject>().SetPoints(FindPoint(commandArray[1]), FindPoint(commandArray[2]));
			
			for(int i=3; i<commandArray.Length; i++){
				Destroy(FindPoint(commandArray[i]));
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
