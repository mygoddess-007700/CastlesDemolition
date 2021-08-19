using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleep : MonoBehaviour
{
    private void Start()
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
            rb.Sleep();
    }
}
