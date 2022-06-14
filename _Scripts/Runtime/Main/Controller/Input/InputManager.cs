
using System;
using Udar.DesignPatterns.Singleton;
using Udarverse.Input;
using UnityEngine;

namespace Udarverse
{
    public class InputManager : Singleton<InputManager>
    {
        private enum PlatformInput
        {
            Automatically,
            PC,
            Mobile
        }
        [SerializeField] private PlatformInput _platformInput;

        private IUserInput _userInput;

        public static IUserInput Ctx => Instance._userInput;
  

        protected override void Awake()
        {
            base.Awake();
            Setup();
        }

      
        public void Setup()
        {
            if ((IsPCPlatform() && _platformInput == PlatformInput.Automatically) || _platformInput == PlatformInput.PC)
            {
                var input = transform.GetComponent<PCInput>();
                input.enabled = true;
                _userInput = input;
                _platformInput = PlatformInput.PC;
            }
            if ((IsMobile() && _platformInput == PlatformInput.Automatically) || _platformInput == PlatformInput.Mobile)
            {
                var input = transform.GetComponent<MobileInput>();
                input.enabled = true;
                _platformInput = PlatformInput.Mobile;
                _userInput = input;
            }
            _userInput.Setup();
        }

        private bool IsPCPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                    {
                        return true;
                    }
                case RuntimePlatform.WindowsEditor:
                    {
                        return true;
                    }
                case RuntimePlatform.WSAPlayerX64:
                    {
                        return true;
                    }
                case RuntimePlatform.WSAPlayerX86:
                    {
                        return true;
                    }
                case RuntimePlatform.WSAPlayerARM:
                    {
                        return true;
                    }
                case RuntimePlatform.OSXEditor:
                    {
                        return true;
                    }
                case RuntimePlatform.OSXPlayer:
                    {
                        return true;
                    }
            }
            return _platformInput == PlatformInput.PC;
        }

        private bool IsMobile()
        {
            return Application.isMobilePlatform || _platformInput == PlatformInput.Mobile;
        }
    }
}

