namespace Project.Internal.Scripts.Enemies.player
{
    using UnityEngine;

    public class PlayerDimensionalToggle : MonoBehaviour
    {
        public KeyCode toggleKey = KeyCode.T;
        public Material originalSkybox; // Reference to the original skybox
        public Material toggledSkybox; // Reference to the toggled skybox

        private bool _isSkyboxToggled = false; // Track whether the skybox is toggled or not
        private PlayerStats playerStats; // Reference to the PlayerStats component

        void Start()
        {
            // Set the initial skybox
            if (originalSkybox != null)
            {
                RenderSettings.skybox = originalSkybox;
            }

            // Find the PlayerStats component
            playerStats = GetComponent<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats component not found on the player.");
            }
        }

        void Update()
        {
            HandleDimensionalToggle();
        }

        private void HandleDimensionalToggle()
        {
            if (Input.GetKeyDown(toggleKey) && playerStats != null && playerStats.HasDimensionSwitch)
            {
                DimensionalObjectManager.Instance.ToggleDimensionalObjects();
                ToggleSkybox();
            }
        }

        private void ToggleSkybox()
        {
            if (_isSkyboxToggled)
            {
                RenderSettings.skybox = originalSkybox;
            }
            else
            {
                RenderSettings.skybox = toggledSkybox;
            }

            _isSkyboxToggled = !_isSkyboxToggled;
            DynamicGI.UpdateEnvironment(); // Update the environment lighting
        }
    }
}