using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour {
	public Texture BoxTexture;

    void OnGUI(){
		GUI.backgroundColor = new Color(0f, 0f, 0f, 0f);
        GUI.Box(new Rect((Screen.width/2f) - 25f/2, (Screen.height/2f) - 25f/2, 25f, 25f), BoxTexture);
    }
	
	public void Update(){
		if(Input.GetMouseButton(0)){
			Destroy(gameObject);
		}
	}
}