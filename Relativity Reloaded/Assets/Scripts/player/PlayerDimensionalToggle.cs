using System.Collections;
using dimensions;

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
        private DimensionZone dimensionZone; // Reference to the RestrictedZone component
        public ParticleSystem particleSystem; // Reference to the Particle System

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

            // Find the RestrictedZone component
            dimensionZone = FindObjectOfType<DimensionZone>();
            if (dimensionZone == null)
            {
                Debug.LogError("RestrictedZone component not found in the scene.");
            }
        }

        void Update()
        {
            HandleDimensionalToggle();
        }

        private IEnumerator PlayParticleSystemForDuration(float duration)
        {
            particleSystem.Play();
            yield return new WaitForSeconds(duration);
            particleSystem.Stop();
        }
        private void HandleDimensionalToggle()
        {
            if (Input.GetKeyDown(toggleKey) && playerStats != null && playerStats.HasDimensionSwitch && (dimensionZone == null || !dimensionZone.IsPlayerInside(transform.position)))
            {
                DimensionalObjectManager.Instance.ToggleDimensionalObjects();
                ToggleSkybox();
                StartCoroutine(PlayParticleSystemForDuration(1.0f)); // Play for 1 second
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
