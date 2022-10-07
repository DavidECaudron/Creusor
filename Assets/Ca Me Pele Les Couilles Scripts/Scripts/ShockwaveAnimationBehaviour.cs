using UnityEngine;
using proto;

namespace caca
{
    public class ShockwaveAnimationBehaviour : StateMachineBehaviour
    {
        public AnimationScriptable _animationScriptable;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameObject clone = Instantiate(_animationScriptable._player._shockwavePrefab, _animationScriptable._player._shockwaveSpawn.position, _animationScriptable._player._shockwaveSpawn.rotation, _animationScriptable._player._abilitiesClone);

            Collider[] hitColliders = Physics.OverlapSphere(_animationScriptable._player._shockwaveSpawn.position, 4.0f);

            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("chest"))
                {
                    Chest chest = hit.transform.GetComponent<Chest>();

                    if (chest._isTaken == false)
                    {
                        _animationScriptable._player._gameManager.AddGold(chest._gold);
                        _animationScriptable._player._gameManager.AddChest(chest._nbChest);

                        if (chest._isTrapped == true)
                        {
                            _animationScriptable._player._gameManager._chestPackTable[chest._indexChestPack].UnTrapChest();
                            _animationScriptable._player._gameManager._enemyPackTable[chest._indexEnemyPack].ShowEnemy();
                        }

                        if (chest._mask != null)
                        {
                            chest._mask.SetActive(true);
                        }

                        if (chest._areaMask != null)
                        {
                            chest._areaMask.SetActive(false);
                        }

                        chest._isTaken = true;

                        if (chest._animator != null)
                        {
                            chest._animator.SetBool("RevealChest", true);
                        }
                    }
                }

                if (hit.CompareTag("retrievable"))
                {
                    Chest chest = hit.transform.GetComponent<Chest>();

                    if (chest._isTaken == false)
                    {
                        _animationScriptable._player._gameManager.AddGold(chest._gold);
                        _animationScriptable._player._gameManager.AddChest(chest._nbChest);

                        if (chest._isTrapped == true)
                        {
                            _animationScriptable._player._gameManager._chestPackTable[chest._indexChestPack].UnTrapChest();
                            _animationScriptable._player._gameManager._enemyPackTable[chest._indexEnemyPack].ShowEnemy();
                        }

                        if (chest._mask != null)
                        {
                            chest._mask.SetActive(true);
                        }

                        if (chest._areaMask != null)
                        {
                            chest._areaMask.SetActive(false);
                        }

                        chest._isTaken = true;

                        if (chest._animator != null)
                        {
                            chest._animator.SetBool("RevealChest", true);
                        }

                        Destroy(hit.gameObject, 2.0f);
                    }
                }

                if (hit.CompareTag("enemy"))
                {
                    hit.transform.parent.parent.GetComponent<Enemy>().TakeDamage(_animationScriptable._player._shockwaveDamage);
                }

                if (hit.CompareTag("destructible"))
                {
                    //hit.gameObject.SetActive(false);

                    TreeBehavior treeBehavior = hit.GetComponent<TreeBehavior>();

                    if (treeBehavior != null)
                    {
                        treeBehavior.DestroyTree(clone.transform.position);
                    }
                }

                //if (hit.CompareTag("ground"))
                //{
                //    Debug.Log("ground");
                //}
            }

            _animationScriptable._player._animator.SetBool("_isShockwaving", false);
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
