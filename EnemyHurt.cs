using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHurt : MonoBehaviour
{
    public GameObject Monster;
    public float HurtScale = 0.2f;
    public float health;
    public Image HealthBarBorder;
    public Image HealthBar;
    void Start()
    {
        GameObject.Find("Shockwaves").GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 10f, 0));
        HealthBarBorder.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 10f, 0));
        HealthBar.fillAmount = health;
    }
    public void OnTriggerEnter(Collider other)
    {
        /*  if(other.gameObject.CompareTag("Enemy Hit") && other.gameObject.transform.root.GetComponent<EnemyMovement>().comboInAction==true)
          {
             // player.GetComponent<FlightMovement>().LucyGetsHit();
              if(player.GetComponent<PlayerHealth>().Health>0)
              player.GetComponent<PlayerHealth>().Health -= 0.2f;
          }
          */
        if (other.gameObject.CompareTag("MeleeAttack"))
        {  //if (other.gameObject.GetComponent<FlightMovement>().isPlayerKicking)
            
                {
                    Debug.Log("Hit by player melee");

                    if (health > 0)
                        health -= 0.1f;
                  //  GameObject.Find("Shockwaves").GetComponent<ParticleSystem>().Play();
                   // StartCoroutine(TurnShockwavesOff());

                }
        }

            else if (other.gameObject.CompareTag("PlayerAttack"))
            {
                Debug.Log("Hit by player energy attack");
                if (health > 0)
                    health -= HurtScale * 2;
               // GameObject.Find("Shockwaves").GetComponent<ParticleSystem>().Play();
               // StartCoroutine(TurnShockwavesOff());
            }


    }
    IEnumerator TurnShockwavesOff()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject.Find("Shockwaves").GetComponent<ParticleSystem>().Stop();
    }
}
