using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class BeamAttack : MonoBehaviour
{
    public RaycastHit hit;
    int hitCount = 0;
    public static bool performBeamAttack;
    public bool isBeamAttack = false;
    public static bool performEagleAttack = false;
    public GameObject FireBeam;
    public GameObject BeamHeadParent;
    public Vector3 initPos;
    public static List<GameObject> spawnedBeams;
    public List<GameObject> BeamsSpawned;

    public GameObject EagleAttackObject;
    public GameObject LucyMoveObject;



    public GameObject EagleAttackPosition;
    public GameObject EagleMainObject;
    public int count;
    public List<GameObject> eagleComponents;
    public GameObject player;
    public static bool attackMode;
    public static bool EagleReady = false;


    public static bool PhoenixAttackReady = false;
    public GameObject PhoenixAttackComponents;
    public bool attackKeyPressed = false;
    Quaternion transRot2;
    public Vector3 hitpoint;


    public float BeamAttackFillDuration = 1f;
    public bool BeamAttackActivated = false;
    public Image BeamBarImg;

    public float EagleAttackFillDuration = 1f;
    public bool EagleAttackActivated = false;
    public Image EagleBarImg;

    public float PhoenixAttackFillDuration = 1f;
    public bool PhoenixAttackActivated = false;
    public Image PhoenixBarImg;


    
    

    void Start()
    {
        initPos = BeamHeadParent.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<MovementRajLucy>().enabled == false)
            return;
        if (BeamAttackFillDuration >= 1)
            BeamAttackActivated = false;
        if(BeamAttackActivated)
        {
            BeamAttackFillDuration += Time.deltaTime/7f ;
        }

        BeamBarImg.fillAmount = BeamAttackFillDuration;

        if (EagleAttackFillDuration >= 1)
            EagleAttackActivated = false;
        if (EagleAttackActivated)
        {
            EagleAttackFillDuration += Time.deltaTime / 7f;
        }

        EagleBarImg.fillAmount = EagleAttackFillDuration;


        if (PhoenixAttackFillDuration >= 1)
            PhoenixAttackActivated = false;
        if (PhoenixAttackActivated)
        {
            PhoenixAttackFillDuration += Time.deltaTime / 7f;
        }

        PhoenixBarImg.fillAmount = PhoenixAttackFillDuration;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.P) && BeamAttackFillDuration>=1)
        {
            BeamAttackActivated = true;
            BeamAttackFillDuration = 0;
            if (Physics.Raycast(ray, out hit))
            {   
                attackKeyPressed = true;
                hitCount++;
                hit.point = new Vector3(hit.point.x, 1.5f, hit.point.z);
                Debug.Log(hit.point);
                hitpoint = hit.point;
                
            }
            isBeamAttack = true;
            this.gameObject.GetComponent<MovementRajLucy>().spellCasting = true;
            LucyMoveObject.GetComponent<Animator>().SetTrigger("spell");
            StartCoroutine(MouseLock());
            StartCoroutine(TurningTime());
            // spellcasting reset through animation event
            /* performBeamAttack = true;
             FireBeam.SetActive(true);
             StartCoroutine(MouseLock());
             StartCoroutine(TurningTime());
             */
        }
        if (Input.GetKeyDown(KeyCode.Q) && EagleAttackFillDuration>=1)
        {
            EagleAttackActivated = true;
            EagleAttackFillDuration = 0;
            attackKeyPressed = true;
            if (Physics.Raycast(ray, out hit))
            {
                hit.point = new Vector3(hit.point.x, 1.5f, hit.point.z);
                Debug.Log(hit.point);
                hitCount++;
            }
            
            performEagleAttack = true;
            this.gameObject.GetComponent<MovementRajLucy>().spellCasting = true;
            LucyMoveObject.GetComponent<Animator>().SetTrigger("phoenixSummon");
            EagleAttackObject.SetActive(true);
            StartCoroutine(MouseLock());
            StartCoroutine(TurningTime());
            Release();

        }
        if(Input.GetKeyDown(KeyCode.Alpha1)&& PhoenixAttackFillDuration >= 1 )
        {
            PhoenixAttackActivated = true;
            PhoenixAttackFillDuration = 0;
            attackKeyPressed = true;
            if (Physics.Raycast(ray, out hit))
            {
                hit.point = new Vector3(hit.point.x, 1.5f, hit.point.z);
                Debug.Log(hit.point);
                hitCount++;
            }
            
            PhoenixAttackReady = true;
            this.gameObject.GetComponent<MovementRajLucy>().spellCasting = true;
            LucyMoveObject.GetComponent<Animator>().ResetTrigger("run");
            LucyMoveObject.GetComponent<Animator>().SetTrigger("phoenixSummon");
            StartCoroutine(TurningTime());
            ReleasePhoenix();
        }
        if(Input.GetMouseButtonDown(0))
        {
            attackKeyPressed = true;
            if (Physics.Raycast(ray, out hit))
            {
                hit.point = new Vector3(hit.point.x, 1.5f, hit.point.z);
                Debug.Log(hit.point);
                hitCount++;
              //  this.gameObject.GetComponent<activateVFX>().KamehamehaPointer.transform.position = hit.point;
            }
            StartCoroutine(TurningTime());
        }
      /*  if (isBeamAttack == true)
            FireBeam.SetActive(true);
        else FireBeam.SetActive(false);
        */
        if (hitCount>0 && this.gameObject.GetComponent<MovementRajLucy>().enabled==false)
        {
            this.gameObject.GetComponent<MovementRajLucy>().newPosition = hit.point;
           // transRot2 = Quaternion.LookRotation(hit.point - this.transform.position, Vector3.up);
           // player.transform.rotation = Quaternion.Slerp(transRot2, player.transform.rotation, 0.7f);
        }
        //player.transform.rotation = transRot2;
        if (!FireBeam.activeInHierarchy)
        {
            if (BeamsSpawned.Count > 0)
            {
                foreach (GameObject g in BeamsSpawned)
                    Destroy(g);
            }
        }
        BeamsSpawned = spawnedBeams;

       
    }
    public void Release()
    {
        GameObject go = Instantiate(EagleMainObject, player.transform);
        go.transform.localPosition = EagleAttackPosition.transform.localPosition;
        
    }
    public void ReleasePhoenix()
    {
        GameObject go = Instantiate(PhoenixAttackComponents,player.transform);
        Destroy(go, 20f);
        
        //go.transform.localPosition = EagleAttackPosition.transform.localPosition;

    }
    public void BeamAttackAnimation()
    {
        // LucyMoveObject.GetComponent<Animator>().SetTrigger("spell");
        // spellcasting reset through animation event
        
        performBeamAttack = true;
        FireBeam.SetActive(true);
        StartCoroutine(MouseLock());
        
    }
    public void ResetSpellCasting()
    {
        performBeamAttack = false;
        this.gameObject.GetComponent<MovementRajLucy>().spellCasting = false;
        BeamHeadParent.GetComponent<BeamHead>().ResetBeamHeadPos();
        isBeamAttack = false;
    }
    IEnumerator TurningTime()
    {
        this.gameObject.GetComponent<MovementRajLucy>().newPosition = hit.point;
        this.gameObject.GetComponent<MovementRajLucy>().spellCasting = true;
        yield return new WaitForSeconds( 5f);
        this.gameObject.GetComponent<MovementRajLucy>().enabled = true;
        attackKeyPressed = false;
        this.gameObject.GetComponent<MovementRajLucy>().spellCasting = false;
        performBeamAttack = false;
        this.gameObject.GetComponent<MovementRajLucy>().spellCasting = false;
        //this.gameObject.GetComponent<Animator>().Play("idle");
    }
    IEnumerator MouseLock()
    {
        Cursor.visible = (false);
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(3f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = (true);
    }
}
