using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryLine : MonoBehaviour
{
    [Header("Setting")]
    public LayerMask layerMask;
    public float defaultLength = 50;
    [SerializeField] private int numOfReflect = 1;

    private Ray ray;
    private Vector3 direction;

    private LineRenderer lineRenderer;
    private Camera cam;
    private RaycastHit hit;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        cam = Camera.main;
    }

    void Update()
    {
        ReflectLine();
    }

    
    void ReflectLine()
    {
        ray = new Ray(transform.position, transform.forward);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);

        float remainLength = defaultLength;

        for (int i = 0; i < numOfReflect; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainLength, layerMask))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount-1, hit.point);

                remainLength -= Vector3.Distance(ray.origin, hit.point);

                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + (ray.direction * remainLength)) ;
            }
        }
    }
    public void ShowLine(bool show)
    {
        lineRenderer.enabled = show;
    }
}
