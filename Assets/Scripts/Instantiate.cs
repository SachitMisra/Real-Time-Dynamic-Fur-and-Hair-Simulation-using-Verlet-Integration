using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    public int hairAmount;
    List<GameObject> strands = new List<GameObject>();
    public Material material;

    public GameObject root;
    Vector3[] norm;
    Mesh mesh;
    public Vector3[] vertices;
    public Vector3 windDirection;
    public float windIntensity = 0f;
    public float hairLength= 0.1f;
    public int hairSegments= 4;
    public Vector3 forceGravity = new Vector3(0f, -1f, 0f);
    public float hairGrainAmount = 1f;
    public float damping = 5f;
    public Vector3 curl = new Vector3(0,0,0);
    public Vector3 offsetCurl = new Vector3(0, 0, 0);

    public float curlStrength = 0f;

    //public bool[] directionCurl = new bool[3];

    public float startWidth =0.02f;
    public float endWidth =0.005f;
    public int endCapVert =0;
    public float randomLength =0f;
    public float randomWidth =0f;




    void Start()
    {
        
        for (int i = 0; i < hairAmount; i++)
        {
            strands.Add(new GameObject());
            strands[i].AddComponent<LineRenderer>();
            strands[i].GetComponent<LineRenderer>().material = material;
            strands[i].AddComponent<HairSim>();
            strands[i].transform.parent = transform;
            
        }
        mesh = GetComponent<MeshFilter>().mesh;
        norm = mesh.normals;
        if(strands.Count!=0)
        for (int i = 0; i < hairAmount; i++)
            {
                
                strands[i].GetComponent<HairSim>().hairLength = hairLength +Random.Range(-0.01f,0.01f)*randomLength; 
                strands[i].GetComponent<HairSim>().segmentLength = hairSegments; 
                strands[i].GetComponent<HairSim>().forceGravity = forceGravity;
                strands[i].GetComponent<HairSim>().hairGrainAmount = hairGrainAmount;
                strands[i].GetComponent<HairSim>().damping = damping;
                strands[i].GetComponent<HairSim>().curl = curl;
                strands[i].GetComponent<HairSim>().curlStrength = curlStrength;
                strands[i].GetComponent<HairSim>().offsetCurl = offsetCurl;
                strands[i].GetComponent<HairSim>().startWidth = startWidth + Random.Range(-0.1f,0.1f)*randomWidth;
                strands[i].GetComponent<HairSim>().endWidth = endWidth + Random.Range(-0.1f,0.1f)*randomWidth;
                strands[i].GetComponent<HairSim>().endCapVert = endCapVert;
            }


    }
    void OnValidate()
    {
        if(strands.Count!=0)
        for (int i = 0; i < hairAmount; i++)
            {
                
                strands[i].GetComponent<HairSim>().hairLength = hairLength +Random.Range(-0.01f,0.01f)*randomLength; 
                strands[i].GetComponent<HairSim>().segmentLength = hairSegments; 
                strands[i].GetComponent<HairSim>().forceGravity = forceGravity;
                strands[i].GetComponent<HairSim>().hairGrainAmount = hairGrainAmount;
                strands[i].GetComponent<HairSim>().damping = damping;
                strands[i].GetComponent<HairSim>().curl = curl;
                strands[i].GetComponent<HairSim>().curlStrength = curlStrength;
                strands[i].GetComponent<HairSim>().offsetCurl = offsetCurl;
                strands[i].GetComponent<HairSim>().startWidth = startWidth + Random.Range(-0.1f,0.1f)*randomWidth;
                strands[i].GetComponent<HairSim>().endWidth = endWidth + Random.Range(-0.1f,0.1f)*randomWidth;
                strands[i].GetComponent<HairSim>().endCapVert = endCapVert;
            }
    }

    // Update is called once per frame
    void Update()
    {
        vertices = mesh.vertices;
        //windspeed for hair swinging on curl param
        //windSpeed += Time.deltaTime; 

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);
            norm[i] = (vertices[i] - root.transform.position);
            //Debug.DrawRay(vertices[i], (vertices[i] -root.transform.position)*0.1f, Color.red);
        }
        if (hairAmount != 0)
        {
            
            int density = vertices.Length / hairAmount;
            for (int i = 0; i < hairAmount; i++)
            {
                strands[i].GetComponent<HairSim>().ropeStartPoint = vertices[i * density];
                strands[i].GetComponent<HairSim>().hairGrain = norm[i * density];
                strands[i].GetComponent<HairSim>().forceGravity += new Vector3(windDirection.x  *Random.Range(-windIntensity, windIntensity), windDirection.y * Random.Range(-windIntensity, windIntensity), windDirection.z * Random.Range(-windIntensity, windIntensity));
            }
        }


    }
}
