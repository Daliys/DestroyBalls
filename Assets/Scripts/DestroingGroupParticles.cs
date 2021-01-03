using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroingGroupParticles : MonoBehaviour
{
    public GameObject[] Particles;
    public Vector2 RigidVelocity;
    public Color color;
    private void Start()
    {
        if (Particles.Length > 0)
        {
            foreach (var item in Particles)
            {
                item.GetComponent<SpriteRenderer>().color = color;
                item.GetComponent<Rigidbody2D>().velocity = RigidVelocity;
            }
        }

    }
}
