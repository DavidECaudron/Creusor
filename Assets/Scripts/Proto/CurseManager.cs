using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    public class CurseManager : MonoBehaviour
    {
        Animator[] curseFXAnimators = new Animator[3];
        AudioSource audioSourceCurse;
        AudioSource audioSourceIslandLoop;

        GameObject audioManager;

        void Awake()
        {
            curseFXAnimators[0] = GameObject.Find("PostProcessing").GetComponent<Animator>();
            curseFXAnimators[1] = GameObject.Find("CurseTimer").GetComponent<Animator>(); 
            curseFXAnimators[2] = GameObject.Find("HP").GetComponent<Animator>();  
            audioManager = GameObject.Find("AudioManager");
            audioSourceIslandLoop = audioManager.transform.GetChild(0).GetComponent<AudioSource>();
            audioSourceCurse = audioManager.transform.GetChild(2).GetComponent<AudioSource>();
        }

        // Update is called once per frame

        public void TimerAlmostOverFX()
        {
            curseFXAnimators[1].SetTrigger("TimerAlmostOver");
        } 

        public void DisplayCurseFX()
        {
            curseFXAnimators[0].SetTrigger("DisplayCurse");
            curseFXAnimators[1].SetTrigger("EndOfTimer");
            curseFXAnimators[2].SetTrigger("CurseHP");            
            StartCoroutine(SetCurseMusic());


        }

        IEnumerator SetCurseMusic()
        {
            float elapsedTime = 0;
            float duration = 0.5f;

            while(elapsedTime < duration)
            {
                float lerp = Mathf.Lerp(1,0, elapsedTime / duration);
                audioSourceIslandLoop.volume = lerp;      
                elapsedTime += Time.deltaTime;
                yield return null;
            } 
            audioSourceIslandLoop.volume = 0;
            audioSourceIslandLoop.Stop();
            audioSourceCurse.Play();

            yield return null;
        }
    }
}
