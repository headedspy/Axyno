using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleObject : CreatedObject {
	private float segmentRadius = 0.2f;
	private float tubeRadius = 0.03f;
	private int segments = 32;
	private int tubes = 12;
	
	public GameObject line1, line2;
	
	public void Connect(GameObject l1, GameObject l2){
		line1 = l1;
		line2 = l2;
		
		l1.GetComponent<LineObject>().ConnectAngle(gameObject);
		l2.GetComponent<LineObject>().ConnectAngle(gameObject);
	}
	
	public void BuildTorus(int segmentsCount){
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
			// next/first segment offset
			int n = (i+1) % segmentList.Count;
			
			//current and next segments
			ArrayList currentTube = (ArrayList)segmentList[i];
			ArrayList nextTube = (ArrayList)segmentList[n];
			
			for(int j=0; j<currentTube.Count; j++){
				int m = (j+1) % currentTube.Count;
				
				Vector3 v1 = (Vector3)currentTube[j];
				Vector3 v2 = (Vector3)currentTube[m];
				Vector3 v3 = (Vector3)nextTube[m];
				Vector3 v4 = (Vector3)nextTube[j];
				
				//add 1st triangle
				indicesList.Add((int)verticesList.IndexOf(v1));
				indicesList.Add((int)verticesList.IndexOf(v2));
				indicesList.Add((int)verticesList.IndexOf(v3));
				
				//finish the quad
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
	
	//GetComponent<Renderer>().material.SetFloat("_Outline", 0.013f);
}
