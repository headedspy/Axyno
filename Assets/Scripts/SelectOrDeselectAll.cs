﻿//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: SelectOrDeselect.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Селектиране/Деселектиране на всички обекти
//------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOrDeselectAll : Tool {

	public bool isDeselect = false;
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Initiate
	// Деселектира или селектира обектите
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public override void Initiate(){
		if(isDeselect){
			foreach(GameObject obj in GetObjects("", true)){
				if(obj.GetComponent<CreatedObject>().isSelected){
					obj.GetComponent<CreatedObject>().Deselect();
				}
			}
			
		}else{
			foreach(GameObject obj in GetObjects("", false)){
				if(!obj.GetComponent<CreatedObject>().isSelected){
					obj.GetComponent<CreatedObject>().Select();
				}
			}
		}
	}
}
