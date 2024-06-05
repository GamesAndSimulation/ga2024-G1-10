using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadAfterIntro : MonoBehaviour
{
    [SerializeField] private float delayBeforeLoading = 90f;
    private float timeElapsed;
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > delayBeforeLoading) {
            SceneManager.LoadScene(4);
        }
    }
}
