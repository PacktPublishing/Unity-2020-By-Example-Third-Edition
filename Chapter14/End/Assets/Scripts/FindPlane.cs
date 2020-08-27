using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Events;

public class PlaneData
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
}

public class FindPlane : MonoBehaviour
{
    public UnityAction<PlaneData> OnValidPlaneFound;
    public UnityAction OnValidPlaneNotFound;

    private ARRaycastManager RaycastManager;
    private readonly Vector3 ViewportCenter = new Vector3(0.5f, 0.5f);

    void Awake()
    {
        RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        IList<ARRaycastHit> hits = GetPlaneHits();
        UpdateSubscribers(hits);
    }

    private List<ARRaycastHit> GetPlaneHits()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(ViewportCenter);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        RaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);
        return hits;
    }

    private void UpdateSubscribers(IList<ARRaycastHit> hits)
    {
        bool ValidPositionFound = hits.Count > 0;
        if (ValidPositionFound)
        {
            PlaneData Plane = new PlaneData
            {
                Position = hits[0].pose.position,
                Rotation = hits[0].pose.rotation 
            };
  
            OnValidPlaneFound?.Invoke(Plane);
        }
        else
        {
            OnValidPlaneNotFound?.Invoke();
        }
    }

}
