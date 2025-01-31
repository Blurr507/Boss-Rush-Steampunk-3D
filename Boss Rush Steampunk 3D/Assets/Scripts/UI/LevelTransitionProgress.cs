using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionProgress : MonoBehaviour
{
    public AudioSource bellSound;

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayBell()
    {
        bellSound.Play();
    }
}
