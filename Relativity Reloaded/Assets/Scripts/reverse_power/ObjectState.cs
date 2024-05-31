namespace Project.Internal.Scripts.Enemies
{
    using UnityEngine;

    [System.Serializable]
    public class ObjectState
    {
        public Vector3 position;
        public Quaternion rotation;

        public ObjectState(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

}