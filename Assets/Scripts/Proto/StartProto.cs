using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace proto
{
    public class StartProto : MonoBehaviour
    {
        public void StartProtoButton()
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            SceneManager.LoadScene("Liam", LoadSceneMode.Additive);
            //SceneManager.LoadScene("David", LoadSceneMode.Additive);
            SceneManager.LoadScene("Bryan", LoadSceneMode.Additive);
        }
    }
}
