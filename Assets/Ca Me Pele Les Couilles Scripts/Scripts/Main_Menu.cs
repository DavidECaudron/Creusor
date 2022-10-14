using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace caca
{
    public class Main_Menu : MonoBehaviour
    {
        public Animator _creditAnimator;
        void Start()
        {
            Invoke("DisplayScene", 2f);
        }
        public Animator _sceneTransitionAnimator;
        public void StartButton()
        {
            _sceneTransitionAnimator.SetBool("HideScreen", true);
            Invoke("LoadScene", 2f);
        }  

        public void CreditButton()
        {
            _sceneTransitionAnimator.SetBool("HideScreen", true);
            Invoke("DisplayCredits",0f);
            Invoke("DisplayScene", 42f);
        } 

        public void DisplayCredits()
        {
            _creditAnimator.SetTrigger("DisplayCredits");
        }

        public void DisplayScene()
        {
            _sceneTransitionAnimator.SetBool("HideScreen", false);            
        } 
        public void LoadScene()
        {
            SceneManager.LoadScene("LevelDesign", LoadSceneMode.Single);
            SceneManager.LoadScene("CommonScene", LoadSceneMode.Additive);
        }   
    }
}
