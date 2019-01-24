using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : CreatedObject {

	public List<GameObject> lines;
	
	public void Start(){
		if(lines == null)lines = new List<GameObject>();
		
		StartCoroutine(checkOverlap());
	}
	
	//return to Start()
	private IEnumerator checkOverlap(){
		yield return new WaitForSeconds(.5f);
		
		Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, gameObject.transform.localScale.x);
		
		List<GameObject> transferedLines = new List<GameObject>();
		
		foreach(Collider collider in hitColliders){
			if(collider.name == "Point" && collider.gameObject != gameObject){
				foreach(GameObject line in collider.gameObject.GetComponent<PointObject>().lines){
					transferedLines.Add(line);
				}
				
				foreach(GameObject line in transferedLines){
					collider.gameObject.GetComponent<PointObject>().Disconnect(line);
					lines.Add(line);
				}
				Destroy(collider.gameObject);
			}
		}
	}

	public bool ConnectedTo(GameObject givenLine){
		foreach(GameObject line in lines){
			if(givenLine == line){
				return true;
			}
		}
		return false;
	}
	
	public void Connect(GameObject givenLine){
		lines.Add(givenLine);
	}
	
	public void Disconnect(GameObject givenLine){
		lines.Remove(givenLine);
	}
	
	public void AddText(string s){
		gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = s;
	}
	
	public string GetText(){
		return gameObject.transform.GetChild(0).GetComponent<TextMesh>().text;
	}
}
