using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMesh  {

    public GameObject MeshObject;
    public Mesh Mesh;
     

    public WaterMesh(){
        Mesh = new Mesh();

    }
    public WaterMesh (GameObject meshObject, Mesh mesh)
    {
        MeshObject = meshObject;
        Mesh = mesh;
    }
}
