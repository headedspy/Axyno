//------------------------------------------------------------------------
// ИМЕ НА ФАЙЛА: AngleObject.cs
// НАСЛЕДЕН ОТ: -
// ЦЕЛ НА КЛАСА: Дефиниране на различни методи, обуславящи
// обект от типа ъгъл
//------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleObject : CreatedObject {
	private float segmentRadius = 0.2f;
	private float tubeRadius = 0.03f;
	private int segments = 32;
	private int tubes = 12;
	
	public GameObject line1;
	public GameObject line2;
	
	public float angleValue = 0f;
	
	public GameObject point;
	
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: Connect
	// Свързва дадените линии за ъгъла
	// ПАРАМЕТРИ:
	// - GameObject l1 : Първата линия, която ще бъде свързана
	// - GameObject l2 : Втората линия, която ще бъде свързана
	//------------------------------------------------------------------------
	public void Connect(GameObject l1, GameObject l2){
		line1 = l1;
		line2 = l2;
		
		l1.GetComponent<LineObject>().ConnectAngle(gameObject);
		l2.GetComponent<LineObject>().ConnectAngle(gameObject);
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: SwitchLine
	// Сменя едната линия на ъгъла с друга
	// ПАРАМЕТРИ:
	// - GameObject fromLine : Линията, която ще бъде заменена
	// - GameObject toLine : Новата линия, която ще бъде свързана
	//------------------------------------------------------------------------
	public void SwitchLine(GameObject fromLine, GameObject toLine){
		if(line1 == fromLine){
			line1.GetComponent<LineObject>().DisconnectAngle(gameObject);
			line1 = toLine;
			line1.GetComponent<LineObject>().ConnectAngle(gameObject);
		}
		if(line2 == fromLine){
			line2.GetComponent<LineObject>().DisconnectAngle(gameObject);
			line2 = toLine;
			line2.GetComponent<LineObject>().ConnectAngle(gameObject);
		}
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: OnDestroy
	// Преди изтриването на ъгъла, той бива разкачена от всички линии
	// ПАРАМЕТРИ:
	// - Няма
	//------------------------------------------------------------------------
	public void OnDestroy(){
		line1.GetComponent<LineObject>().DisconnectAngle(gameObject);
		line2.GetComponent<LineObject>().DisconnectAngle(gameObject);
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: UpdateAngle
	// Построява и позиционира ъгловата форма между двете линии
	// ПАРАМЕТРИ:
	// - GameObject line1 : Едната линия на ъгъла
	// - GameObject line2 : Другата линия на ъгъла
	//------------------------------------------------------------------------
	public void UpdateAngle(GameObject line1, GameObject line2){
		point = null;
		
		if(line1.GetComponent<LineObject>().point1 == line2.GetComponent<LineObject>().point1){
			point = line1.GetComponent<LineObject>().point1;
		}else if(line1.GetComponent<LineObject>().point1 == line2.GetComponent<LineObject>().point2){
			point = line1.GetComponent<LineObject>().point1;
		}else if(line1.GetComponent<LineObject>().point2 == line2.GetComponent<LineObject>().point1){
			point = line1.GetComponent<LineObject>().point2;
		}else if(line1.GetComponent<LineObject>().point2 == line2.GetComponent<LineObject>().point2){
			point = line1.GetComponent<LineObject>().point2;
		}
		
		if(point != null){
			gameObject.transform.position = point.transform.position;
			gameObject.transform.rotation = Quaternion.identity;
			
			GameObject p1 = line1.GetComponent<LineObject>().point1 == point ? line1.GetComponent<LineObject>().point2 : line1.GetComponent<LineObject>().point1;
			GameObject p2 = line2.GetComponent<LineObject>().point1 == point ? line2.GetComponent<LineObject>().point2 : line2.GetComponent<LineObject>().point1;
			
			gameObject.transform.LookAt((p1.transform.position + p2.transform.position) / 2);
			
			float oa = line1.GetComponent<LineObject>().GetLength();
			float ob = line2.GetComponent<LineObject>().GetLength();
			float ab = Vector3.Distance(p1.transform.position, p2.transform.position);
			
			angleValue = Mathf.Acos( (oa*oa + ob*ob - ab*ab) / (2 * oa * ob) );
			
			BuildTorus( Mathf.RoundToInt( (4 * (angleValue*Mathf.Rad2Deg)) / 45) );
			
			gameObject.transform.localScale = new Vector3(1f, 0.12f, 1f);
			
			Vector3 side1 = p1.transform.position - point.transform.position;
			Vector3 side2 = p2.transform.position - point.transform.position;
			
			Vector3 yVector = Vector3.Cross(side1, side2);
			
			gameObject.transform.rotation = Quaternion.LookRotation(yVector) * Quaternion.Euler(90f, 0f, 0f);
			
			
			gameObject.transform.position = point.transform.position;
			
			
			gameObject.transform.LookAt(p2.transform, yVector);
			gameObject.transform.Rotate(Vector3.up * -90f);
			
			GameObject text = gameObject.transform.GetChild(0).gameObject;
	
			text.transform.LookAt((p2.transform.position + p1.transform.position) / 2f);
			text.transform.Translate(Vector3.forward * 0.5f, Space.Self);
			
			text.GetComponent<FaceCamera>().enabled = true;
		}
	}
	

	//------------------------------------------------------------------------
	// ФУНКЦИЯ: BuildTorus
	// Построява част от тор с определен брой сегменти от 32
	// ПАРАМЕТРИ:
	// - int segmentsCount : Брой на сегментите на тора
	//------------------------------------------------------------------------
	private void BuildTorus(int segmentsCount){
		int totalVertices = segments * tubes;
		int totalPrimitives = totalVertices * 2;
		int totalIndices = totalPrimitives * 3;
		
		ArrayList verticesList = new ArrayList();
		ArrayList indicesList = new ArrayList();
		
		float numSegments = segments;
		float numTubes = tubes;
		
		float segmentSize = 2*Mathf.PI/numSegments;
		float tubeSize = 2*Mathf.PI/numTubes;
		
		float x;
		float y;
		float z;
		
		ArrayList segmentList = new ArrayList();
		ArrayList tubeList = new ArrayList();
		
		for(int i=0; i<=segmentsCount; i++){
			tubeList = new ArrayList();
			
			for(int j=0; j<numTubes; j++){
				
				x = (segmentRadius + tubeRadius * Mathf.Cos(j*tubeSize)) * Mathf.Cos(i*segmentSize);
				y = (segmentRadius + tubeRadius * Mathf.Cos(j*tubeSize)) * Mathf.Sin(i*segmentSize);
				z = tubeRadius * Mathf.Sin(j*tubeSize);
				
				tubeList.Add(new Vector3(x, z, y));
				verticesList.Add(new Vector3(x, z, y));
			}
			
			segmentList.Add(tubeList);
		}
		
		for(int i=0; i<segmentList.Count-1; i++){
			int n = (i+1) % segmentList.Count;
			
			ArrayList currentTube = (ArrayList)segmentList[i];
			ArrayList nextTube = (ArrayList)segmentList[n];
			
			for(int j=0; j<currentTube.Count; j++){
				int m = (j+1) % currentTube.Count;
				
				Vector3 v1 = (Vector3)currentTube[j];
				Vector3 v2 = (Vector3)currentTube[m];
				Vector3 v3 = (Vector3)nextTube[m];
				Vector3 v4 = (Vector3)nextTube[j];
				
				indicesList.Add((int)verticesList.IndexOf(v1));
				indicesList.Add((int)verticesList.IndexOf(v2));
				indicesList.Add((int)verticesList.IndexOf(v3));
				
				indicesList.Add((int)verticesList.IndexOf(v3));
				indicesList.Add((int)verticesList.IndexOf(v4));
				indicesList.Add((int)verticesList.IndexOf(v1));
			}
		}
		
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[totalVertices];
		verticesList.CopyTo(vertices);
		int[] triangles = new int[totalIndices];
		indicesList.CopyTo(triangles);
		
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		
		mesh.RecalculateBounds();
		
		MeshFilter mFilter = GetComponent(typeof(MeshFilter)) as MeshFilter;
		mFilter.mesh = mesh;
		
		GetComponent<MeshCollider>().sharedMesh = mesh;
		
		GetComponent<MeshFilter>().mesh.RecalculateNormals();
	}
	
	//------------------------------------------------------------------------
	// ФУНКЦИЯ: AddText
	// Дава репрезентативна стойност на ъгъла
	// ПАРАМЕТРИ:
	// - string s : Стойността, което ъгъла ще има
	//------------------------------------------------------------------------
	public void AddText(string s){
		transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = s;
	}
}
