namespace Project.Internal.Scripts.Enemies.player
{
    using UnityEngine;
    using Project.Internal.Scripts.Enemies.reverse_power;

    public class PlayerReversal : MonoBehaviour
    {
        public KeyCode reversalKey = KeyCode.Z; // Key to activate reversal power

        void Update()
        {
            HandleReversal();
        }

        private void HandleReversal()
        {
            if (Input.GetKeyDown(reversalKey))
            {
                Reversable[] reversableObjects = FindObjectsOfType<Reversable>();
                foreach (Reversable reversable in reversableObjects)
                {
                    reversable.StartReversing();
                }
            }
            else if (Input.GetKeyUp(reversalKey))
            {
                Reversable[] reversableObjects = FindObjectsOfType<Reversable>();
                foreach (Reversable reversable in reversableObjects)
                {
                    reversable.StopReversing();
                }
            }
        }
    }

}