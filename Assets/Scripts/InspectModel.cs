using UnityEngine;

public class InspectModel : MonoBehaviour
{
    public float rotationSpeed = 1f;
    float currentZoom;
    public float zoomDistance = 1f;

    float XaxisRotation = 0f;
    float YaxisRotation = 0f;

    void Start()
    {
        currentZoom = Camera.main.transform.position.z;
    }

    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") >0) // forward
 {
            Camera.main.transform.Translate(new Vector3(0,0,zoomDistance));
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
 {
            Camera.main.transform.Translate(new Vector3(0,0,-(zoomDistance)));
        }
        transform.Rotate(Vector3.down, XaxisRotation);
        transform.Rotate(Vector3.left, YaxisRotation);
    }
    void Update()
    {
        OnMouseDrag();
    }
}