using UnityEngine;

namespace Refacto_Trois
{
    public class PlayerCamera : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Player_Data _playerData;
        [SerializeField] private Camera _camera;

        #endregion


        #region Get Set

        public Camera Camera { get => _camera; private set => _camera = value; }

        #endregion
    }
}
