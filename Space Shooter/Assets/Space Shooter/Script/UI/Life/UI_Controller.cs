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

    public Color[] fillColour = new Color[3];

    private void Start()
    {
        shipController = GameObject.FindObjectOfType<Ship_Controller>();
        healthBar.wholeNumbers = true;
        healthBar.minValue = 0;
        PowerBar.minValue = 0;

        fillColour[0] = new Color(0, 255, 0);
        fillColour[1] = new Color(255, 194, 0);
        fillColour[2] = new Color(255, 0, 0);

    }

    private void Update()
    {
        healthBar.maxValue = shipController.shipStats.maxHealth;
        healthBar.value = shipController.shipStats.CurrentHealth;
        PowerBar.value = shipController.shipStats.CurrentPower;


        healthBarFill.color = fillColour[0];
        if (healthBar.value <= shipController.shipStats.maxHealth / 2) healthBarFill.color = fillColour[1];
        if (healthBar.value <= shipController.shipStats.maxHealth / 10) healthBarFill.color = fillColour[2];
    }
}