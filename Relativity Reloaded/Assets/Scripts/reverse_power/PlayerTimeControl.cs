namespace Project.Internal.Scripts.Enemies
{
    using UnityEngine;
    using System.Collections.Generic;

    public class PlayerTimeControl : MonoBehaviour
    {
        public List<ReversibleObject> reversibleObjects = new List<ReversibleObject>();
        public KeyCode reverseTimeKey = KeyCode.R;

        void Update()
        {
            if (Input.GetKeyDown(reverseTimeKey))
            {
                StartReversingTime();
            }

            if (Input.GetKeyUp(reverseTimeKey))
            {
                StopReversingTime();
            }
        }

        private void StartReversingTime()
        {
            foreach (ReversibleObject obj in reversibleObjects)
            {
                obj.StartReversing();
            }
        }

        private void StopReversingTime()
        {
            foreach (ReversibleObject obj in reversibleObjects)
            {
                obj.StopReversing();
            }
        }
    }
}