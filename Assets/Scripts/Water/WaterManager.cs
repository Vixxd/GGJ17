﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{



    //constants
    const float springconstant = 0.02f;
    const float damping = 0.04f;
    const float spread = 0.05f;
    const float z = -1f;

    //water dimentions 
    float _baseheight;
    float _left;
    float _bottom;
    float _right;

    private List<WaterElement> _waterElements;
    private List<WaterMesh> _waterMeshes;
    private LineRenderer _lineRenederer;

    [Tooltip("The number of nodes used per world unit")]
    [SerializeField]
    private int waterResolution;
    [Tooltip("The material that the line of the water will be")]
    [SerializeField]
    private Material LineMat;
    [Tooltip("The width of the line of the water")]
    [SerializeField]
    private float LineWidth = 0.1f;
    [Tooltip("the kind of mesh we're going to use for the main body of water:")]
    [SerializeField]
    private GameObject watermesh;

    private void Awake()
    {
        //get the size of the screen and determine the position of each water element
        _waterElements = new List<WaterElement>();
        _waterMeshes = new List<WaterMesh>();

    }

    // Use this for initialization
    void Start()
    {
        SpawnWater(-10, 20, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        waveUpdate();
        UpdateMeshes();
    }

    private int DetermineNumberOfWaterElements()
    {
        float numOfEl = (float)Screen.width / (float)waterResolution;
        return Mathf.RoundToInt(numOfEl);
    }


    public void SpawnWater(float Left, float Width, float Top, float bottom)
    {
        _bottom = bottom;
        int edgecount = Mathf.RoundToInt(Width) * waterResolution;
        int nodecount = edgecount + 1;

        _lineRenederer = gameObject.AddComponent<LineRenderer>();
        _lineRenederer.material = LineMat;
        _lineRenederer.material.renderQueue = 1000;
        _lineRenederer.SetVertexCount(nodecount);
        _lineRenederer.SetWidth(LineWidth, LineWidth);

        for (int i = 0; i < nodecount; i++)
        {
            WaterElement currentElement = new WaterElement(Left + Width * i / edgecount, Top);
            //currentElement.X = Left + Width * i / edgecount;
            //currentElement.Y = Top;
            _lineRenederer.SetPosition(i, new Vector3(currentElement.X, currentElement.Y, z));
            _waterElements.Add(currentElement);
        }

        for (int i = 0; i < edgecount; i++)
        {
            WaterMesh currentMesh = new WaterMesh();


            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(_waterElements[i].X, _waterElements[i].Y, z);
            Vertices[1] = new Vector3(_waterElements[i + 1].X, _waterElements[i + 1].Y, z);
            Vertices[2] = new Vector3(_waterElements[i].X, bottom, z);
            Vertices[3] = new Vector3(_waterElements[i + 1].X, bottom, z);

            Vector2[] UVs = new Vector2[4];
            UVs[0] = new Vector2(0, 1);
            UVs[1] = new Vector2(1, 1);
            UVs[2] = new Vector2(0, 0);
            UVs[3] = new Vector2(1, 0);


            int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

            currentMesh.Mesh.vertices = Vertices;
            currentMesh.Mesh.uv = UVs;
            currentMesh.Mesh.triangles = tris;

            currentMesh.MeshObject = Instantiate(watermesh, Vector3.zero, Quaternion.identity) as GameObject;
            currentMesh.MeshObject.GetComponent<MeshFilter>().mesh = currentMesh.Mesh;
            currentMesh.MeshObject.transform.parent = transform;

            _waterMeshes.Add(currentMesh);

        }
    }

    void UpdateMeshes()
    {
        for (int i = 0; i < _waterMeshes.Count; i++)
        {

            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(_waterElements[i].X, _waterElements[i].Y, z);
            Vertices[1] = new Vector3(_waterElements[i + 1].X, _waterElements[i + 1].Y, z);
            Vertices[2] = new Vector3(_waterElements[i].X, _bottom, z);
            Vertices[3] = new Vector3(_waterElements[i + 1].X, _bottom, z);

            _waterMeshes[i].Mesh.vertices = Vertices;
        }
    }

    private void waveUpdate()
    {
        foreach (WaterElement currenetElement in _waterElements)
        {
            currenetElement.Y = sinWave(currenetElement.X);
        }
    }

    private float sinWave (float x){
        float val = 0.0f;
        val = Mathf.Sin(4* Mathf.Sin(x + Time.time));
        return val;
   }
}
