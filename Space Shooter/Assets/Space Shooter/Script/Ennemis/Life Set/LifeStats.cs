using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class LifeStats : MonoBehaviour
{
    public float MaxHealth;

    [HideInInspector]
    public float currentHealth;

    public float Damage;

    public int MoneyDrop;

    public bool Minautore;

    public bool Pégase;

    public bool Hydre;

}
