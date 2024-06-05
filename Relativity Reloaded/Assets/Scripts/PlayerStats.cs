using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    [SerializeField] private PHUD hud;
    [SerializeField] GameObject enemy;
    public GameObject handheldGun; // Reference to the handheld gun object
    [SerializeField] private Material freezePowerMaterial; // Reference to the freeze power material

    // New properties
    private Vector3 _lastCheckpoint;
    private int _coinCount; // Variable to keep track of the number of coins
    private const int _totalCoins = 5; // Total number of coins

    private bool hasGun;
    public bool HasGun
    {
        get => hasGun;
        set
        {
            hasGun = value;
            UpdateGunStatus();
        }
    }

    public bool hasDimensionSwitch;
    public bool HasDimensionSwitch
    {
        get => hasDimensionSwitch;
        set => hasDimensionSwitch = value;
    }

    public bool hasReversePower;
    public bool hasFreezePower;

    private void Start()
    {
        InitVariables();
        GetReferences();
        Alive();
        UpdateHUD();
    }

    private void GetReferences()
    {
        if (hud == null)
        {
            if (!TryGetComponent(out hud))
            {
                Debug.LogError("PHUD component not found on the same GameObject.");
            }
        }

        if (handheldGun == null)
        {
            Debug.LogError("Handheld gun object not found in the player hierarchy.");
        }
    }

    public void UpdateGunStatus()
    {
        if (handheldGun != null)
        {
            handheldGun.SetActive(hasGun);
        }
    }

    private void UpdateFreezePowerStatus()
    {
        if (hasFreezePower && handheldGun != null)
        {
            // Assuming the gun pipe is a child of the handheld gun
            Renderer gunRenderer = handheldGun.GetComponentInChildren<Renderer>();
            if (gunRenderer != null)
            {
                gunRenderer.material = freezePowerMaterial;
            }
            else
            {
                Debug.LogError("Gun Renderer not found.");
            }
        }
    }

    public void CollectCoin()
    {
        _coinCount++;
        UpdateHUD();
    }

    public void SetCoinCount(int coinCount)
    {
        _coinCount = coinCount;
        UpdateHUD();
    }

    public void SetFreezePower(bool hasFreezePower)
    {
        this.hasFreezePower = hasFreezePower;
        UpdateFreezePowerStatus();
        UpdateHUD();
    }

    public bool HasAllCoins()
    {
        return _coinCount >= _totalCoins;
    }

    private void UpdateHUD()
    {
        if (hud != null)
        {
            hud.UpdateHealth(health, maxHealth);
            hud.UpdateCoins(_coinCount, _totalCoins); // Update the coin count
        }
        else
        {
            Debug.LogError("HUD reference is null.");
        }
    }

    public override void Alive()
    {
        base.Alive();
        UpdateHUD();
    }
}
