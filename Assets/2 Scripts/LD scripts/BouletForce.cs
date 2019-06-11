using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouletForce : MonoBehaviour {

    public float m_speed = 5;
    public int destroyBullet = 5;

    Rigidbody m_rbody;

    private void Start()
    {
        m_rbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        m_rbody.velocity = new Vector3(0, - m_speed, 0);
        Destroy(gameObject, destroyBullet);
    }
}
