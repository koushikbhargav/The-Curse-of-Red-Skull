using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 newPosition;
    public float range=1.0f;
    public float speed = 5.0f;
    public GameObject PlayerObject;
    public Animator anim;
    RaycastHit hit;
    RaycastHit hit0;
    public Vector3 hitpoint;
    public int attackID = 1;

    public static bool lockCursor = false;

    //public GameObject markerPrefab;
    //Quaternion transRot;
    public GameObject go;
    public static Vector3 attackPos;
    public bool LMB = false;
    public bool LeftClickPressed = false;
    public GameObject LeftClickMarker;
    public GameObject orgMegumin;
    public GameObject Megumin3;
    public GameObject Megumin4;
    public GameObject Megumin5;

    public bool run;
    public bool spellCasting = false;
    //VFX
    public GameObject MagicCircle1;

    //VFX

    //UI
    //public Image spellBar1;
    //public Image spellBar2;
    public float spell1FillAmt = 0.0f;
    public float spell2FillAmt = 0.0f;
    public float spell3FillAmt = 0.0f;
    //public static float spell1FillAmtStatic = 0.0f;
    //public static float spell2FillAmtStatic = 0.0f;
    public static bool spell1Flag = false;
    public static bool spell2Flag = false;
    public static bool spell3Flag = false;

    public Image spell1Bar;
    public Image spellBar2;
    public Image spellBar3;

    public GameObject attack1UI;
    public GameObject attack2UI;

    
    //UI
    

    void Start()
    {
        attack1UI.SetActive(false);
        attack2UI.SetActive(false);
        attackID = 2;
        LMB = false;
        go.SetActive(false);
        run = false;
        newPosition = this.transform.position;
        spell1FillAmt = 1.0f;
        spell2FillAmt = 1.0f;
        spell3FillAmt = 1.0f;
        StartCoroutine(ChangeAttackID());
    }

    // Update is called once per frame
    void Update()
    {
        spell1Bar.fillAmount = spell1FillAmt;
        spellBar2.fillAmount = spell2FillAmt;
        spellBar3.fillAmount = spell3FillAmt;
        //spellBar1.fillAmount = spell1FillAmt;
        //spellBar2.fillAmount = spell2FillAmt;
        //spell1FillAmtStatic = spell1FillAmt;
        //spell2FillAmtStatic = spell2FillAmt;
        bool RMB = Input.GetMouseButtonDown(1);
        LMB = Input.GetMouseButtonDown(0);
        if (LMB)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit0) && hit0.transform.tag == "Ground" && hit0.transform.tag != "hit" && hit0.transform.tag!="btn")
            //if (Physics.Raycast(ray, out hit0) && hit0.transform.tag != "hit")
            {

                attackPos = hit0.point;
                if (Vector3.Distance(hit0.point, this.transform.position) > range && spellCasting != true)
                {
                    
                    Quaternion transRot2 = Quaternion.LookRotation(attackPos - this.transform.position, Vector3.up);
                    //PlayerObject.transform.rotation = Quaternion.Slerp(transRot2, PlayerObject.transform.rotation, 0.7f);
                    PlayerObject.transform.rotation = transRot2;
                }
                if (attackID == 1)
                {
                    activateVFX.firetornado = true;
                    activateVFX.kamehameha = false;
                    activateVFX.bigfiretornado = false;
                }
                else if (attackID == 2)
                {
                    activateVFX.firetornado = false;
                    activateVFX.kamehameha = false;
                    activateVFX.bigfiretornado = true;
                    
                }
                else if (attackID == 3)
                {
                    activateVFX.firetornado = false;
                    activateVFX.bigfiretornado = false;
                    activateVFX.kamehameha = true;

                }
                //activateVFX.firetornado = true;
                //activateVFX.attackIndex = attackID;
                //StartCoroutine(FireTornadoAttack());
                StartCoroutine(SpellAttack(attackID));
                
                
               

                //spellCasting = true;
                //LeftClickPressed = true;
                //StartCoroutine(setAttackMarker());
            }
        }
        if (RMB)
        {
            //RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Ground")
            {
                newPosition = hit.point;
                newPosition.y = 1.5f;
            }

        }
        
        if(Vector3.Distance(newPosition, this.transform.position) > range && spellCasting!=true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, newPosition, speed * Time.deltaTime);
            Quaternion transRot = Quaternion.LookRotation(newPosition - this.transform.position, Vector3.up);
            PlayerObject.transform.rotation = Quaternion.Slerp(transRot, PlayerObject.transform.rotation, 0.7f);
        }
        hitpoint = this.transform.position;
        go.transform.position = hit.point;
        LeftClickMarker.transform.position = hit0.point;
        LeftClickMarker.SetActive(true);
        
        if (Mathf.Abs(this.transform.position.x - hit.point.x) > 1 && Mathf.Abs(this.transform.position.z - hit.point.z) > 1)  
        {
            run = true;
            anim.Play("run");
            go.SetActive(true);
        }
        else if (Mathf.Abs(this.transform.position.x - hit.point.x) < 1 && Mathf.Abs(this.transform.position.z - hit.point.z) < 1)
        {
            run = false;
            anim.Play("idle");
            go.SetActive(false);
        }
        //if (spellCasting)
        //{
        //    //anim.Play("spell1");
        //}
        //if (run)
        //{
        //    anim.settrigger("run");
        //}
        //else if (!run)
        //{
        //    anim.settrigger("idle");
        //}
        if (Input.GetKey(KeyCode.Alpha1))
        {
            //spellCasting = true;
            
            attackID = 1;
            
            //StartCoroutine(FireTornadoAttack());
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            //spellCasting = true;

            attackID = 2;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            //spellCasting = true;

            attackID = 3;
        }
        if (spell1FillAmt <= 1.0f)
        {
            spell1FillAmt += Time.deltaTime * 0.2f;
        }
        if (spell2FillAmt <= 1.0f)
        {
            spell2FillAmt += Time.deltaTime * 0.2f;
        }
        if (spell3FillAmt <= 1.0f)
        {
            spell3FillAmt += Time.deltaTime * 0.2f;
        }
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        
        //GameObjectActivation
        




        //GameObjectActivation
    }
    IEnumerator ChangeAttackID()
    {
        if (attackID == 2)
        {

            yield return new WaitForSeconds(6f);
            attackID = 3;
            StartCoroutine(ChangeAttackID());
        }
        else
        {
            yield return new WaitForSeconds(6f);
            attackID = 2;
            StartCoroutine(ChangeAttackID());
        }

    }
    public void setAttackID3()
    {
        attackID = 3;
    }
    public void setAttackID2()
    {
        attackID = 2;
    }

    IEnumerator setAttackMarker()
    {
        LeftClickMarker.transform.position = hit0.point;
        LeftClickMarker.SetActive(true);
        yield return new WaitForSeconds(4f);
        LeftClickPressed = false;
        LMB = false;
        LeftClickMarker.SetActive(false);
    }
    IEnumerator FireTornadoAttack()
    {
        orgMegumin.SetActive(false);
        Megumin3.SetActive(true);
        spellCasting = true;
        yield return new WaitForSeconds(3.07f);
        Megumin3.SetActive(false);
        orgMegumin.SetActive(true);
        spellCasting = false;
    }
    IEnumerator SpellAttack(int attackID)
    {
        if (attackID == 1 && spell1FillAmt>=1.0f)
        {
            orgMegumin.SetActive(false);
            Megumin5.SetActive(false);
            Megumin4.SetActive(false);
            Megumin3.SetActive(true);
            spellCasting = true;
            spell1Flag = true;
            yield return new WaitForSeconds(3.07f);
            Megumin3.SetActive(false);
            orgMegumin.SetActive(true);
            spellCasting = false;
            spell1FillAmt = 0.0f;
        }
        else if (attackID == 2 && spell2FillAmt>=1.0f)
        {
            orgMegumin.SetActive(false);
            Megumin3.SetActive(false);
            Megumin4.SetActive(true);
            Megumin5.SetActive(false);
            spellCasting = true;
            spell2Flag = true;
            attack1UI.SetActive(true);
            attack2UI.SetActive(false);
            MagicCircle1.SetActive(true);
            lockCursor = true;
            yield return new WaitForSeconds(3.15f);
            Megumin4.SetActive(false);
            attack1UI.SetActive(false);
            

            orgMegumin.SetActive(true);
            spellCasting = false;
            spell2FillAmt = 0.0f;
            MagicCircle1.SetActive(false);
        }
        else if (attackID == 3 && spell3FillAmt >= 1.0f)
        {
            orgMegumin.SetActive(false);
            Megumin3.SetActive(false);
            Megumin4.SetActive(false);
            Megumin5.SetActive(true);
            spellCasting = true;
            spell3Flag = true;
            attack1UI.SetActive(false);
            attack2UI.SetActive(true);
           
            //lockCursor = true;
            yield return new WaitForSeconds(4.8f);
            Megumin5.SetActive(false);
            attack2UI.SetActive(false);


            orgMegumin.SetActive(true);
            spellCasting = false;
            spell3FillAmt = 0.0f;
            //MagicCircle1.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(10.0f);
        }
        
    }
}
