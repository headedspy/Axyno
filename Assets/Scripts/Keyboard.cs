//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: Keyboard.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Именуване на точки и задаване на репрезентативни
//  стойности на линии и ъгли чрез клавиатурата
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Keyboard : ActionsManager {
	public static string text = "";
	public GameObject textField;
	public bool isEnter;
	public bool isBackspace;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Проверява дали е въведено име или стойност и я записва на 
	// съответния селектиран обект
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		if(isEnter){
			if(Regex.IsMatch(text, "[0-9]", RegexOptions.IgnoreCase) && !Regex.IsMatch(text, "[a-z]", RegexOptions.IgnoreCase)){
				Debug.Log("cifra");
				
				if(float.Parse(text) == 0f){
					ReportMessage("ERROR: Value cannot be zero");
					return;
				}
				
				List<GameObject> lines = GetObjects("Line", true);
				List<GameObject> angles = GetObjects("Angle", true);
			
				if(lines.Count > 0){
					foreach(GameObject line in lines){
						line.GetComponent<LineObject>().AddText(text);
						line.GetComponent<CreatedObject>().SelectClick();
					}
				}else if(angles.Count > 0){
					foreach(GameObject angle in angles){
						angle.GetComponent<AngleObject>().AddText(text);
						angle.GetComponent<CreatedObject>().SelectClick();
					}
				}else{
					ReportMessage("ERROR: Select an object");
				}
			}else if(Regex.IsMatch(text, "[a-z]", RegexOptions.IgnoreCase) && 
					(!Regex.IsMatch(text.Substring(1), "[a-z]", RegexOptions.IgnoreCase) || text.Length == 1)){
				Debug.Log("bukfa");
				
				List<GameObject> objects = GetObjects("Point", true);
			
				if(objects.Count > 1){
					ReportMessage("ERROR: Select only one point");
				}else if(objects.Count == 0){
					ReportMessage("ERROR: Select a point");
				}else{
					foreach(GameObject obj in GetObjects("Point", false)){
						if(obj.GetComponent<PointObject>().GetText() == text){
							ReportMessage("ERROR: Point with name \"" + text + "\" already exists");
							return;
						}
					}
					
					objects[0].GetComponent<PointObject>().AddText(text);
					objects[0].GetComponent<CreatedObject>().SelectClick();
					
					text = "";
				}
			}else{
				ReportMessage("ERROR: Invalid name");
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
