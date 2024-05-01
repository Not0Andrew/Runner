using UnityEngine;

namespace AdvancedInputSystem
{
    public class BaseInputSystem : MonoBehaviour
    {
        public static event OnMoveInput MoveInput;
        public delegate void OnMoveInput(Vector2Int direction);

        private Vector2 _tapPosition;
        private Vector2 _swipeDelta;

        private readonly float _deadZone = 80;

        private bool _isSwiping;

        private void Update()
       {
           if(MoveInput == null)
                return;
           
           if (Input.GetKeyDown(KeyCode.W))
           {
               MoveInput(new Vector2Int(0, 1));
           }
           if (Input.GetKeyDown(KeyCode.S))
           {
               MoveInput(new Vector2Int(0, -1));
           }
           if (Input.GetKeyDown(KeyCode.D))
           {
               MoveInput(new Vector2Int(1, 0));
           }
           if (Input.GetKeyDown(KeyCode.A))
           {
               MoveInput(new Vector2Int(-1, 0));
           }

           if (Input.GetMouseButtonDown(0))
           {
               _isSwiping = true;
               _tapPosition = Input.mousePosition;
           }
           else if(Input.GetMouseButtonUp(0))
           {
               ResetSwipe();
           }
           
           CheckSwipe();
       }

        private void CheckSwipe()
        {
            if(MoveInput == null)
                return;
            
            _swipeDelta = Vector2.zero;

            if (_isSwiping)
            {
                if (Input.GetMouseButton(0))
                {
                    _swipeDelta = (Vector2) Input.mousePosition - _tapPosition;
                }
            }

            if (_swipeDelta.magnitude > _deadZone)
            {
                if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
                {
                    MoveInput(_swipeDelta.x > 0 ? Vector2Int.right : Vector2Int.left);
                }
                else
                {
                    MoveInput(_swipeDelta.y > 0 ? Vector2Int.up : Vector2Int.down);
                }
                
                ResetSwipe();
            }
        }

        private void ResetSwipe()
        {
            _isSwiping = false;

            _tapPosition = Vector2.zero;
            _swipeDelta = Vector2.zero;
        }
    }
}
