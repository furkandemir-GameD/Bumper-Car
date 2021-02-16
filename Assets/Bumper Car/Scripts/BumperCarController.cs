using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BumperCarController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distDivision;
    [SerializeField] private GameObject wheel;
    [SerializeField] private Transform wheelFirstRotation;
    [SerializeField] private float blobEffectActiveSecond;
    [SerializeField] private float rotSpeed = 250;
    [SerializeField] private float damping = 10;
    [SerializeField] private float Speed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private GameObject bloodEffect;
    private float distance;
    private Rigidbody _rb;
    private float startPos;
    private Vector3 impulseForce;
    private void Start()
    {
        _rb = this.gameObject.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition.x;
        }
        if (Input.GetMouseButton(0))
        { // TODO: Car Mechanic
            _rb.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            distance = Input.mousePosition.x - startPos;
            Speed = _rb.velocity.magnitude;
            angularSpeed = _rb.angularVelocity.magnitude*Mathf.Rad2Deg;
            var q = Quaternion.AngleAxis(45, transform.up);
            float angle;
            Vector3 axis;
            q.ToAngleAxis(out angle, out axis);
            _rb.angularVelocity = axis * angle * Mathf.Deg2Rad* distance / 200;
            _rb.AddForce(transform.forward * speed * Time.fixedDeltaTime,ForceMode.Acceleration);
            if (distance < 0)
            {
                wheel.transform.eulerAngles = new Vector3(wheel.transform.eulerAngles.x, wheel.transform.eulerAngles.y, wheel.transform.eulerAngles.z + -distance / 3 * Time.fixedDeltaTime);
            }
            else
            {
                wheel.transform.eulerAngles = new Vector3(wheel.transform.eulerAngles.x, wheel.transform.eulerAngles.y, wheel.transform.eulerAngles.z + -distance / 3 * Time.fixedDeltaTime);
            }
        }
        else
        {
            wheel.transform.rotation = Quaternion.Lerp(wheel.transform.rotation, wheelFirstRotation.transform.rotation,5f*Time.deltaTime);
            _rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _rb.angularVelocity = Vector3.zero;
            _rb.angularDrag = 0;
            _rb.velocity /= 1.5f;
        } 
    }
    private void OnCollisionEnter(Collision collision)
    { // TODO: Collision Mechanic
        if (collision.gameObject.CompareTag("AICars"))
        {
            _rb = transform.GetComponent<Rigidbody>();
            impulseForce = collision.impactForceSum;
            collision.rigidbody.AddForce(-impulseForce * collision.rigidbody.mass, ForceMode.Impulse);
            _rb.constraints = RigidbodyConstraints.FreezePositionY;
            Taptic.Heavy();
            bloodEffect.SetActive(true);
            Run.After(blobEffectActiveSecond, async() =>{
                bloodEffect.SetActive(false);
            });
        }
    }
}
