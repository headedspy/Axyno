using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSelection : MonoBehaviour {
	
	public GameObject task;
	public GameObject linePrefab;
	public GameObject pointPrefab;
	public GameObject anglePrefab;
	
	//private List<GameObject> points;
	
	public Texture dashedLineTexture;

	private List<GameObject> GetObjects(string name, bool selected){
		List<GameObject> list = new List<GameObject>();
		
		foreach(Transform child in task.transform){
			if(child.gameObject.name == name || name == ""){
				if(selected){
					if(child.gameObject.GetComponent<CreatedObject>().isSelected){
						list.Add(child.gameObject);
					}
				}else{
					list.Add(child.gameObject);
				}
			}
		}
		
		return list;
	}
	
	public void CreateLine(){
		List<GameObject> points = GetObjects("Point", true);
		
		if(points.Count != 2){
			//ERR: 2 points must be selected
		}else{
			BuildLine(points[0], points[1]);
			
			//deselect dots
			foreach(GameObject point in points){
				point.GetComponent<PointObject>().SelectClick();
			}
		}
	}
	
	private void BuildLine(GameObject point1, GameObject point2){
		//check if line already exists
		foreach(GameObject connectedLine in point1.GetComponent<PointObject>().lines){
			if(connectedLine.GetComponent<LineObject>().point1 == point2 || connectedLine.GetComponent<LineObject>().point2 == point2){
				//ERR: Line already exists
				return;
			}
		}
		
		//create line object
		float distance = Vector3.Distance(point1.transform.position, point2.transform.position);
		GameObject line = Instantiate(linePrefab, point1.transform.position, Quaternion.identity, task.transform);
		line.transform.LookAt(point2.transform, line.transform.up * -1);
		line.transform.Rotate(Vector3.left * 90f, Space.Self);
		line.transform.Translate(Vector3.down * (distance/2), Space.Self);
		line.transform.localScale += new Vector3(0, distance/2, 0);
		line.name = "Line";
			
		//Unity2018 bug *shrug face*
		line.GetComponent<Collider>().enabled = false;
		line.GetComponent<Collider>().enabled = true;
		//dont ask
			
		//add points to the line and vice versa
		line.GetComponent<LineObject>().SetPoints(point1, point2);
	}
	
	public void ChangeColor(){
		Color c = gameObject.GetComponent<Renderer>().material.color;
		
		foreach(GameObject obj in GetObjects("", true)){
			obj.GetComponent<CreatedObject>().ChangeColor(c);
		}
	}
	
	public void ShowColorMenu(){  //CHANGE NAME PROLLY?
		GameObject menu = gameObject.transform.GetChild(0).gameObject;
		
		if(menu.activeSelf){
			menu.SetActive(false);
		}else{
			menu.SetActive(true);
		}
	}
	
	public void ChangeTypeOfLine(){
		foreach(GameObject line in GetObjects("Line", true)){
			Color tempColor = line.GetComponent<Renderer>().material.color;
		
			//reset back to solid <<<<prolly refactor that
			line.GetComponent<Renderer>().material.mainTexture = null;
			
			if(gameObject.name == "Solid"){
				line.GetComponent<Renderer>().material.color = new Color(tempColor.r, tempColor.g, tempColor.b, 1f);
			}else if(gameObject.name == "Dashed"){
				line.GetComponent<Renderer>().material.mainTexture = dashedLineTexture;
				line.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f, line.transform.localScale.y * 10f);
			}else{ //Transparent
				line.GetComponent<Renderer>().material.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.5f);
			}
		}
	}
	public void SubdivideLines(){
		int numberOfDivisions = int.Parse(transform.parent.GetChild(3).GetChild(0).GetChild(0).GetComponent<TextMesh>().text);	

		foreach(GameObject line in GetObjects("Line", true)){

			float lineLength = Vector3.Distance(line.GetComponent<LineObject>().point1.transform.position, line.GetComponent<LineObject>().point2.transform.position);
			float subDistance = lineLength / numberOfDivisions;

			List<GameObject> createdPoints = new List<GameObject>();

			createdPoints.Add(line.GetComponent<LineObject>().point2);

			for(int i=1; i<numberOfDivisions; i++){
				GameObject tempPoint = Instantiate(pointPrefab, line.transform.position, line.transform.rotation, task.transform);
				tempPoint.transform.Translate(Vector3.down * lineLength / 2, Space.Self);
				tempPoint.name = "Point";
				createdPoints.Add(tempPoint);
					
				tempPoint.transform.Translate(Vector3.up * subDistance * i, Space.Self);
			}

			createdPoints.Add(line.GetComponent<LineObject>().point1);

			for(int i=0; i<numberOfDivisions; i++){
				BuildLine(createdPoints[i], createdPoints[i+1]);
			}
			Destroy(line);
		}
	}
	
	public void CreateAngleRepresentation(){
		List<GameObject> list = GetObjects("Line", true);
		
		if(list.Count != 2){
			//ERR HERE
		}else{
			BuildAngle(list[0], list[1]);
		}
	}
	
	private void BuildAngle(GameObject line1, GameObject line2){
		
		//is this angle existing?
		foreach(GameObject angle in GetObjects("Angle", false)){
			if((angle.GetComponent<AngleObject>().line1 == line1 && angle.GetComponent<AngleObject>().line2 == line2) ||
			   (angle.GetComponent<AngleObject>().line1 == line2 && angle.GetComponent<AngleObject>().line2 == line1)){
				   //ERR: Angle already exists
				   return;
			   }
		}
		
		GameObject point = null;
		
		if(line1.GetComponent<LineObject>().point1 == line2.GetComponent<LineObject>().point1){
			point = line1.GetComponent<LineObject>().point1;
		}else if(line1.GetComponent<LineObject>().point1 == line2.GetComponent<LineObject>().point2){
			point = line1.GetComponent<LineObject>().point1;
		}else if(line1.GetComponent<LineObject>().point2 == line2.GetComponent<LineObject>().point1){
			point = line1.GetComponent<LineObject>().point2;
		}else if(line1.GetComponent<LineObject>().point2 == line2.GetComponent<LineObject>().point2){
			point = line1.GetComponent<LineObject>().point2;
		}
		
		if(point != null){
			GameObject angle = Instantiate(anglePrefab, point.transform.position, Quaternion.identity, task.transform);
			
			GameObject p1 = line1.GetComponent<LineObject>().point1 == point ? line1.GetComponent<LineObject>().point2 : line1.GetComponent<LineObject>().point1;
			GameObject p2 = line2.GetComponent<LineObject>().point1 == point ? line2.GetComponent<LineObject>().point2 : line2.GetComponent<LineObject>().point1;
			
			angle.transform.LookAt((p1.transform.position + p2.transform.position) / 2);
			
			float oa = line1.GetComponent<LineObject>().GetLength();
			float ob = line2.GetComponent<LineObject>().GetLength();
			float ab = Vector3.Distance(p1.transform.position, p2.transform.position);
			
			float angleValue = Mathf.Acos( (oa*oa + ob*ob - ab*ab) / (2 * oa * ob) );  //Vector3.Angle mai imashe sm shit like dis
			
			angleValue *= Mathf.Rad2Deg;
			
			//32-full_torus
			angle.GetComponent<AngleObject>().BuildTorus( Mathf.RoundToInt( (4*angleValue) / 45) );
			
			angle.transform.localScale = new Vector3(1f, 0.12f, 1f);
			
			angle.name = "Angle";
			
			Vector3 side1 = p1.transform.position - point.transform.position;
			Vector3 side2 = p2.transform.position - point.transform.position;
			
			Vector3 yVector = Vector3.Cross(side1, side2);
			
			angle.transform.rotation = Quaternion.LookRotation(yVector) * Quaternion.Euler(90f, 0f, 0f);
			
			//um, okay thats here for reasons (angle does the moves thing ._.)
			angle.transform.position = point.transform.position;
			//
			
			angle.transform.LookAt(p2.transform, yVector);
			angle.transform.Rotate(Vector3.up * -90f);
			
			//connect to lines
			angle.GetComponent<AngleObject>().Connect(line1, line2);
			
			//deselect lines
			line1.GetComponent<LineObject>().SelectClick();
			line2.GetComponent<LineObject>().SelectClick();
		}else{
			//ERR: Nqa obshta tochka
		}
	}
	
	public void Perpendicular(){
		
		if(GetObjects("Line", true).Count != 1 || GetObjects("Point", true).Count != 1){
			//ERR: select 1 line & 1 point u dum fuk
			return;
		}
		
		//dont like how u call the same f() twice tho
		GameObject line = GetObjects("Line", true)[0];
		GameObject point = GetObjects("Point", true)[0];
		
		float xa = line.GetComponent<LineObject>().point1.transform.position.x;
		float ya = line.GetComponent<LineObject>().point1.transform.position.y;
		float za = line.GetComponent<LineObject>().point1.transform.position.z;
		float xb = line.GetComponent<LineObject>().point2.transform.position.x;
		float yb = line.GetComponent<LineObject>().point2.transform.position.y; 
		float zb = line.GetComponent<LineObject>().point2.transform.position.z;
		float xo = point.transform.position.x;
		float yo = point.transform.position.y;
		float zo = point.transform.position.z;
		
		float l = (xo*xb-xa*xb+xa*xa-xa*xo-ya*yb+yo*yb+ya*ya-ya*yo-za*zb+zo*zb+za*za-za*zo)/
				  (xb*xb-2*xa*xb+xa*xa+yb*yb-2*ya*yb+ya*ya+zb*zb-2*za*zb+za*za);
		
		GameObject newPoint = Instantiate(pointPrefab, new Vector3(xa+l*(xb-xa), ya+l*(yb-ya), za+l*(zb-za)), Quaternion.identity, task.transform);
		
		newPoint.name = "Point";
		
		BuildLine(newPoint, point);
		
		line.GetComponent<LineObject>().SelectClick();
		point.GetComponent<PointObject>().SelectClick();
		
		GameObject pointOne = line.GetComponent<LineObject>().point1;
		GameObject pointTwo = line.GetComponent<LineObject>().point2;
		
		Destroy(line);
		
		BuildLine(pointOne, newPoint);
		BuildLine(newPoint, pointTwo);
	}
	
	public void ChangeCount(int num){
		   //button          background     text
		GameObject textObj = gameObject.transform.GetChild(0).GetChild(0).gameObject;
		
		int newCount = int.Parse(textObj.GetComponent<TextMesh>().text) + num;
		
		if(newCount < 2)newCount = 2;
		else if(newCount > 10)newCount = 10;
		
		textObj.GetComponent<TextMesh>().text = newCount.ToString();
	}
	
	//TO-DO
	private void PrintMessage(int type){ //1-info 2-warning 3-error
		
	}
}
