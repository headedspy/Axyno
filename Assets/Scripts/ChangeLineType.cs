//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: ChangeLineType.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Смяна на типа на линията: солидна,
// прекъсната или прозрачна
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLineType : ActionsManager {

	public Texture dashedLineTexture;
	private bool locked = false;

	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Променя материала на съответната линия според натиснатия бутон
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		// При неселектирани никакви линии, изписва грешка до потребителя
		List<GameObject> lines = GetObjects("Line", true);
		if(lines.Count == 0){
			ReportMessage("Select at least one line", 3);
			return;
		}
		
		// Смени типа на всяка селектирана линия
		foreach(GameObject line in lines){
			
			GameObject lineMesh = line.transform.GetChild(1).gameObject;
			
			if(!locked)AddCommand("STYLE_"+line.GetComponent<LineObject>().type+"_"+gameObject.name.ToUpper()+"_"+line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText()+"_"+line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText());
			
			// Запазва се цвета на линията
			Color tempColor = lineMesh.GetComponent<Renderer>().material.color;

			// Изчиства се текстурата от материала на линията
			lineMesh.GetComponent<Renderer>().material.mainTexture = null;
			
			// Ако бутона за солиден тип е избран 
			if(gameObject.name == "Solid"){
				// Цвета на линията се слага на запазения без прозрачност и линията зе запазва като непрозрачна
				lineMesh.GetComponent<Renderer>().material.color = new Color(tempColor.r, tempColor.g, tempColor.b, 1f);
				line.GetComponent<LineObject>().SetTransparency(false);
				line.GetComponent<LineObject>().type = "SOLID";
				
			// Ако бутона за прекъснат тип е избран 
			}else if(gameObject.name == "Dashed"){
				// На материала на линията се задава и оразмерява текстура и линията зе запазва като непрозрачна
				lineMesh.GetComponent<Renderer>().material.mainTexture = dashedLineTexture;
				lineMesh.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f, line.transform.localScale.y * 10f);
				line.GetComponent<LineObject>().SetTransparency(false);
				line.GetComponent<LineObject>().type = "DASHED";
				
			// Ако бутона за прозрачен тип е избран 
			}else{
				// Цвета на линията се слага на запазения с 50% прозрачност и линията зе запазва като прозрачна
				lineMesh.GetComponent<Renderer>().material.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.5f);
				line.GetComponent<LineObject>().SetTransparency(true);
				line.GetComponent<LineObject>().type = "TRANSPARENT";
			}
			
			// Линията се деселектира
			line.GetComponent<LineObject>().SelectClick();
			
			locked = false;
		}
	}
	
	public void Lock(){
		locked = true;
	}
}
