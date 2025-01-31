using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionProgress : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
