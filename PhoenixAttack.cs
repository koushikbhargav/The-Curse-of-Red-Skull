using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoenixAttack : MonoBehaviour
{
    RaycastHit hit;
    public List<GameObject> PhoenixObjects = new List<GameObject>(2);
   
    public Vector3 target;
    public int i = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (PhoenixObjects[0])
        {
            PhoenixObjects[0].GetComponent<EagleRevolve>().isLaunch = true;

            if (PhoenixObjects[0].GetComponent<EagleRevolve>().isFly == true)
                PhoenixObjects[1].GetComponent<EagleRevolve>().isLaunch = true;
        }

        if (PhoenixObjects[1] == null)
            Destroy(this.gameObject);





    }
}
