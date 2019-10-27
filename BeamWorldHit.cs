using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWorldHit : MonoBehaviour
{
    public GameObject ExplosionObject;
    GameObject go;
    public GameObject FireBeam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WorldObject"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {

            }


            

            go = Instantiate(ExplosionObject);
            //go.transform.position = other.ClosestPoint(this.transform.position);
            go.transform.position = hit.point;
            //Destroy(go,2f);
            //this.transform.localPosition = new Vector3(0, 0, 4.66f);
            FireBeam.SetActive(false);
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit Ground: ");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {

            }

            Debug.Log("Hit");
           

            go = Instantiate(ExplosionObject);
            //go.transform.position = other.ClosestPoint(this.transform.position);
            go.transform.position = hit.point;
            //Destroy(go,2f);
            //this.transform.localPosition = new Vector3(0, 0, 4.66f);
            FireBeam.SetActive(false);
        }
    }


}

