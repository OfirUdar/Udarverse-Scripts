
using Udar.DesignPatterns.Singleton;
using Udar.DesignPatterns.UdarPool;
using UnityEngine;

namespace Udarverse
{
    public class GameManager : Singleton<GameManager>
    {
        [field: SerializeField] public GameObject MainPlayer { get; private set; }

        private CharacterController _characterController;
        public CharacterController CharacterController
        {
            get
            {
                if (_characterController == null)
                    _characterController = MainPlayer.GetComponent<CharacterController>();
                return _characterController;
            }
        }

        public void SetPlayerPosition(Vector3 position)
        {
            CharacterController.enabled = false;
            MainPlayer.transform.position = position;
            CharacterController.enabled = true;
        }

      
    }

}
