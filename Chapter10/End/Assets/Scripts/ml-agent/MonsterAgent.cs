using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class MonsterAgent : Agent
{
    public float MoveSpeed = 1.0f;
    public float TurnSpeed = 90.0f;
    public float MaxVelocity = 10.0f;

    private Rigidbody ThisRigidbody;
    private ObjectSpawner SceneObjectSpawner;

    void Awake()
    {
        ThisRigidbody = GetComponent<Rigidbody>();
        SceneObjectSpawner = FindObjectOfType<ObjectSpawner>();
    }

    public override void OnActionReceived(float[] VectorAction)
    {
        var MovementAction = (int)VectorAction[0];
        var RotationAction = (int)VectorAction[1];

        var MovementDir = Vector3.zero;
        if (MovementAction == 1)
        {
            MovementDir = transform.forward;
        }
        else if (MovementAction == 2)
        {
            MovementDir = -transform.forward;
        }

        var RotationDir = Vector3.zero;
        if (RotationAction == 1)
        {
            RotationDir = -transform.up;
        }
        else if (RotationAction == 2)
        {
            RotationDir = transform.up;
        }

        ApplyMovement(MovementDir, RotationDir);
    }

    public override void OnEpisodeBegin()
    {
        SceneObjectSpawner.Reset();
        ThisRigidbody.velocity = Vector3.zero;
        transform.position = new Vector3(0, 2, 0);
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void ApplyMovement(Vector3 MovementDir, Vector3 RotationDir)
	{
        ThisRigidbody.AddForce(MovementDir * MoveSpeed, ForceMode.VelocityChange);
        transform.Rotate(RotationDir, Time.fixedDeltaTime * TurnSpeed);


        if (ThisRigidbody.velocity.sqrMagnitude > MaxVelocity)
        {
            ThisRigidbody.velocity *= 0.95f;
        }
    }

    void OnCollisionEnter(Collision OtherCollision)
    {
        if (OtherCollision.gameObject.CompareTag("Chick"))
        {
            Destroy(OtherCollision.gameObject);
            SceneObjectSpawner.SpawnFood();
            AddReward(2f);

        }
        else if (OtherCollision.gameObject.CompareTag("Rock"))
        {
            Destroy(OtherCollision.gameObject);
            SceneObjectSpawner.SpawnRock();
            AddReward(-1f);
        }
        else if (OtherCollision.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);
        }
    }
}
