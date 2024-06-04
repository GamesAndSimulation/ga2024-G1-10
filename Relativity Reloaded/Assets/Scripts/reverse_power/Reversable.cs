using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.Scripts.Enemies.reverse_power
{
    public class Reversable : MonoBehaviour
    {
        private List<Vector3> positions;
        private List<Quaternion> rotations;
        private bool isReversing = false;

        public int maxStoredPositions = 10000; // Maximum number of positions to store

        void Start()
        {
            positions = new List<Vector3>();
            rotations = new List<Quaternion>();
        }

        void Update()
        {
            if (!isReversing)
            {
                // Store the current position and rotation
                positions.Add(transform.position);
                rotations.Add(transform.rotation);

                // Remove oldest entry if we exceed the maximum capacity
                if (positions.Count > maxStoredPositions)
                {
                    positions.RemoveAt(0);
                    rotations.RemoveAt(0);
                }
            }
        }

        public void StartReversing()
        {
            isReversing = true;
            StartCoroutine(ReverseMotion());
        }

        public void StopReversing()
        {
            isReversing = false;
        }

        private IEnumerator ReverseMotion()
        {
            while (positions.Count > 0 && isReversing)
            {
                transform.position = positions[positions.Count - 1];
                transform.rotation = rotations[rotations.Count - 1];
                positions.RemoveAt(positions.Count - 1);
                rotations.RemoveAt(rotations.Count - 1);
                yield return null;
            }

            // Stop reversing once all positions are replayed
            isReversing = false;
        }
    }
}