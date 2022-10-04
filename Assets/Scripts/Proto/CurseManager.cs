using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    public class CurseManager : MonoBehaviour
    {
        Animator[] curseFXAnimators = new Animator[2];
        void Awake()
        {
            curseFXAnimators[0] = GameObject.Find("PostProcessing").GetComponent<Animator>();
            curseFXAnimators[1] = GameObject.Find("CurseTimer").GetComponent<Animator>();  
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

        }


        void Update()
        {
        
        }
    }
}
