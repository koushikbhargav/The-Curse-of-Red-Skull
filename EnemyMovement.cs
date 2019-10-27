using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Animator anim;
    public GameObject player;
    public Vector3 dist;
    public float distMagnitude;
    [Range(0,200)]
    public float RangeFromPlayer;
    CharacterController charCont;
    public int randInt;
    public int randIntHit;
    public bool comboInAction = false;
    //public bool isCombo2playing = false;
    public bool isMonsterHit = false;
    public NavMeshAgent nav;
    public Vector3 dir;
    public bool isFlyBack = false;
    public bool haltCombo = false;
    public GameObject Armature;
    public float timer=0;
    public bool playerHurt;


    public bool LongRange = false;
    public float attackProb;
    public int LongRangeInt = 0;
    public bool isPerformingLongRangeAttack = false;


    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        charCont = GetComponent<CharacterController>();
        nav = GetComponent<NavMeshAgent>();
        
        StartCoroutine(ChooseAttackMode());
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("FlyBack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
            return;
       
        if (isPerformingLongRangeAttack == true && !anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            anim.ResetTrigger("Walk");
        if(isMonsterHit==true)
        {   if(timer<3f && (!anim.GetCurrentAnimatorStateInfo(0).IsName("StandUp")))
            timer += Time.deltaTime;
            else
            {
                timer = 0;
                isMonsterHit = false;
            }
        }
       else  if (isMonsterHit == false && playerHurt == false)
            haltCombo = false;
       
        this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        distMagnitude = dist.magnitude;
        dist = player.transform.position - this.transform.position;
        dist.y = 0;
        if(isMonsterHit==true)
        {
            anim.ResetTrigger("Walk");
        }
        else if(dist.magnitude<RangeFromPlayer && isMonsterHit==false)
        {
            

            Quaternion transRot = Quaternion.LookRotation(new Vector3( player.transform.position.x - this.transform.position.x, 0, player.transform.position.z - this.transform.position.z), transform.up);
            this.transform.rotation = Quaternion.Slerp(transRot, this.transform.rotation, 0.7f);
            if (dist.magnitude > 11f)
            {   
               
                if (LongRange==true )
                {
                    if (isPerformingLongRangeAttack == false)
                    {//anim.SetTrigger("GroundSmash");
                        SelectLongRangeAttack();
                        if (LongRangeInt == 0)
                        {
                            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Slash"))
                                anim.SetTrigger("GroundSmash");
                        }
                        else if (LongRangeInt == 1)
                        {
                            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("GroundSmash"))
                                anim.SetTrigger("Slash");
                            StartCoroutine(SetSlashPerformingLongeFalse());
                        }
                    }
                }
                else
                //charCont.Move(dist.normalized*10 * Time.deltaTime);
                {
                    
                    if ( !(anim.GetCurrentAnimatorStateInfo(0).IsName("FlyBack")) && !(anim.GetCurrentAnimatorStateInfo(0).IsName("StandUp")) )

                    {   if (isPerformingLongRangeAttack == true)
                        {
                            anim.SetTrigger("Idle");
                        }
                    else
                        {
                            anim.SetTrigger("Walk");
                            nav.Move(dist.normalized * 10 * Time.deltaTime);
                        }

                    }
                }
            }
            else
            {   if (!(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1))
                {   
                   
                    comboInAction = false;
                }
                
               // Debug.Log("Resetting walk");
                SelectCombo();
            }

        }
        else if( dist.magnitude>=RangeFromPlayer)
        {   
            
            if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("IdleMonster")))
                anim.SetTrigger("Idle");
            else if(playerHurt==true)
                anim.SetTrigger("Idle");
        }
        if (!charCont.isGrounded)
            charCont.Move( new Vector3(0,-4 * Time.deltaTime,0));


        this.gameObject.transform.position= new Vector3(this.transform.position.x,0,this.transform.position.z);

    
    }
    IEnumerator SetSlashPerformingLongeFalse()
    {
        yield return new WaitForSeconds(1.1f);
        isPerformingLongRangeAttack = false;
    }
   public void SelectLongRangeAttack()
    {
        isPerformingLongRangeAttack = true;
        LongRangeInt = Random.Range(0, 2);
       
        
    }
    public void SelectCombo()
    {

        //if (AttackProb >= 5f)
        {
            if (player.GetComponent<FlightMovement>().enabled == true)
                if (player.GetComponent<FlightMovement>().WaitForPlayerToRecover == true)
                    playerHurt = true;
                else playerHurt = false;

            if (comboInAction == false && haltCombo == false && playerHurt == false)
            {
                comboInAction = true;
                randInt = Random.Range(0, 2);
               // Debug.Log("Selecting randInt" + randInt);
                if (randInt == 0)
                    AttackCombo1();
                else AttackCombo2();
            }
        }
       /* else if(isMonsterHit==false && isFlyBack==false)
            anim.SetTrigger("ReadyIdle");
            */
       
        
    }
   public void AttackModeSelect()
    {
        //isPerformingLongRangeAttack = false;
        StartCoroutine(ChooseAttackMode());
    }
   IEnumerator ChooseAttackMode()
    {
        isPerformingLongRangeAttack = false;
        yield return new WaitForSeconds(3.5f);
        attackProb = Random.Range(0, 10);
        if (attackProb >= 4f)
            LongRange = true;
        else LongRange = false;

        if((anim.GetCurrentAnimatorStateInfo(0).IsName("GroundSmash") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime<1) || (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) )
        {
            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            StartCoroutine(ChooseAttackMode());
        }
        else
        StartCoroutine(ChooseAttackMode());
    }
    public void AttackCombo1()
    {
        
        anim.ResetTrigger("Walk");
        comboInAction = true;
        if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")) && haltCombo == false && !(anim.GetCurrentAnimatorStateInfo(0).IsName("FlyBack")))
        {
            if(!isMonsterHit && !isFlyBack)
                anim.SetTrigger("Combo1");
        }
        
    }
    public void ComboEnd()
    {
       // Debug.Log("Combo end called");
        comboInAction = false;
        
        SelectCombo();
    }
    public void AttackCombo2()
    {
       
        comboInAction = true;
        if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2")) && isMonsterHit == false && haltCombo == false && !(anim.GetCurrentAnimatorStateInfo(0).IsName("FlyBack")))

        {
            if (!isMonsterHit && !isFlyBack)
                anim.SetTrigger("Combo2");
        }
        

    }

    
 /*   public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("PlayerAttack"))
        {
            isMonsterHit = true;
            anim.SetTrigger("Fly Back");
        }
    }
    */
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAttack"))
        {   
            
            isMonsterHit = true;
            comboInAction = false;

            if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("FlyBack")) )
                anim.SetTrigger("Fly Back");
                
        }
        
         if (other.gameObject.CompareTag("MeleeAttack") )
        {
            if (player.GetComponent<FlightMovement>().isMelee && isMonsterHit==false && playerHurt==false && player.GetComponent<FlightMovement>().isPlayerKicking )
            {
                isMonsterHit = true;
                comboInAction = false;
                haltCombo = true;
                randIntHit = Random.Range(0,3);
                
                if (randIntHit == 2)
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                    {
                        if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit")) )
                        {


                            anim.SetTrigger("GetHit");
                            isFlyBack = true; //edited now
                            Vector3 hitPoint = other.ClosestPointOnBounds(this.gameObject.transform.position);
                            dir = (hitPoint - player.transform.position).normalized;
                            dir.y = 0;
                            StartCoroutine(FlyBack());

                            // this.gameObject.transform.parent.GetComponent<Animator>().Play("FlyBack");
                        }
                    }
                }
                
                 if (randIntHit == 0)
                {
                    if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("GotHit1")) && !(anim.GetCurrentAnimatorStateInfo(0).IsName("FlyBack")))
                    {

                        anim.SetTrigger("GotHit1");

                        Vector3 hitPoint = other.ClosestPointOnBounds(this.gameObject.transform.position);
                        dir = (hitPoint - player.transform.position).normalized;
                        dir.y = 0;
                        StartCoroutine(ResetIsMonsterHit1());
                        // this.gameObject.transform.parent.GetComponent<Animator>().Play("FlyBack");
                    }

                }
                else if (randIntHit == 1)
                {
                    if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("GotHit2")) && !(anim.GetCurrentAnimatorStateInfo(0).IsName("FlyBack")))
                    {

                        anim.SetTrigger("GotHit2");
                        Vector3 hitPoint = other.ClosestPointOnBounds(this.gameObject.transform.position);
                        dir = (hitPoint - player.transform.position).normalized;
                        dir.y = 0;
                        StartCoroutine(ResetIsMonsterHit1());
                        // this.gameObject.transform.parent.GetComponent<Animator>().Play("FlyBack");
                    }

                }


            }
        }
        
    }
    IEnumerator ResetIsMonsterHit1()
    {
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f);
            
        isMonsterHit = false;
    }
    IEnumerator ResetIsMonsterHit2()
    {
        yield return new WaitForSeconds(3f);
        if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("GotHit2")))
            isMonsterHit = false;
    }
    IEnumerator FlyBack()
    {
        isFlyBack = true;
        RootMotion();
        yield return new WaitForSeconds(1.03f);
        DisableRootMotion();
        //this.transform.position = Armature.transform.position;
        isFlyBack = false;
    }
    public void RootMotion()
    {
        this.gameObject.GetComponent<Animator>().applyRootMotion = true;
    }
    public void DisableRootMotion()
    {
        this.gameObject.GetComponent<Animator>().applyRootMotion = false;
    }

    /*  public void OnCollisionEnter(Collision other)
      {
          if (other.gameObject.CompareTag("MeleeAttack"))
          {
              if (player.GetComponent<FlightMovement>().isMelee)
              {
                  isMonsterHit = true;
                  comboInAction = false;
                  if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("FlyBack")))
                      anim.SetTrigger("Fly Back");



              }
          }
      }
      */
    public void FallBackEnded()
    {
        Debug.Log("Fall Back End called from Animation event");
        anim.ResetTrigger("Fly Back");
        anim.SetTrigger("Stand");
        isFlyBack = false;
        
    }
    public void StandUpEnded()
    {

        /*  if (dist.magnitude < RangeFromPlayer)
              anim.SetTrigger("Walk");
          else
          */
        anim.ResetTrigger("Stand");
        anim.ResetTrigger("Combo1");
        haltCombo = false;
        isFlyBack = false;
        anim.SetTrigger("Idle");

        StartCoroutine(DelayAfterStandUp());
        Debug.Log("StandUpEnded Called from Animation Event");

    }
    IEnumerator DelayAfterStandUp()
    {
        yield return new WaitForSeconds(0.1f);
        isMonsterHit = false;
        haltCombo = false;
    }
    
}
