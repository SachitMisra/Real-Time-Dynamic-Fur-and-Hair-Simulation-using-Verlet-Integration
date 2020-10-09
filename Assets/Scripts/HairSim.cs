using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairSim : MonoBehaviour
{
    private LineRenderer hair;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    public float hairLength = 0.05f;
    //Set Max number of segments here (CPU intensive)
    public int segmentLength = 16;
    public Vector3 ropeStartPoint;
    public Vector3 forceGravity = new Vector3(0f, -1f, 0f);
    public Vector3 hairGrain;
    public float hairGrainAmount = 1f;
    public float damping = 5f;
    public Vector3 curl = new Vector3(0, 0, 0);
    public Vector3 offsetCurl = new Vector3(0, 0, 0);


    public float curlStrength = 1f;

    //public bool[] directionCurl = new bool[3];
    public float startWidth = 0.1f;
    public float endWidth = 0.05f;
    public int endCapVert =0;



    void Start()
    {
        // for (int i = 0; i < segmentLength; i++)
        // {
        //     ropeSegments.Add(new RopeSegment(ropeStartPoint));
        //     ropeStartPoint.y -= hairLength;
        // }
        // ropeStartPoint.y +=hairLength*segmentLength;
        hair = GetComponent<LineRenderer>();

    }

    void OnValidate()
    {
        for (int i = 0; i < segmentLength; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= hairLength;
        }
        ropeStartPoint.y += hairLength * segmentLength;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(UnityEditor.TransformUtils.GetInspectorRotation(transform.parent.transform).z*Mathf.PI/180);
        DrawRope();
    }
    void FixedUpdate()
    {
        //DrawRope();
        Simulate();
    }
    private void Simulate()
    {
        for (int i = 0; i < segmentLength; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector3 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity / (damping + Random.Range(-0.1f, 5f));
            firstSegment.posNow += forceGravity * Time.deltaTime;
            firstSegment.posNow += hairGrain * Time.deltaTime * hairGrainAmount;


            

                firstSegment.posNow.y += Mathf.Sin(curl.z * i + offsetCurl.z + UnityEditor.TransformUtils.GetInspectorRotation(transform.root).z * Mathf.PI / 180) * curlStrength;
                firstSegment.posNow.x += Mathf.Cos(curl.z * i + offsetCurl.z + UnityEditor.TransformUtils.GetInspectorRotation(transform.root).z * Mathf.PI / 180) * curlStrength;
                firstSegment.posNow.x += Mathf.Sin(curl.y * i + offsetCurl.y + UnityEditor.TransformUtils.GetInspectorRotation(transform.root).y * Mathf.PI / 180) * curlStrength;
                firstSegment.posNow.z += Mathf.Cos(curl.y * i + offsetCurl.y + UnityEditor.TransformUtils.GetInspectorRotation(transform.root).y * Mathf.PI / 180) * curlStrength;
                firstSegment.posNow.z += Mathf.Sin(curl.x * i + offsetCurl.x + UnityEditor.TransformUtils.GetInspectorRotation(transform.root).x * Mathf.PI / 180) * curlStrength;
                firstSegment.posNow.y += Mathf.Cos(curl.x * i + offsetCurl.x + UnityEditor.TransformUtils.GetInspectorRotation(transform.root).x * Mathf.PI / 180) * curlStrength;
                
            








            ropeSegments[i] = firstSegment;
        }

        ApplyConstraint();

    }
    private void DrawRope()
    {
        hair.startWidth = startWidth;
        hair.endWidth = endWidth;
        hair.numCapVertices = endCapVert;
        Vector3[] ropePositions = new Vector3[segmentLength];
        for (int i = 0; i < segmentLength; i++)
        {
            ropePositions[i] = ropeSegments[i].posNow;
        }
        hair.positionCount = ropePositions.Length;
        hair.SetPositions(ropePositions);
    }
    private void ApplyConstraint()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = ropeStartPoint;
        ropeSegments[0] = firstSegment;

        for (int i = 0; i < segmentLength - 1; i++)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - hairLength);
            Vector3 changeDir = Vector3.zero;

            if (dist > hairLength)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < hairLength)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector3 changeAmount = changeDir * error;
            secondSeg.posNow += changeAmount;
            ropeSegments[i + 1] = secondSeg;
        }
    }
    public struct RopeSegment
    {
        public Vector3 posNow;
        public Vector3 posOld;

        public RopeSegment(Vector3 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
}
