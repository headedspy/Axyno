using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gvr.Internal;

public abstract class CreatedObject : MonoBehaviour {

	public bool isSelected = false;
	
	private int rotateSpeed = 5;
	private GameObject head;
	private GameObject task;
	private bool rotationLock = false;

	public virtual void ChangeColor(Color c){
		gameObject.GetComponent<Renderer>().material.color = c;
	}


	public void Select(){
		isSelected = true;
		gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Outline");
	}

	public void Deselect(){
		isSelected = false;
        gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Legacy Shaders/Transparent/Bumped Diffuse");
    }
	
	public void SelectClick(){
		if(head.GetComponent<Info>().tool == "Select"){
			if(isSelected)Deselect();
			else Select();
		}else if(head.GetComponent<Info>().tool == "ShapeSelect"){
			SelectShape();
		}
	}
	
	public void SelectShape(){
		RecursiveAdd(gameObject, !isSelected);
	}
	
	private void RecursiveAdd(GameObject obj, bool isSelect){
		if((!obj.GetComponent<CreatedObject>().isSelected && isSelect) || (obj.GetComponent<CreatedObject>().isSelected && !isSelect)){
			if(isSelect)obj.GetComponent<CreatedObject>().Select();
			else obj.GetComponent<CreatedObject>().Deselect();
			if(obj.name == "Point"){
				foreach(GameObject connectedLine in obj.GetComponent<PointObject>().lines){
					RecursiveAdd(connectedLine, isSelect);
				}
			}else if(obj.name == "Line"){
				RecursiveAdd(obj.GetComponent<LineObject>().point1, isSelect);
				RecursiveAdd(obj.GetComponent<LineObject>().point2, isSelect);
			}else if(obj.name == "Angle"){
				RecursiveAdd(obj.GetComponent<AngleObject>().line1, isSelect);
				RecursiveAdd(obj.GetComponent<AngleObject>().line2, isSelect);
			}
		}
	}
	
	public void Awake(){
		task = GameObject.Find("Task");
		head = GameObject.Find("Head");
	}
	
	public void Update(){
		if(head.GetComponent<Info>().tool == "Rotate"){
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider.gameObject == gameObject){
					rotationLock = true;
				}
			}
		}
		if(rotationLock)Rotate();
	}
	
	private void Rotate(){
		if(Input.GetMouseButton(0)){
			float rotX = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
			float rotY = Input.GetAxis("Mouse Y") * rotateSpeed * Mathf.Deg2Rad;
			
			task.transform.RotateAround(Vector3.up, -rotX);
			task.transform.RotateAround(Vector3.right, rotY);
		}else{
			rotationLock = false;
		}
	}
}
