namespace Project.Internal.Scripts.Enemies.player
{
    using UnityEngine;

    public class PlayerInput
    {
        public event System.Action OnToggleFreeCamera;
        public event System.Action OnTogglePauseGame;

        public void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                OnToggleFreeCamera?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                OnTogglePauseGame?.Invoke();
            }
        }
    }

}