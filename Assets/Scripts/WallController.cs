using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public float wallHealth = 7F;

    public void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude > wallHealth)
        {
            Collapse();
        }
    }

    private void Collapse()
    {
        Destroy(gameObject);
    }
}
