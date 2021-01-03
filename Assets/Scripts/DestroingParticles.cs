using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroingParticles : MonoBehaviour {
	

	void FixedUpdate () {
        transform.localScale = new Vector2(transform.localScale.x - 0.4f, transform.localScale.y - 0.4f);
        if (transform.localScale.x <= 0)
        {
            GameObject Parent = transform.parent.gameObject;
            transform.parent = transform.parent;
            Destroy(gameObject);
            Destroy(Parent.gameObject);
        }
	}
}
