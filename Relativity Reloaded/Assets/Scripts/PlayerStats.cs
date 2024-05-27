using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    [SerializeField] private PHUD hud;

    private void Start()
    {
        InitVariables();
        GetReferences();
        Alive();
    }

    private void GetReferences()
    {
        if (hud == null)
        {
            if (!TryGetComponent<PHUD>(out hud))
            {
                Debug.LogError("PHUD component not found on the same GameObject.");
            }
        }
    }

    public override void Alive()
    {
        base.Alive();
        if (hud != null)
        {
            hud.UpdateHealth(health, maxHealth);
        }
        else
        {
            Debug.LogError("HUD reference is null.");
        }
    }
}
