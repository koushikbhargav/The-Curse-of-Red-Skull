using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamRelease : MonoBehaviour
{
    public GameObject FullBeamObject;
    public GameObject MainBeam;
    public GameObject Sphere;
   // public Animator SphereAnim;
    //public Animator SwordAnim;
   // public GameObject FireSword;
    //public AudioSource cero;
    //public AudioSource kamehameha;
    public static bool attackMode;
    public GameObject beamHead;
   
    

    [SerializeField]
    string s;
    void Start()
    {   
        MainBeam.SetActive(false);
        Sphere.SetActive(false);
        
        beamHead.SetActive(false);
      //  FullBeamObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (BeamAttack.performBeamAttack==true)
        {

            BeamAttack.performBeamAttack = false;
            Sphere.SetActive(true);
            
           
            StartCoroutine(Release());
            
        }
        
    }

    IEnumerator ReleaseBeam()
    {
        yield return new WaitForSeconds(2f);
        //kamehameha.Stop();
        //cero.Play();
        StartCoroutine(Release());
        
    }
    IEnumerator Release()
    {
       
        MainBeam.SetActive(true);
        attackMode = true;
        beamHead.SetActive(true);
        // FullBeamObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        
        beamHead.transform.localPosition = new Vector3(0, 0, 4.66f);
        
        FullBeamObject.SetActive(false);
       
       
    }
}
