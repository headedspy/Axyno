using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLineType : Tool {

	public Texture dashedLineTexture;

	public override void Initiate(){
		List<GameObject> lines = GetObjects("Line", true);
		
		if(lines.Count == 0){
			ReportMessage("Select at least one line", 3);
			return;
		}
		
		foreach(GameObject line in lines){
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
			
			line.GetComponent<LineObject>().SelectClick();
		}
	}
}
