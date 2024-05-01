using System;
using UnityEngine;

namespace AdvancedInputSystem
{
    public class PlayerInputSystem : MonoBehaviour
    {
        public event Action OnJump;
        public event Action OnDown;
        
        private void Awake()
        {
            BaseInputSystem.MoveInput += OnMoveInput;
        }

        private void OnMoveInput(Vector2Int directionInput)
        {
            if (directionInput.y == 1)
            {
                OnJump?.Invoke();
            }
            else if (directionInput.y == -1)
            {
                OnDown?.Invoke();
            }
        }
    }
}
