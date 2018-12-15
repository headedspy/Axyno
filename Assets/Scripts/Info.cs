using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour {

	public string tool = "None";

	public void ChangeTool(string s){
		tool = s;
	}
	
	public void ResetTool(){
		tool = "None";
	}
}
