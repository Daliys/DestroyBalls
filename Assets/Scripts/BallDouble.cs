using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallDouble : MonoBehaviour {
    Vector2 RigidVelocity;
    public GameObject DestroingParticles;
    public bool balldouble;
    public int hillPoint
    {
        get { return HillPoint; }
        set { if (value < 0) HillPoint = 0; else HillPoint = value; }
    }
    int HillPoint = 1;
    private Rigidbody2D rigidBody;
    Text HillPointText;
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        HillPointText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        //hillPoint = Ball.hillPoint;
    }
	
	void Update () {

        HillPointText.text = hillPoint.ToString();
        //Game.isGenerate = false;
        if (hillPoint <= 0)
        {
            GameObject i = Instantiate(DestroingParticles, transform.position, transform.rotation, transform.parent);
            i.GetComponent<DestroingGroupParticles>().RigidVelocity = RigidVelocity;
            i.GetComponent<DestroingGroupParticles>().color = GetComponent<SpriteRenderer>().color;
            Destroy(i, 5);
            HillPointText.text = null;
            Destroy(gameObject);
            Game.isChekingClearField = true;
        }
        if (transform.position.y <= -10 || transform.position.y >= 10)
        {
            Destroy(gameObject);
            Game.isChekingClearField = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "BallDestroy") RigidVelocity = rigidBody.velocity;
        if (collision.gameObject.tag == "BallDestroy" && balldouble && hillPoint > 0)
        {
            GameObject d = Instantiate(gameObject, transform.position, transform.rotation, null);
            d.GetComponent<BallDouble>().hillPoint = hillPoint;
            d.GetComponent<Rigidbody2D>().velocity = new Vector2(-rigidBody.velocity.x, rigidBody.velocity.y);
            balldouble = false;
        }
    }
}
