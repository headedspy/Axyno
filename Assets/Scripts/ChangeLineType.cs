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
		List<GameObject> lines = GetObjects("Line", true);
		if(lines.Count == 0){
			ReportMessage("Select at least one line");
			return;
		}
		
		foreach(GameObject line in lines){
			
			GameObject lineMesh = line.transform.GetChild(1).gameObject;
			
			if(!locked)AddCommand("STYLE_"+line.GetComponent<LineObject>().type+"_"+gameObject.name.ToUpper()+"_"+line.GetComponent<LineObject>().point1.GetComponent<PointObject>().GetText()+"_"+line.GetComponent<LineObject>().point2.GetComponent<PointObject>().GetText());
			
			Color tempColor = lineMesh.GetComponent<Renderer>().material.color;

			lineMesh.GetComponent<Renderer>().material.mainTexture = null;
			
			if(gameObject.name == "Solid"){
				lineMesh.GetComponent<Renderer>().material.color = new Color(tempColor.r, tempColor.g, tempColor.b, 1f);
				line.GetComponent<LineObject>().SetTransparency(false);
				line.GetComponent<LineObject>().type = "SOLID";
 
			}else if(gameObject.name == "Dashed"){
				lineMesh.GetComponent<Renderer>().material.mainTexture = dashedLineTexture;
				lineMesh.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f, line.transform.localScale.y * 10f);
				line.GetComponent<LineObject>().SetTransparency(false);
				line.GetComponent<LineObject>().type = "DASHED";
				
			}else{
				lineMesh.GetComponent<Renderer>().material.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.5f);
				line.GetComponent<LineObject>().SetTransparency(true);
				line.GetComponent<LineObject>().type = "TRANSPARENT";
			}
			
			line.GetComponent<LineObject>().SelectClick();
			
			locked = false;
		}
		
		Vibrate();
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Променя материала на съответната линия според натиснатия бутон
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Lock(){
		locked = true;
	}
}
