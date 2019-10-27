using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float Health=1;
    public Image HealthBar;
    public Image HealthBarBorder;
    public Image BloodOnScreen;
    Color BloodOnScreenColor;
    void Start()
    {
        BloodOnScreen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 10f, 0));
        HealthBarBorder.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 10f, 0));
        if (Health < 1)
            Health += Time.deltaTime / 20f;

        HealthBar.fillAmount = Health;
        BloodOnScreenColor.a = Mathf.Abs((1 - Health)-0.8f);
        BloodOnScreen.color = new Color(BloodOnScreen.color.r, BloodOnScreen.color.g, BloodOnScreen.color.b, BloodOnScreenColor.a);
        if (Health < 0.4f)
        {
            BloodOnScreen.enabled = true;
        }
        else
            BloodOnScreen.enabled = false;
    }
}
