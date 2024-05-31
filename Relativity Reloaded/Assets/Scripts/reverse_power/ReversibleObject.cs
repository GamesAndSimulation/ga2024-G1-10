namespace Project.Internal.Scripts.Enemies
{
    using UnityEngine;
    using System.Collections.Generic;

    public class ReversibleObject : MonoBehaviour
    {
        public float recordInterval = 0.1f;
        private List<ObjectState> states = new List<ObjectState>();
        private bool isReversing = false;
        private float recordTimer = 0f;

        void Update()
        {
            if (isReversing)
            {
                ReverseTime();
            }
            else
            {
                RecordState();
            }
        }

        private void RecordState()
        {
            recordTimer += Time.deltaTime;
            if (recordTimer >= recordInterval)
            {
                states.Insert(0, new ObjectState(transform.position, transform.rotation));
                recordTimer = 0f;
            }
        }

        private void ReverseTime()
        {
            if (states.Count > 0)
            {
                ObjectState state = states[0];
                transform.position = state.position;
                transform.rotation = state.rotation;
                states.RemoveAt(0);
            }
            else
            {
                isReversing = false;
            }
        }

        public void StartReversing()
        {
            isReversing = true;
        }

        public void StopReversing()
        {
            isReversing = false;
        }
    }

}