using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform lookAt;
    Vector3 distance = Vector3.zero;

    private void Update()
    {
        distance = lookAt.position - transform.position;
        transform.forward = lookAt.position - transform.position;
        if (distance.magnitude > 15)
        {
            transform.position = Vector3.MoveTowards(transform.position, lookAt.position, 50 * Time.deltaTime);
        }
    }
}
