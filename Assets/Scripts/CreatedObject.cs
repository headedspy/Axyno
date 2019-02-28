//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: CreatedObject.cs
// НАСЛЕДЕН ОТ: PointObject, LineObject. AngleObject
// ЦЕЛ НА КЛАСА: Създаване на помощни методи, използвани от
// всеки създаден обект от типа точка, права или ъгъл
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gvr.Internal;

public abstract class CreatedObject : MonoBehaviour {

	public bool isSelected = false;
	private GameObject head;
	private GameObject task;
	private bool rotationLock = false;
	
	private Gyroscope gyro;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Awake
	// Функцията бива извиквана при пускането на приложението.
	// Задава съответните променливи, съдържащи родителския обект на 
	// чертежа и камерата на потребителя.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Awake(){
		task = GameObject.Find("Task");
		head = GameObject.Find("Head");
		
		gyro = Input.gyro;
		gyro.enabled = true;
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: ChangeColor
	// Променя цвета на обекта с подадения.
	// ПАРАМЕТРИ:
	// - Color c : Цвета, който ще бъде зададен
	//------------------------------------------------------------------------
	public virtual void ChangeColor(Color c){
		gameObject.GetComponent<Renderer>().material.color = c;
	}

	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Select
	// Селектира обекта.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Select(){
		isSelected = true;
		
		if(gameObject.name == "Line"){
			gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Outline");
		}else{
			gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Outline");
		}
		
		if(gameObject.name == "Angle"){
			GetComponent<Renderer>().material.SetFloat("_Outline", 1f);
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Deselect
	// Деселектира обекта.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Deselect(){
		isSelected = false;
		
		if(gameObject.name == "Line"){
			gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Bumped Diffuse");
		}else{
			gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Bumped Diffuse");
		}
    }
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: SelectClick
	// Сменя състоянието на обекта от селектиран на деселектиран или обратно.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void SelectClick(){
		if(head.GetComponent<Info>().tool == "Select"){
			if(isSelected)Deselect();
			else Select();

		}else if(head.GetComponent<Info>().tool == "ShapeSelect"){
			RecursiveAdd(gameObject, !isSelected);
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: RecursiveAdd
	// Рекурсивно селектира или деселектира всички обекти, които са 
	// свързани към дадения
	// ПАРАМЕТРИ:
	// - GameObject obj : Игралния обект, който ще бъде селектиран
	// - bool isSelect : Дали обектите да бъдат селектирани или деселектирани
	//------------------------------------------------------------------------
	private void RecursiveAdd(GameObject obj, bool isSelect){
		if((!obj.GetComponent<CreatedObject>().isSelected && isSelect) || 
			(obj.GetComponent<CreatedObject>().isSelected && !isSelect)){
			
			if(isSelect)obj.GetComponent<CreatedObject>().Select();
			else obj.GetComponent<CreatedObject>().Deselect();
			
			if(obj.name == "Point"){
				foreach(GameObject connectedLine in obj.GetComponent<PointObject>().lines){
					RecursiveAdd(connectedLine, isSelect);
				}
			
			}else if(obj.name == "Line"){
				RecursiveAdd(obj.GetComponent<LineObject>().point1, isSelect);
				RecursiveAdd(obj.GetComponent<LineObject>().point2, isSelect);
				
				foreach(GameObject angle in obj.GetComponent<LineObject>().connectedAngles){
					RecursiveAdd(angle, isSelect);
				}
				
			}else if(obj.name == "Angle"){
				RecursiveAdd(obj.GetComponent<AngleObject>().line1, isSelect);
				RecursiveAdd(obj.GetComponent<AngleObject>().line2, isSelect);
			}
		}
	}
	
	Vector3 oldRotation;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Update
	// Функцията бива извиквана на всеки кадър.
	// Позволява въртенето на чертежа ако потребителя е избрал
	// "Rotate" инструмента
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Update(){
		if(head.GetComponent<Info>().tool == "Rotate"){
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider.gameObject == gameObject || hit.collider.gameObject.transform.parent == gameObject.transform){
					rotationLock = true;
				}
			}
		}
		
		if(rotationLock)Rotate();
		else oldRotation = gyro.attitude.eulerAngles;
	}
	
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Rotate
	// Завърта чертежа свободно около неговия център.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	private void Rotate(){
		if(Input.GetMouseButton(0)){
			
			Vector3 newRotation = gyro.attitude.eulerAngles;
			
			Vector3 deltaRotation = newRotation - oldRotation;
			
			oldRotation = newRotation;
			
			task.transform.Rotate(-deltaRotation.y * Vector3.right);
			task.transform.Rotate(deltaRotation.x * Vector3.up);
		}else{
			rotationLock = false;
		}
	}
}
