using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    [SerializeField] private PHUD hud;

    public GameObject handheldGun; // Reference to the handheld gun object

    // New properties
    private Vector3 lastCheckpoint;

    public Vector3 LastCheckpoint
    {
        get { return lastCheckpoint; }
        set { lastCheckpoint = value; }
    }

    private bool hasGun;

    public bool HasGun
    {
        get { return hasGun; }
        set
        {
            hasGun = value;
            UpdateGunStatus();
        }
    }

    private bool hasDimensionSwitch;

    public bool HasDimensionSwitch
    {
        get { return hasDimensionSwitch; }
        set { hasDimensionSwitch = value; }
    }

    private bool hasReversePower;

    public bool HasReversePower
    {
        get { return hasReversePower; }
        set { hasReversePower = value; }
    }

    private bool hasFreezePower;

    public bool HasFreezePower
    {
        get { return hasFreezePower; }
        set { hasFreezePower = value; }
    }

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

        if (handheldGun == null)
        {
            Debug.LogError("Handheld gun object not found in the player hierarchy.");
        }
    }

    private void UpdateGunStatus()
    {
        if (handheldGun != null)
        {
            handheldGun.SetActive(hasGun);
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