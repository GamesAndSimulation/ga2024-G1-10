using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Internal.Scripts.Enemies.platform_mechanics
{
    public class PlatformAttach : MonoBehaviour
    {
        [FormerlySerializedAs("Player")] public GameObject player;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                player.transform.parent = transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                player.transform.parent = null;
            }
        }
    }

}