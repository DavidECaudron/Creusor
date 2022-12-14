using UnityEngine;

namespace caca
{
    public class SlashAnimationBehaviour : StateMachineBehaviour
    {
        public AnimationScriptable _animationScriptable;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //_initialRotation = _animationScriptable._player._shovelTransform.rotation;
            //_animationScriptable._player._shovelTransform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //_animationScriptable._player._shovelTransform.rotation = _initialRotation;
            _animationScriptable._player._animator.SetBool("_isSlashing", false);
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
