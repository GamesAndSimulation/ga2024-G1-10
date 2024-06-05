namespace Project.Internal.Scripts.Enemies.reverse_power
{
    using UnityEngine;

    public class ShortcutManager : MonoBehaviour
    {
        private PlayerStats playerStats;

        void Start()
        {
            playerStats = GetComponent<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats component not found on the player.");
            }
        }

        void Update()
        {
            // Check for key presses and teleport accordingly
            if (Input.GetKey(KeyCode.Alpha1))
            {
                TeleportTo("BeginningCheckpoint");
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                TeleportTo("GunCheckpoint");
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                TeleportTo("Moving_plat_Checkpoint");
            }
            else if (Input.GetKey(KeyCode.Alpha4) && playerStats != null)
            {
                playerStats.SetCoinCount(5); // Set to the maximum number of coins
                Debug.Log("All coins collected!");
            }
            else if (Input.GetKey(KeyCode.Alpha5) && playerStats != null)
            {
                playerStats.SetFreezePower(true); // Grant freeze power
                Debug.Log("Freeze power obtained!");
            }
            else if (Input.GetKey(KeyCode.Alpha6) && playerStats != null)
            {
                playerStats.HasDimensionSwitch = true; // Grant dimensional power
                Debug.Log("Dimensional power obtained!");
            }
            else if (Input.GetKey(KeyCode.Alpha7) && playerStats != null)
            {
                playerStats.HasGun = true; // Grant gun
                playerStats.UpdateGunStatus();
                Debug.Log("Gun obtained!");
            }
        }

        void TeleportTo(string checkpointName)
        {
            GameObject checkpoint = GameObject.Find(checkpointName);
            if (checkpoint != null)
            {
                transform.position = checkpoint.transform.position;
                Debug.Log($"Teleported to {checkpointName}");
            }
            else
            {
                Debug.LogWarning($"Checkpoint {checkpointName} not found");
            }
        }
    }
}
