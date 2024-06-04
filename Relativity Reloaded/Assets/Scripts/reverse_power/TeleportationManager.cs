namespace Project.Internal.Scripts.Enemies.reverse_power
{
    using UnityEngine;

    public class TeleportationManager : MonoBehaviour
    {
        void Update()
        {
            // Check for key presses and teleport accordingly
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TeleportTo("BeginningCheckpoint");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TeleportTo("GunCheckpoint");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                TeleportTo("LakeCheckpoint");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                TeleportTo("PlatformColumnCheckpoint");
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