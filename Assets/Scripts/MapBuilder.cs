using PushingBoxStudios.Pathfinding;
using PushingBoxStudios.SteampunkTd.Cameras;
using PushingBoxStudios.SteampunkTd.Maps;
using UnityEngine;

namespace Assets.Scripts
{
    public class MapBuilder : MonoBehaviour
    {
        [SerializeField]
        private Transform _ground;

        [SerializeField]
        private GameObject _obstaclePrefab;

        [SerializeField]
        private string _mapImageFile;

        public Grid Grid { get; private set; }

        private void Awake()
        {
            var tex = Resources.Load<Texture2D>("Textures/Maps/" + _mapImageFile);
            var nodes = MapReader.Read(tex);
            Grid = new Grid(nodes);
        }

        private void Start()
        {
            var rtsCam = GameObject.Find("RtsCamera").GetComponent<RtsCameraHandler>();
            rtsCam.MinBounds = new Vector3(-Grid.Width / 2, 0, -Grid.Width / 2);
            rtsCam.MaxBounds = new Vector3(Grid.Width / 2, 0, Grid.Width / 2);

            SetupGround();
            CreateObstacles();
        }

        private void SetupGround()
        {
            var scale = new Vector3
            {
                x = (float)Grid.Width / 10,
                y = 1,
                z = (float)Grid.Height / 10
            };

            _ground.localScale = scale;
            var renderer = _ground.GetComponent<Renderer>();
            renderer.material.mainTextureScale = new Vector2(Grid.Width, Grid.Height);
        }

        private void CreateObstacles()
        {
            var location = new Location();

            for (location.Y = 0; location.Y < Grid.Height; location.Y++)
            {
                for (location.X = 0; location.X < Grid.Width; location.X++)
                {
                    if (Grid[location])
                    {
                        continue;
                    }

                    var pos = GridToSpace(location);

                    Instantiate(_obstaclePrefab, pos, Quaternion.identity);
                }
            }
        }

        public Vector3 GridToSpace(Location location)
        {
            return new Vector3
            {
                x = location.X - (Grid.Width / 2),
                y = 0,
                z = -location.Y + (Grid.Height / 2)
            };
        }

        public Location SpaceToGrid(Vector3 pos)
        {
            return new Location
            {
                X = Mathf.RoundToInt(pos.x) + (int)Grid.Width / 2,
                Y = Mathf.RoundToInt(-pos.z) + (int)Grid.Height / 2
            };
        }
    }
}
