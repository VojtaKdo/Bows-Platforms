using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashChargeBarScript : MonoBehaviour
{
    public Slider slider;
    public Camera playerCamera;
    public Image dashChargeBar;
    // Start is called before the first frame update
    void Start()
    {
        UpdateChargeBarImage(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateChargeBarSlider(double curentValue, double maxValue)
    {
        slider.value = (float)(curentValue / maxValue);
    }

    public void UpdateChargeBarImage(double curentValue, double maxValue)
    {
        dashChargeBar.fillAmount = (float)(curentValue / maxValue);
    }
}
