using UnityEngine;

namespace caca
{
    [CreateAssetMenu(fileName = "AnimationScriptable", menuName = "ScriptableObjects/AnimationScriptable")]
    public class AnimationScriptable : ScriptableObject
    {
        public Player _player;
    }
}
