using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WaterManager : MonoBehaviour
{

    public struct waveTarget
    {
        public float amplitudeReduction;
        public float speed;
        public float HeightMultiplyer;

        public waveTarget(float amplitudeReduction, float speed, float HeightMultiplyer)
        {
            this.amplitudeReduction = amplitudeReduction;
            this.speed = speed;
            this.HeightMultiplyer = HeightMultiplyer;
        }
    }


    waveTarget clamSeas = new waveTarget(15, 1, 0.5f);
    waveTarget smallwaves = new waveTarget(8, 1.5f, 0.6f);
    waveTarget choppyWater = new waveTarget(5, 2, 0.7f);
    waveTarget mediumWaves = new waveTarget(4, 2.5f, 1f);
    waveTarget largeWaves = new waveTarget(3f, 3.5f, 1.2f);
    waveTarget currentWaves = new waveTarget(0, 0, 0);

    private bool _isLerping;

    private waveTarget targetWaveTarget;

    public int waveType = 0;
    public float mainLerpSpeed;
    private float lerpSpeed = 2.0f;


    private int _NewWaveType = 0;
    private float _NewWaveTypeSwapPos = 0;
    private bool _SwapingWaveType;

    //constants
    const float z = -1f;

    //water dimentions 
    float _baseheight;
    float _left;
    float _bottom;
    float _right;

    private List<WaterElement> _waterElements;
    private List<WaterMesh> _waterMeshes;
    private LineRenderer _lineRenederer;

    [Tooltip("The number of nodes used per world unit")] [SerializeField] private int waterResolution;
    [Tooltip("The material that the line of the water will be")] [SerializeField] private Material LineMat;
    [Tooltip("The width of the line of the water")] [SerializeField] private float LineWidth = 0.1f;

    [Tooltip("the kind of mesh we're going to use for the main body of water:")] [SerializeField] private GameObject
        watermesh;

    [Tooltip("the physics material of the water, optional")] [SerializeField] private PhysicsMaterial2D waterPMat;

    public string WaterChildLayerName = "Water";

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += Instance_OnGameStateChange;
        
    }

    private void Awake()
    {
        //get the size of the screen and determine the position of each water element
        _waterElements = new List<WaterElement>();
        _waterMeshes = new List<WaterMesh>();

    }

    // Use this for initialization
    void Start()
    {
        lerpSpeed = mainLerpSpeed;
        targetWaveTarget = clamSeas;
        currentWaves = clamSeas;
        Vector3 left = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 Right =
            Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight,
                Camera.main.nearClipPlane));
        SpawnWater(left.x, Right.x - left.x, 0, -10);

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


            int[] tris = new int[6] {0, 1, 3, 3, 2, 0};

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
            currentMesh.Collider.layer = LayerMask.NameToLayer(WaterChildLayerName);
            if (waterPMat)
                currentMesh.Collider.GetComponent<CapsuleCollider2D>().sharedMaterial = waterPMat;

            //Set the position and scale to the correct dimensions
            //tesd
            currentMesh.Collider.transform.position = new Vector3(Left + Width * (i + 0.5f) / edgecount, Top - 0.5f, 0);
            currentMesh.Collider.transform.localScale = new Vector3(Width / edgecount, 1, 1);

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
            _waterMeshes[i].Collider.transform.position = new Vector3(_waterMeshes[i].Collider.transform.position.x,
                _waterElements[i].Y - 0.5f);

        }
    }

    private void waveUpdate()
    {
        //foreach (WaterElement currenetElement in _waterElements)
        for (int i = 0; i < _waterElements.Count; i++)
        {
            _waterElements[i].Y = LerpingWave(_waterElements[i].X);
            _lineRenederer.SetPosition(i, new Vector3(_waterElements[i].X, _waterElements[i].Y, z));
        }
    }



    private void LerpToWave(int newWaveType)
    {
        switch (newWaveType)
        {
            case 0:
                targetWaveTarget = clamSeas;
                break;
            case 1:
                targetWaveTarget = smallwaves;
                break;
            case 2:
                targetWaveTarget = choppyWater;
                break;
            case 3:
                targetWaveTarget = mediumWaves;
                break;
            case 4:
                targetWaveTarget = largeWaves;
                break;
            default:
                targetWaveTarget = clamSeas;
                break;
        }

        StartCoroutine(LerpToNewWaveTarget());

    }

    private IEnumerator LerpToNewWaveTarget()
    {
        float timetotal = lerpSpeed;
        float timeremaining = timetotal;
        waveTarget origWaves = new waveTarget(currentWaves.amplitudeReduction, currentWaves.speed,
            currentWaves.HeightMultiplyer);

        while (timeremaining > 0 && _isLerping)
        {
            timeremaining -= Time.deltaTime;

            float t = (timeremaining / timetotal);
            // Debug.Log("T: " + t + ", Time remaining :" + timeremaining);
            currentWaves.speed = Mathf.Lerp(targetWaveTarget.speed, origWaves.speed, t);

            currentWaves.amplitudeReduction = Mathf.Lerp(targetWaveTarget.amplitudeReduction,
                origWaves.amplitudeReduction, t);

            currentWaves.HeightMultiplyer = Mathf.Lerp(targetWaveTarget.HeightMultiplyer, origWaves.HeightMultiplyer, t);

            //Debug.Log("Currentspeed: " + currentWaves.speed + " orig speed: " + origWaves.speed + " target speed: " +
            //     targetWaveTarget.speed);

            //Debug.Log("amp reduction : " + currentWaves.amplitudeReduction + " orig amp reduction: " + origWaves.amplitudeReduction + " target amp reduction: " +
            //        targetWaveTarget.amplitudeReduction);
            //Debug.Log("current HeightMultiplyer : " + currentWaves.HeightMultiplyer + " orig HeightMultiplyer: " + origWaves.HeightMultiplyer + " target HeightMultiplyer: " +
            //       targetWaveTarget.HeightMultiplyer);
            yield return null;

        }

        _isLerping = false;
        //currentWaves = targetWaveTarget;
    }



    private float LerpingWave(float x)
    {
        float val = 0.0f;
        val = Mathf.Sin(x / currentWaves.amplitudeReduction + (Time.time * currentWaves.speed)) *
              currentWaves.HeightMultiplyer;
        return val;
    }



    private void Instance_OnGameStateChange(GameEnums.GameState gameState)
    {
        if (gameState == GameEnums.GameState.Game)
        {
            _isLerping = false;
            StartCoroutine(DoMainLerp());

        }
        else if (gameState == GameEnums.GameState.GameOver)
        {
            _isLerping = false;
            StartCoroutine(DoEndLerp());
        }
    }

    private IEnumerator DoEndLerp()
    {
        yield return new WaitForSeconds(0.2f);
        lerpSpeed = 10.0f;
        _isLerping = true;
        LerpToWave(0);
    }

    private IEnumerator DoMainLerp()
    {
        yield return new WaitForSeconds(0.2f);
        lerpSpeed = mainLerpSpeed;
        _isLerping = true;
        LerpToWave(waveType);
    }


}


/*  private float SelectWave(float x, int _waveType)
  {
      float val = 0;
      switch (_waveType)
      {
          case 0:
              val = CalmWater(x);
              break;
          case 1:
              val = Smallwaves(x);
              break;
          case 2:
              val = ChoppyWater(x);
              break;
          case 3:
              val = MediumWaves(x);
              break;
          case 4:
              val = LargeWaves(x);
              break;
          default:
              val = 0.0f;
              break;
      }


      return val;
  }

  private float GenerateWave(float x)
  {
      float val = 0;
      if (_SwapingWaveType)
      {

          if (x < _NewWaveTypeSwapPos)
          {
              val = SelectWave(x, _CurrentWaveType);
          }
          else
          {
              // Debug.Log("X: " + x + " _NewWaveTypeSwapPos: " + _NewWaveTypeSwapPos  );
              val = SelectWave(x, _NewWaveType);
          }
      }
      else
      {
          val = SelectWave(x, _CurrentWaveType);
      }

      return val;
  }*/


/*private void ChangeWave(int newWaveType)
{

    _NewWaveType = newWaveType;
    _NewWaveTypeSwapPos = _waterElements[_waterElements.Count-1].X;
    StartCoroutine(StartWaveChange());
}

private IEnumerator StartWaveChange()
{
    while ( WaveAtZero(_NewWaveTypeSwapPos))
    {
        Debug.Log(GenerateWave(_NewWaveTypeSwapPos));
        yield return null;
    }
    Debug.Log("Ready to swap");
    _SwapingWaveType = true;
}

private bool WaveAtZero(float pos)
{
   return  GenerateWave(pos) > 0.02 || GenerateWave(pos) < -0.02f;
}


private float CalmWater(float x)
{
    return SinBasedWave(x, 100, 1, 0.5f);
}

private float Smallwaves(float x)
{
    return SinBasedWave(x, 8, 1.5f, 0.6f);
}

private float ChoppyWater(float x)
{
    return SinBasedWave(x, 5, 2, 0.7f);
}


private float MediumWaves(float x)
{
    return SinBasedWave(x, 4, 2.5f, 1f);
}

private float LargeWaves(float x)
{
    return SinBasedWave(x, 3f, 3.5f, 1.2f);
}*/

/* private float SinBasedWave(float x, float amplitudeReduction, float speed, float HeightMultiplyer)
{
 float val = 0.0f;
 val = Mathf.Sin(x / amplitudeReduction + (Time.time * speed)) * HeightMultiplyer;
 return val;
}*/
