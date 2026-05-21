using UnityEngine;

namespace PortfolioFilling.Enemy
{
    public sealed class ZombieAI : MonoBehaviour
    {
        private Transform _playerTarget;
        private Transform _objectiveTarget;
        private float _moveSpeed;

        public Transform CurrentTarget { get; private set; }

        public void Initialize(Transform playerTarget, Transform objectiveTarget, float moveSpeed)
        {
            _playerTarget = playerTarget;
            _objectiveTarget = objectiveTarget;
            _moveSpeed = moveSpeed;
        }

        private void Update()
        {
            ChooseTarget();
            if (CurrentTarget == null)
            {
                return;
            }

            var direction = CurrentTarget.position - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.01f)
            {
                return;
            }

            transform.position += direction.normalized * (_moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), 10f * Time.deltaTime);
        }

        private void ChooseTarget()
        {
            if (_playerTarget == null)
            {
                CurrentTarget = _objectiveTarget;
                return;
            }

            var playerDistance = Vector3.Distance(transform.position, _playerTarget.position);
            var objectiveDistance = _objectiveTarget == null ? float.MaxValue : Vector3.Distance(transform.position, _objectiveTarget.position);
            CurrentTarget = playerDistance <= objectiveDistance + 2f ? _playerTarget : _objectiveTarget;
        }
    }
}
