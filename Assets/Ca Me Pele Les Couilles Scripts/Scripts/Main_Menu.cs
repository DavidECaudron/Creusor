using UnityEngine;
using UnityEngine.SceneManagement;

namespace caca
{
    public class Main_Menu : MonoBehaviour
    {
        public void StartButton()
        {
            SceneManager.LoadScene("LevelDesign", LoadSceneMode.Single);
            //SceneManager.LoadScene("CommonScene", LoadSceneMode.Additive);
            SceneManager.LoadScene("Developpeur", LoadSceneMode.Additive);
        }
    }
}
