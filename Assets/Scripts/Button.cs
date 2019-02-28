//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: Button.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Управление на обектите бутони в сцената
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public GameObject menu;

	private bool isOn = false;
	public bool isTool = false;
	
	public static bool isMenuOpen = false;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: PressDown
	// Извиква се при натискане върху бутона. Отваря съответното меню
	// на бутона и променя цвета на самия бутон
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void PressDown(){
		if(!isTool){
			StartCoroutine(PlayAnimation(!isOn));
			isOn = !isOn;
			gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
		}else{
			if(!isMenuOpen){
				isOn = true;
				isMenuOpen = true;
				StartCoroutine(PlayAnimation(true));
			}else{
				if(isOn){
					isOn = false;
					isMenuOpen = false;
					StartCoroutine(PlayAnimation(false));
				}
			}
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: PressUp
	// Извиква се при отпускане от бутона. Връща цвета на бутона обратно на черен
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void PressUp(){
		if(isOn && isTool && isMenuOpen){
			gameObject.GetComponent<Renderer> ().material.color = new Color (1f, 1f, 1f);
		}else{
			gameObject.GetComponent<Renderer> ().material.color = new Color (0f, 0f, 0f);
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: PlayAnimation
	// Отваря/Затваря съответното меню с анимация
	// ПАРАМЕТРИ:
	// - bool isOpen: Отваряща анимация ли е
	//------------------------------------------------------------------------
	private IEnumerator PlayAnimation(bool isOpen){
		float scale = 1f;
		if(isTool)scale = 1.1f;
		
		if(isOpen){
			menu.SetActive(isOpen);
			for(float i=0f; i<scale; i+=0.15f){
				menu.transform.localScale = new Vector3(menu.transform.localScale.x, i, menu.transform.localScale.z);
				yield return new WaitForSeconds(0.01f);
			}
		}else{
			for(float i=scale; i>0f; i-=0.15f){
				menu.transform.localScale = new Vector3(menu.transform.localScale.x, i, menu.transform.localScale.z);
				yield return new WaitForSeconds(0.01f);
			}
			menu.SetActive(isOpen);
		}
	}
}
