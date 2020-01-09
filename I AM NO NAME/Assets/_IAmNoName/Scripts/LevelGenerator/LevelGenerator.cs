///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 09/12/2019 19:55
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.IAmNoName.LevelGenerator {
    public class LevelGenerator : MonoBehaviour {
        [SerializeField] private int seed = 0;
        [SerializeField] private bool randomSeed = false;
        [SerializeField] private Transform[] startingPositions;
        [SerializeField] private GameObject[] startingRooms;
        [Header("Rooms")]
        [SerializeField] public GameObject[] leftRooms;
        [SerializeField] public GameObject[] rightRooms;
        [SerializeField] public GameObject[] topRooms;
        [SerializeField] public GameObject[] bottomRooms;
        [SerializeField] public GameObject[] walls;
        [Space]
        [SerializeField] private LayerMask RoomMask;
        [SerializeField,Range(0.1f,1f)] public float timeBetweenSpawn = 0.25f;
        [Header("Level Size")]
        [SerializeField] private int maxRoomLegth;
        [SerializeField] private int maxRoom;
        [Header("Direction Percent")]
        [SerializeField] private float right;
        [SerializeField] private float left, up, down;

        
        [HideInInspector] public bool stopGeneration = false;

        private int _downCounter;
        private int _roomCount;
        private int _seed;
        private List<GameObject> rooms = new List<GameObject>();

        private static LevelGenerator _instance;
        public static LevelGenerator Instance { get { return _instance; } }

        private enum Direction {
            rigth,
            left,
            up,
            down
        }
        
        private void Awake() {
            if (_instance) {
                Destroy(gameObject);
                return;
            }
            _instance = this;


            _seed = randomSeed ?Random.Range(int.MinValue,int.MaxValue) : seed;
            Random.InitState(_seed);
            Debug.Log("<size=22>Seed : "+ "<color=green>" + _seed + "</color></size>");
        }

        private void Start() {
            CreateStartRoom();

            left += right;
            up += left;
            down += up;
        }

        public void OnNewRoomCreated(GameObject room) {
            _roomCount++;
            rooms.Add(room);
            if (_roomCount >= maxRoom && !stopGeneration) {
                stopGeneration = true;
                Debug.Log("<size=22><color=purple>Level Generated</color></size>");
            }
        }

        private void OnDestroy() {
            if (this == _instance) {
                _instance = null;
            }
        }

        private void CreateStartRoom() {
            int randStartPos = Random.Range(0, startingPositions.Length);
            transform.position = startingPositions[randStartPos].position;

            int randStarRoom = Random.Range(0, startingRooms.Length);
            Instantiate(startingRooms[randStarRoom], transform.position, Quaternion.identity);
        }
    }
}