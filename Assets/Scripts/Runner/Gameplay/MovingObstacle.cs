using Runner.Core;
using UnityEngine;
using VContainer;

namespace Runner.Gameplay
{
    public sealed class MovingObstacle : MonoBehaviour
    {
        public enum MovementMode
        {
            TowardPlayer = 0,
            PerpendicularOscillation = 1
        }

        [SerializeField] private MovementMode mode = MovementMode.TowardPlayer;
        [SerializeField] private float speed = 2f;
        [SerializeField] private float amplitude = 0.75f;
        [SerializeField] private Vector3 axis = Vector3.forward;

        private IGameStateService _gameStateService;
        private Vector3 _startPosition;

        [Inject]
        public void Construct(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        private void Awake()
        {
            _startPosition = transform.localPosition;
        }

        private void OnEnable()
        {
            _startPosition = transform.localPosition;
        }

        private void Update()
        {
            var stateService = _gameStateService ?? RunnerRuntimeContext.GameStateService;
            if (stateService != null && stateService.State != GameState.Playing)
                return;

            if (mode == MovementMode.TowardPlayer)
            {
                transform.position += Vector3.left * (speed * Time.deltaTime);
                return;
            }

            float value = Mathf.Sin(Time.time * speed) * amplitude;
            transform.localPosition = new Vector3(transform.position.x,
                _startPosition.y + axis.normalized.y * value, _startPosition.z);
        }
    }
}
