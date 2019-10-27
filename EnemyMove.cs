using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Animator anim;
    public GameObject skeletonRun;
    public GameObject skeletonDeath;
    
    public float dist;
    public float closeRange = 45.0f;
    public float speed = 10f;
    void Start()
    {
        //skeletonDeath.SetActive(true);
        //skeletonRun.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(player.transform.position, this.transform.position);
        if(Vector3.Distance(player.transform.position, this.transform.position) < closeRange && SkeletonHit.isDead!=true)
        {
            anim.Play("Run");
            //skeletonRun.SetActive(true);
            //skeletonDeath.SetActive(false);
            this.transform.position=Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
