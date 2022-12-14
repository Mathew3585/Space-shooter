using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    [HideInInspector]
    public Ship_Controller shipController;

    public Slider healthBar;
    public Image healthBarFill;
    public Slider PowerBar;
    public Image PowerBarFill;

    public Sprite[] fillColour = new Sprite[3];

    private void Start()
    {
        shipController = GameObject.FindObjectOfType<Ship_Controller>();
        healthBar.wholeNumbers = true;
        healthBar.minValue = 0;
        PowerBar.minValue = 0;

    }

    private void Update()
    {
        healthBar.maxValue = shipController.shipStats.maxHealth;
        healthBar.value = shipController.shipStats.CurrentHealth;
        PowerBar.value = shipController.shipStats.CurrentPower;


        healthBarFill.sprite = fillColour[0];
        if (healthBar.value <= shipController.shipStats.maxHealth / 2) healthBarFill.sprite = fillColour[1];
        if (healthBar.value <= shipController.shipStats.maxHealth / 10) healthBarFill.sprite = fillColour[2];
    }
}