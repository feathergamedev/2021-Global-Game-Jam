using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic : MonoBehaviour
{
    [SerializeField] private BoxCollider2D m_collider;

    public virtual void GetTriggered(GameObject target) { }

    public void ShutDown()
    {
        m_collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetTriggered(collision.gameObject);
    }


}
