using PushingBoxStudios.Pathfinding;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PathfinderRobot : MonoBehaviour
    {
        private AbstractPathfinder _pathfinder;
        private IPath _currentPath;
        private Animator _animator;
        private bool _invoked;
        private Grid _grid;

        public event EventHandler PathFound;

        [SerializeField]
        private MapBuilder _mapBuilder;

        [SerializeField]
        private float _speed = 1.5f;

        [SerializeField]
        private int _rotationSpeed = 360;

        [SerializeField]
        private bool _isWandering = false;

        public IPath CurrentPath { get { return _currentPath; } }

        public void Start()
        {
            _animator = GetComponent<Animator>();
            _grid = _mapBuilder.Grid;

            _pathfinder = new AStar();
        }

        public void Update()
        {
            if (_currentPath == null || _currentPath.Size == 0)
            {
                if (_isWandering && !_invoked)
                {
                    _invoked = true;
                    Invoke("DoWander", 1);
                }

                _animator.SetBool("IsWalking", false);
                return;
            }

            _animator.SetBool("IsWalking", true);

            var pos = transform.position;
            var targetPos = _mapBuilder.GridToSpace(_currentPath.Front);

            if (pos == targetPos || Vector3.Distance(pos, targetPos) <= _speed * Time.deltaTime)
            {
                _currentPath.PopFront();

                if (_currentPath.Size > 0)
                {
                    targetPos = _mapBuilder.GridToSpace(_currentPath.Front);
                }
                else
                {
                    pos = targetPos;
                }
            }

            pos = Vector3.MoveTowards(pos, targetPos, _speed * Time.deltaTime);
            transform.position = pos;

            UpdateRotation(targetPos);
        }

        public void FindPath(Vector3 pos)
        {
            var start = _mapBuilder.SpaceToGrid(transform.position);
            var goal = _mapBuilder.SpaceToGrid(pos);

            if (start.Equals(goal))
            {
                return;
            }

            _currentPath = _pathfinder.FindPath(_grid, start, goal);

            if (_currentPath == null)
            {
                return;
            }

            OnPathFound();
            _currentPath.PopFront();
        }

        private void UpdateRotation(Vector3 targetPos)
        {
            var dir = targetPos - transform.position;
            var toRot = Quaternion.LookRotation(dir, Vector3.up);
            var fromRot = transform.rotation;
            var rot = Quaternion.RotateTowards(fromRot, toRot, _rotationSpeed * Time.deltaTime);

            transform.rotation = rot;
        }

        private void DoWander()
        {
            _invoked = false;
            var randLocation = new Location();

            do
            {
                randLocation = new Location
                {
                    X = UnityEngine.Random.Range(0, (int)_grid.Width),
                    Y = UnityEngine.Random.Range(0, (int)_grid.Width)
                };
            }
            while (!_grid[randLocation]);

            var pos = _mapBuilder.GridToSpace(randLocation);
            FindPath(pos);
        }

        protected virtual void OnPathFound()
        {
            if (PathFound != null)
            {
                PathFound(this, EventArgs.Empty);
            }
        }
    }
}
