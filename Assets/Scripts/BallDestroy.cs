using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallDestroy : MonoBehaviour {
    public GameObject CoinsParticles;
    public GameObject PigCoinsParticles;
    public enum TypeOfBalls {BallClassical, BallAdd, BallDouble, BallCoin, BallPig, BallBomb, BallComet, BallBreaker }
    public TypeOfBalls TypeOfBall;
    Animator anim;
    int HillPoint;
    public int hillPoint
    {
        get { return HillPoint; }
        set { if (value < 0) HillPoint = 0; else HillPoint = value; }
    }
    Text HillPointText;
    int killPoint;
    public bool isNeedTakeHpFromGame = true;       // если в тру то берез изначальное хп в классе game иначе не берет его а остается с тем что есть

    void Start () {
        anim = GetComponent<Animator>();
        HillPointText = transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();

        if(isNeedTakeHpFromGame) hillPoint = Game.SpawnBallHillPoint;

        HillPointText.text = hillPoint.ToString();
        // значение хп для бонусных шаров
        if (TypeOfBall == TypeOfBalls.BallBomb || TypeOfBall == TypeOfBalls.BallCoin || TypeOfBall == TypeOfBalls.BallPig || TypeOfBall == TypeOfBalls.BallBreaker) hillPoint = 1;
    }
	
	void Update () {
        if (hillPoint <= 0)
        {
            if(TypeOfBall == TypeOfBalls.BallComet)
            {
                Ball.ballComet = true;
            }
            if (TypeOfBall == TypeOfBalls.BallAdd)
            {
                Game.MainBallHillPoint+=Game.AddHpCount;
                GameObject AddPointText = transform.GetChild(0).GetChild(1).gameObject;
                GameObject AddText = Instantiate(AddPointText, transform.position, AddPointText.transform.rotation, transform);
                AddText.transform.SetParent(GameObject.FindGameObjectWithTag("CanvasGame").transform, true);
                AddText.SetActive(true);
            }
            /*else if (TypeOfBall == TypeOfBalls.BallDouble)
            {
                Ball.balldouble = true;
            }*/
            if (TypeOfBall == TypeOfBalls.BallCoin)
            {
                GameObject Part = Instantiate(CoinsParticles, transform.position, transform.rotation, transform.parent.parent.parent);
                Destroy(Part, 5);
                Game.coins++;
                PlayerPrefs.SetInt("Coins", Game.coins);
            }
            else if (TypeOfBall == TypeOfBalls.BallPig)
            {
                GameObject Part = Instantiate(PigCoinsParticles, transform.position, transform.rotation, transform.parent.parent.parent);
                Destroy(Part, 3);
                Game.coins += 5;
                PlayerPrefs.SetInt("Coins", Game.coins);
            }
            if (TypeOfBall == TypeOfBalls.BallBomb)
            {
                gameObject.GetComponent<Animator>().SetBool("Explosion", true);
                
            }
            else if(TypeOfBall == TypeOfBalls.BallClassical) Destroy(gameObject);

            // удаление объекта
            if (TypeOfBall != TypeOfBalls.BallBomb) Destroy(gameObject);
        }
        HillPointText.text = hillPoint.ToString();
    }

    void EndTakeDamage()
    {
        anim.SetBool("TakeDamage", false);
    }

    void BoombExplosion()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            killPoint = Ball.hillPoint;
            Game.counter += (Ball.hillPoint - hillPoint >=0 ? hillPoint : Ball.hillPoint);
            Ball.hillPoint -= hillPoint;
            hillPoint -= killPoint;
            if (hillPoint <= 0 && TypeOfBall == TypeOfBalls.BallDouble) ball.balldouble = true;
            else if (TypeOfBall == TypeOfBalls.BallBreaker)
            {
                Ball.hillPoint = 0;
                collision.gameObject.GetComponent<Ball>().rigidBody.velocity = Vector2.zero;
            }
        }
        if (collision.gameObject.tag == "BallDouble")
        {
            BallDouble ball = collision.gameObject.GetComponent<BallDouble>();
            killPoint = ball.hillPoint;
            Game.counter += ball.hillPoint - hillPoint >= 0 ? hillPoint : ball.hillPoint;
            ball.hillPoint -= hillPoint;
            hillPoint -= killPoint;
            if (hillPoint <= 0 && TypeOfBall == TypeOfBalls.BallDouble) ball.balldouble = true;
            else if (TypeOfBall == TypeOfBalls.BallBreaker) ball.hillPoint = 0;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BoombExplosion")
        {
            if(anim != null) anim.SetBool("TakeDamage", true);
            if (hillPoint > 10) hillPoint -= (int)( hillPoint * ((float)Game.BoombDamage/100));
            else hillPoint = 0;
        }
        if (collision.gameObject.tag == "Ball")
        {
            if (anim != null) anim.SetBool("TakeDamage", true);
            if (hillPoint > 10) hillPoint -= (int)(hillPoint * ((float)Game.BoombDamage / 100));
            else hillPoint = 0;
        }
    }
}
