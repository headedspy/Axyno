using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryMenuControl : Tool {

	public GameObject menu;
	
	public override void Initiate(){
		if(menu.activeSelf){
			menu.SetActive(false);
		}else{
			menu.SetActive(true);
		}
	}
}
