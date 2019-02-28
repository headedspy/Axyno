//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: ToolButton.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Управление на инструментите за директна манипулация
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolButton : Tool {

	public string toolName = "";
	public bool isSelected = false;
	public List<GameObject> otherTools;
	public GameObject pointerObject;
	public GameObject captionObject;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Start
	// Инициализира списъка с другите инструменти
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Start(){
		if(otherTools == null)otherTools = new List<GameObject>();
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Селектира инструмента и деселектира останалите
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		if(!isSelected)Select();
		else{
			if(GameObject.Find("Head").GetComponent<Info>().tool == "Select" || GameObject.Find("Head").GetComponent<Info>().tool == "ShapeSelect"){
				foreach(GameObject selectedObject in GetObjects("", true)){
					selectedObject.GetComponent<CreatedObject>().SelectClick();
				}
			}
			Deselect();
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Select
	// Селектира инструмента
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	private void Select(){
		foreach(GameObject otherTool in otherTools){
			if(otherTool.GetComponent<ToolButton>().isSelected == true){
				otherTool.GetComponent<ToolButton>().Initiate();
			}
		}
		isSelected = true;
		GameObject.Find("Head").GetComponent<Info>().ChangeTool(toolName);
		gameObject.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f);
		
		Color c;
		
		if(toolName == "Select")c = new Color(0.26f, 0.23f, 0.51f);
		else if(toolName == "ShapeSelect")c = new Color(0.61f, 0.24f, 0.13f);
		else c = new Color(0.19f, 0.65f, 0.26f);
		
		captionObject.GetComponent<TextMesh>().color = c;
		pointerObject.GetComponent<Renderer>().material.color = c;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Select
	// Деселектира инструмента
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	private void Deselect(){
		isSelected = false;
		GameObject.Find("Head").GetComponent<Info>().ResetTool();
		gameObject.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f);
		
		pointerObject.GetComponent<Renderer>().material.color = new Color(0.25f, 0.25f, 0.25f);
		captionObject.GetComponent<TextMesh>().color = new Color(0.25f, 0.25f, 0.25f);
	}
}
