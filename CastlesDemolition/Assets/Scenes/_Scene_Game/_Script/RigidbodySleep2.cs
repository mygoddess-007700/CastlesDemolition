using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleep2 : MonoBehaviour
{
    private void Start()
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.angularVelocity = Vector3.zero;//try Quaternion.identity if this gives you any issues
        rb.velocity = Vector3.zero;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

}
