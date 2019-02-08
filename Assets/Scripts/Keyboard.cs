using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Keyboard : Tool {
	public static string text = "";
	public GameObject textField;
	public bool isEnter;
	public bool isBackspace;

	public override void Initiate(){
		if(isEnter){
			if(Regex.IsMatch(text, "[0-9]", RegexOptions.IgnoreCase) && !Regex.IsMatch(text, "[a-z]", RegexOptions.IgnoreCase)){
				Debug.Log("cifra");
				
				List<GameObject> lines = GetObjects("Line", true);
				List<GameObject> angles = GetObjects("Angle", true);
			
				if(lines.Count > 0){
					foreach(GameObject line in lines){
						line.GetComponent<LineObject>().AddText(text);
						line.GetComponent<CreatedObject>().SelectClick();
					}
				}else if(angles.Count > 0){
					foreach(GameObject angle in angles){
						// addtext method prolly
						angle.GetComponent<AngleObject>().AddText(text);
						//angle.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = text;
						angle.GetComponent<CreatedObject>().SelectClick();
					}
				}else{
					ReportMessage("ERROR: Select an object", 3);
				}
				/*!Regex.IsMatch(text, "[0-9]", RegexOptions.IgnoreCase)*/
			}else if(Regex.IsMatch(text, "[a-z]", RegexOptions.IgnoreCase) && 
					(!Regex.IsMatch(text.Substring(1), "[a-z]", RegexOptions.IgnoreCase) || text.Length == 1)){
				Debug.Log("bukfa");
				
				List<GameObject> objects = GetObjects("Point", true);
			
				if(objects.Count > 1){
					ReportMessage("ERROR: Select only one point", 3);
				}else if(objects.Count == 0){
					ReportMessage("ERROR: Select a point", 3);
				}else{
					foreach(GameObject obj in GetObjects("Point", false)){
						if(obj.GetComponent<PointObject>().GetText() == text){
							ReportMessage("ERROR: Point with name \"" + text + "\" already exists", 3);
							return;
						}
					}
					
					objects[0].GetComponent<PointObject>().AddText(text);
					objects[0].GetComponent<CreatedObject>().SelectClick();
					
					text = "";
				}
			}else{
				ReportMessage("ERROR: Invalid name", 3);
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
