using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : Tool {
	public static string text = "";
	public GameObject textField;
	public bool isEnter;
	public bool isBackspace;

	public override void Initiate(){
		if(isEnter){
			
			List<GameObject> objects = GetObjects("Point", true);
			
			if(objects.Count > 1){
				ReportMessage("ERROR: Select only one point", 3);
			}else if(objects.Count == 0){
				ReportMessage("ERROR: Select a point", 3);
			}else{
				foreach(GameObject obj in GetObjects("Point", false)){
					if(obj.GetComponent<CreatedObject>().GetText() == text){
						ReportMessage("ERROR: Point with name \"" + text + "\" already exists", 3);
						return;
					}
				}
				
				objects[0].GetComponent<CreatedObject>().AddText(text);
				objects[0].GetComponent<CreatedObject>().SelectClick();
				
				text = "";
			}
		}else if(isBackspace){
			if(text.Length != 0){
				text = text.Substring(0, text.Length - 1);
			}
		}else{
			if(text.Length <= 3){
				text += gameObject.name;
			}
		}
		
		textField.GetComponent<TextMesh>().text = text;
	}
}
