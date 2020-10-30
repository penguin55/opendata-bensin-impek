using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public bool insight;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TakeDamage(insight);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject;
            if (Data.instance.Character.PlayerDashing())
            {
                Debug.Log("Enemy Stun!");
                StartCoroutine(DelayAttack());
            }

            if (!Data.instance.Character.PlayerDashing())
            {
                insight = true;

            }

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            insight = false;
            player = null;
        }
    }

    private void TakeDamage(bool insight)
    {
        if (player != null)
        {
            double distance = Math.Sqrt(Math.Pow(player.transform.position.x - this.transform.position.x, 2)
                    + Math.Pow(player.transform.position.y - this.transform.position.y, 2));


            if (distance < 4 && insight)
            {
                StartCoroutine(StopAttack());
            }
        }
    }

    IEnumerator StopAttack()
    {
        player.GetComponent<CharaBehaviour>().TakeDamage();
        insight = false;
        yield return new WaitForSeconds(2f);
        insight = true;
    }

    IEnumerator DelayAttack()
    {
        insight = false;
        yield return new WaitForSeconds(2f);
        insight = true;
    }
}
