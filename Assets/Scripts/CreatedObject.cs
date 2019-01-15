//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: CreatedObject.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Създаване на помощни методи, използвани от
// всеки създаден обект от типа точка, права или ъгъл
//------------------------------------------------------------------------

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
	private void Select(){
		isSelected = true;
		gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Outline");
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Deselect
	// Деселектира обекта.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	private void Deselect(){
		isSelected = false;
        gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Legacy Shaders/Transparent/Bumped Diffuse");
    }
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: SelectClick
	// Сменя състоянието на обекта от селектиран на деселектиран или обратно.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void SelectClick(){
		// Ако потребителя е със "Select" инструмента
		if(head.GetComponent<Info>().tool == "Select"){
			if(isSelected)Deselect();
			else Select();
			
		// Ако потребителя е със "ShapeSelect" инструмента
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
		
		// Проверка дали обекта вече не е селектиран или съответно деселектиран
		if((!obj.GetComponent<CreatedObject>().isSelected && isSelect) || 
			(obj.GetComponent<CreatedObject>().isSelected && !isSelect)){
			
			// Ако не е, той бива селектиран или съответно деселектиран
			if(isSelect)obj.GetComponent<CreatedObject>().Select();
			else obj.GetComponent<CreatedObject>().Deselect();
			
			// Ако обекта е от тип точка
			if(obj.name == "Point"){
				foreach(GameObject connectedLine in obj.GetComponent<PointObject>().lines){
					// Извиква се същия метод за всички линии, свързани към точката
					RecursiveAdd(connectedLine, isSelect);
				}
			
			// Ако обекта е от тип линия
			}else if(obj.name == "Line"){
				// Извиква се същия метод за двете точки, ограничаващи линията
				RecursiveAdd(obj.GetComponent<LineObject>().point1, isSelect);
				RecursiveAdd(obj.GetComponent<LineObject>().point2, isSelect);
				
			// Ако обекта е от тип ъгъл
			}else if(obj.name == "Angle"){
				// Извиква се същия метод за двете линии, съставляващи ъгъла
				RecursiveAdd(obj.GetComponent<AngleObject>().line1, isSelect);
				RecursiveAdd(obj.GetComponent<AngleObject>().line2, isSelect);
			}
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Update
	// Функцията бива извиквана на всеки кадър.
	// Позволява въртенето на чертежа ако потребителя е избрал
	// "Rotate" инструмента
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void Update(){
		// Ако потребителя е със "Rotate" инструмента
		if(head.GetComponent<Info>().tool == "Rotate"){
			// Спуска се лъч от центъра на екрана напред
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit)){
				// Ако лъчът удари обекта може да се предприеме ротация
				if(hit.collider.gameObject == gameObject){
					rotationLock = true;
				}
			}
		}
		
		// Извиква се Rotate всеки кадър
		if(rotationLock)Rotate();
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Rotate
	// Завърта чертежа свободно около неговия център.
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	private void Rotate(){
		// Започване на ротацията при натискане на екрана
		if(Input.GetMouseButton(0)){
			float rotX = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
			float rotY = Input.GetAxis("Mouse Y") * rotateSpeed * Mathf.Deg2Rad;
			
			task.transform.RotateAround(Vector3.up, -rotX);
			task.transform.RotateAround(Vector3.right, rotY);
		}else{
			// При отпускане на екрана спира изпълнението
			rotationLock = false;
		}
	}
}
