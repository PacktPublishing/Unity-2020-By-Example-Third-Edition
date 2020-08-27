using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SightLine : MonoBehaviour
{
    public Transform EyePoint;
    public string TargetTag = "Player";
    public float FieldOfView = 45f;

    public bool IsTargetInSightLine { get; set; } = false;
    public Vector3 LastKnowSighting { get; set; } = Vector3.zero;

    private SphereCollider ThisCollider;

    void Awake()
    {
        ThisCollider = GetComponent<SphereCollider>();
        LastKnowSighting = transform.position;
    }

    void OnTriggerStay(Collider Other)
    {
        if (Other.CompareTag(TargetTag))
        {
            UpdateSight(Other.transform);
        }
    }

    void OnTriggerExit(Collider Other)
    {
        if (Other.CompareTag(TargetTag)) { IsTargetInSightLine = false; }
    }

    private bool HasClearLineofSightToTarget(Transform Target)
    {
        RaycastHit Info;

        Vector3 DirToTarget = (Target.position - EyePoint.position).normalized;
        if (Physics.Raycast(EyePoint.position, DirToTarget, out Info, ThisCollider.radius))
        {
            if (Info.transform.CompareTag(TargetTag))
            {
                return true;
            }
        }

        return false;
    }

    private bool TargetInFOV(Transform Target)
    {
        Vector3 DirToTarget = Target.position - EyePoint.position;
        float Angle = Vector3.Angle(EyePoint.forward, DirToTarget);

        if (Angle <= FieldOfView)
        {
            return true;
        }

        return false;
    }

    private void UpdateSight(Transform Target)
    {
        IsTargetInSightLine = HasClearLineofSightToTarget(Target) && TargetInFOV(Target);

        if (IsTargetInSightLine)
        {
            LastKnowSighting = Target.position;
        }
    }

}
