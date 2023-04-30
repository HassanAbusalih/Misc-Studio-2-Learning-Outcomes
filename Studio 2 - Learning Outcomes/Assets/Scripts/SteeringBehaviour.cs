using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SteeringBehaviour : MonoBehaviour
{
    [SerializeField] float edgeValue;
    [SerializeField] float speed;
    [SerializeField] float circleDistance;
    [SerializeField] float circleRadius;
    float angle = 20;
    [SerializeField] [Range(0, 2)] float wanderMagnitude = 5;
    [SerializeField] [Range(0, 30)] float arrivalRange;
    [SerializeField] [Range(0, 30)] float arrivalDistance;
    [SerializeField] [Range(0, 2)] float arrivalMagnitude;
    [SerializeField] [Range(0, 2)] float alignmentMagnitude;
    [SerializeField] [Range(0, 2)] float avoidanceMagnitude = 5;
    [SerializeField] [Range(0, 20)] float avoidanceRange = 5;
    [SerializeField] [Range(0, 30)] float angleRange = 5;
    [SerializeField] Transform circle;
    [SerializeField] Transform pointOnCircle;
    SteeringBehaviour[] otherWanderers;
    Rigidbody rb;
    public Rigidbody Rb => rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        otherWanderers = FindObjectsOfType<SteeringBehaviour>();
        rb.velocity = transform.forward * speed;
    }

    void FixedUpdate()
    {
        Vector3 avoidance = AvoidanceForce();
        Vector3 arrivalForce = ArrivalForce();
        Vector3 alignmentForce = AlignmentForce();
        Vector3 wanderForce = WanderForce();
        rb.velocity += arrivalForce + avoidance + alignmentForce + wanderForce;
        rb.velocity = rb.velocity.normalized * speed;
        transform.forward = rb.velocity;
        SnapToEdge();
    }

    Vector3 WanderForce()
    {
        Vector3 circlePos  = transform.position + transform.forward * circleDistance;
        angle += Random.Range(-angleRange, angleRange) * Time.deltaTime;
        Vector3 targetPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), Mathf.Sin(angle)) * circleRadius;
        targetPos += circlePos;
        if (pointOnCircle.gameObject.activeSelf && circle.gameObject.activeSelf) 
        {
            pointOnCircle.position = targetPos;
            circle.position = circlePos;
        }
        Vector3 wanderforce = targetPos - transform.position;
        return Vector3.ClampMagnitude(wanderforce, wanderMagnitude);
    }

    Vector3 AvoidanceForce()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, avoidanceRange);
        if (colliders.Length == 0)
        {
            return Vector3.zero;
        }
        Vector3 avoidanceForce = Vector3.zero;
        foreach (Collider collider in colliders)
        {
            Vector3 distance = transform.position - collider.transform.position;
            if (distance.magnitude < avoidanceRange)
            {
                avoidanceForce += distance;
            }
        }
        avoidanceForce /= colliders.Length;
        return Vector3.ClampMagnitude(avoidanceForce, avoidanceMagnitude);
    }

    Vector3 ArrivalForce()
    {
        Vector3 arrivalForce = Vector3.zero;
        Vector3 direction = Vector3.zero;
        int counter = 0;
        foreach(SteeringBehaviour wanderer in otherWanderers)
        {
            Vector3 distance = wanderer.transform.position - transform.position;
            if (distance.magnitude <= arrivalRange)
            {
                direction += wanderer.transform.position;
                counter++;
            }
        }
        if (counter != 0)
        {
            direction /= counter;
            arrivalForce = (direction - transform.position).normalized;
        }
        return Vector3.ClampMagnitude(arrivalForce, arrivalMagnitude);
    }

    Vector3 AlignmentForce()
    {
        Vector3 alignmentForce = Vector3.zero;
        int counter = 0;
        foreach (SteeringBehaviour wanderer in otherWanderers)
        {
            Vector3 distance = wanderer.transform.position - transform.position;
            if (distance.magnitude <= arrivalRange)
            {
                alignmentForce += wanderer.Rb.velocity;
                counter++;
            }
        }
        if (counter != 0)
        {
            alignmentForce /= counter;
        }
        return Vector3.ClampMagnitude(alignmentForce, alignmentMagnitude);
    }

    void SnapToEdge()
    {
        Vector3 position = transform.position;
        Vector3 newposition = Vector3.zero;
        if (position.x > edgeValue)
        {
            newposition.x = -edgeValue;
        }
        else if (position.x < -edgeValue)
        {
            newposition.x = edgeValue;
        }
        else
        {
            newposition.x = position.x;
        }
        if (position.y > edgeValue)
        {
            newposition.y = -edgeValue;
        }
        else if (position.y < -edgeValue)
        {
            newposition.y = edgeValue;
        }
        else
        {
            newposition.y = position.y;
        }
        if (position.z > edgeValue)
        {
            newposition.z = -edgeValue;
        }
        else if (position.z < -edgeValue)
        {
            newposition.z = edgeValue;
        }
        else
        {
            newposition.z = position.z;
        }
        transform.position = newposition;
    }
}
