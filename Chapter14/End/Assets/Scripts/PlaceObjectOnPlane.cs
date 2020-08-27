using System;
using UnityEngine;

public class PlaceObjectOnPlane : MonoBehaviour
{
    public GameObject ObjectToPlace;

    private FindPlane PlaneFinder;
    private PlaneData Plane = null;

    void Awake()
    {
        PlaneFinder = FindObjectOfType<FindPlane>();
    }

    void OnEnable()
    {
        PlaneFinder.OnValidPlaneFound += StorePlaneData;
        PlaneFinder.OnValidPlaneNotFound += RemovePlaneData;
    }

    void OnDisable()
    {
        PlaneFinder.OnValidPlaneFound -= StorePlaneData;
        PlaneFinder.OnValidPlaneNotFound -= RemovePlaneData;
    }

    void LateUpdate()
    {
        if (ShouldPlaceObject())
        {
            Instantiate(ObjectToPlace, Plane.Position, Plane.Rotation, transform);
        }
    }

    private void StorePlaneData(PlaneData Plane)
    {
        this.Plane = Plane;
    }

    private void RemovePlaneData()
    {
        Plane = null;
    }

    private bool ShouldPlaceObject()
    {
        if (Plane != null && Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }
        }

        return false;
    }
}
