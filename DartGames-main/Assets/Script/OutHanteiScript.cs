using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutHanteiScript : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "IsTarget" && !collision.gameObject.GetComponent<Dart>().IsOnBoard())
        {
            collision.gameObject.GetComponent<Dart>().IsOnBoard(true);
            collision.gameObject.GetComponentInParent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            collision.gameObject.GetComponentInParent<Rigidbody>().useGravity = false;
            collision.gameObject.GetComponentInParent<Rigidbody>().isKinematic = true;
            collision.gameObject.GetComponentInChildren<CircleCollider2D>().enabled = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {

            if (collision.gameObject.GetComponent<DartScore>().scoreUpdated == false)
            {

                collision.gameObject.GetComponent<DartScore>().scoreUpdated = true;
                int score = 0;
                int multiplier = 1;
                collision.gameObject.GetComponent<DartScore>().score = score;
                ScoreSystem scoreSystem = GameObject.Find("GameController").GetComponent<ScoreSystem>();
                scoreSystem.ChangePoint(score, multiplier);
            }
        }
    }


}
