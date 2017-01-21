using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{



    //constants
    const float _springconstant = 0.02f;
    const float _damping = 0.04f;
    const float _spread = 0.05f;
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
    [Tooltip("the physics material of the water, optional")]
    [SerializeField]
    private PhysicsMaterial2D waterPMat;


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


            //Create our colliders, set them be our child
            currentMesh.Collider = new GameObject();
            currentMesh.Collider.name = "Collider";
            currentMesh.Collider.AddComponent<CapsuleCollider2D>();
            currentMesh.Collider.transform.parent = transform;
            if (waterPMat) 
                currentMesh.Collider.GetComponent<CapsuleCollider2D>().sharedMaterial = waterPMat;

            //Set the position and scale to the correct dimensions
            currentMesh.Collider.transform.position = new Vector3(Left + Width * (i + 0.5f) / edgecount, Top - 0.5f, 0);
            currentMesh.Collider.transform.localScale = new Vector3(Width / edgecount, 1, 1);

            //Add a WaterDetector and make sure they're triggers
            // currentMesh.Collider.GetComponent<BoxCollider2D>().isTrigger = true;
           // currentMesh.Collider.GetComponent<BoxCollider2D>()//. = true;
            //currentMesh.Collider.AddComponent<WaterDetector>();

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
            _waterMeshes[i].Collider.transform.position = new Vector3(_waterMeshes[i].Collider.transform.position.x, _waterElements[i].Y-0.5f);

        }


    }

    private void waveUpdate()
    {
        //foreach (WaterElement currenetElement in _waterElements)
        for(int i = 0;i<_waterElements.Count;i++)
        {
            _waterElements[i].Y = sinWave(_waterElements[i].X);
            _lineRenederer.SetPosition(i, new Vector3(_waterElements[i].X, _waterElements[i].Y, z));
        }
    }

    private float sinWave (float x)
    {
        float amplitudeReduction = 4;
        float speed = 3;

        float val = 0.0f;
        val =   Mathf.Sin(x/ amplitudeReduction + (Time.time * speed));
        return val;
   }
}
