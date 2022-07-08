using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody rb;
    private float curveHight;
    private float gravity;
    public void Initialize(Vector3 targetPosition)
    {
        rb = GetComponent<Rigidbody>();
        gravity = Physics.gravity.y;
        curveHight = Vector3.Distance(targetPosition, transform.position) / -gravity;
        rb.centerOfMass = transform.forward * transform.lossyScale.z;
        rb.velocity = CalculateLaunchVelocity(targetPosition);
    }

    Vector3 CalculateLaunchVelocity(Vector3 targetPosition)
    {
        float displacementY = targetPosition.y - transform.position.y;
        Vector3 displacementXZ = new Vector3(targetPosition.x - transform.position.x, 0, targetPosition.z - transform.position.z);
        float value = (displacementY - curveHight) / gravity;
        float clampedCurve = Mathf.Clamp(value, 0, value);
        float time = Mathf.Sqrt(-2 * curveHight / gravity) + Mathf.Sqrt(2 * clampedCurve);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * curveHight);
        Vector3 velocityXZ = displacementXZ / time;
        return velocityXZ + velocityY * -Mathf.Sign(gravity);
    }

    private void Update()
    {
        transform.LookAt(transform.position + rb.velocity);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
