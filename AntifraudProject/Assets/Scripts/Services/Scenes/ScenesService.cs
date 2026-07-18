using UnityEngine.SceneManagement;

namespace Services.Scenes
{
    public class ScenesService
    {
        public void LaunchGameplay()
        {
            SceneManager.LoadScene("Gameplay");
        }

        public void LaunchMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}