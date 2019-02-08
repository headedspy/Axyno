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
		Revert(commands[commands.Count - 1]);
		commands.RemoveAt(commands.Count - 1);
	}
	
	// LINE_[p1Name]_[p2Name]
	// ANGLE_[p1Name]_[pCName]_[p2Name]
	
	public void AddCommand(string command){
		commands.Add(command);
		Debug.Log(command);
	}
	
	private void Revert(string command){
		string[] commandArray = command.Split('_');
		
		if(commandArray[0] == "LINE"){
			GameObject p1 = null;
			GameObject p2 = null;
			string p1Name = commandArray[1];
			string p2Name = commandArray[2];
			
			foreach(GameObject point in GetObjects("Point", false)){
				if(point.GetComponent<PointObject>().GetText() == p1Name){
					p1 = point;
				}else if(point.GetComponent<PointObject>().GetText() == p2Name){
					p2 = point;
				}
			}
			
			if(p1 != null && p2 != null){
				GameObject line = null;
				
				foreach(GameObject connectedLine in p1.GetComponent<PointObject>().lines){
					if((connectedLine.GetComponent<LineObject>().point1 == p1 && connectedLine.GetComponent<LineObject>().point2 == p2) || 
					   (connectedLine.GetComponent<LineObject>().point2 == p1 && connectedLine.GetComponent<LineObject>().point1 == p2)){
						   line = connectedLine;
						   break;
					   }
				}
				
				if(line != null){
					Destroy(line);
				}else{
					//ERR
				}
			}else{
				//SOME ERROR WTF DDZ
			}
		}else if(commandArray[0] == "ANGLE"){
			GameObject l1 = null;
			GameObject l2 = null;
			
			string p1Name = commandArray[1];
			string pCName = commandArray[2];
			string p2Name = commandArray[3];
			
			foreach(GameObject line in GetObjects("Line", false)){

				if((line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() == p1Name && line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() == pCName) || 
				   (line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() == p1Name && line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() == pCName)){
					l1 = line;
				}else if((line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() == p2Name && line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() == pCName) || 
						 (line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText() == p2Name && line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText() == pCName)){
							l2 = line;
				}
				
			}
				
			if(l1 != null && l2 != null){
				GameObject angle = null;
				
				//FINISH FOR THE ANGLE
				foreach(GameObject connectedAngle in l1.GetComponent<LineObject>().connectedAngles){
					if((connectedAngle.GetComponent<AngleObject>().line1 == l1 && connectedAngle.GetComponent<AngleObject>().line2 == l2) || 
					   (connectedAngle.GetComponent<AngleObject>().line1 == l2 && connectedAngle.GetComponent<AngleObject>().line2 == l1)){
						  angle = connectedAngle;
						  break;
					  }
				}
				
				if(angle != null){
					Destroy(angle);
					Debug.Log("Angle");
				}else{
					//ERROR
					Debug.Log("NO ANGLE FOUND");
				}
			}
		}
	}
	
	private void RevertCreatePrimitive(){
		
	}
}
