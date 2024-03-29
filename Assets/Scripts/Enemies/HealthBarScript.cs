using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Camera playerCamera;
    public Image hpBar;

    void Start()
    {
        slider = GetComponentInParent<Slider>();
    }
    public void UpdateHealthBarSlider(float curentValue, float maxValue)
    {
        slider.value = curentValue / maxValue;
    }

    public void UpdateHealthBarImage(float curentValue, float maxValue)
    {
        hpBar.fillAmount = curentValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera = Camera.main;
        transform.rotation = playerCamera.transform.rotation;
    }
}
