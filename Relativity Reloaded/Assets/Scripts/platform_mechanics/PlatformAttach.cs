using UnityEngine;

namespace Project.Internal.Scripts.Enemies.platform_mechanics
{
    public class PlatformAttach : MonoBehaviour
    {
        public GameObject Player;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Player)
            {
                Player.transform.parent = transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == Player)
            {
                Player.transform.parent = null;
            }
        }
    }

}