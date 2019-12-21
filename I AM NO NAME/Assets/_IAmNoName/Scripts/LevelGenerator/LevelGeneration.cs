///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 09/12/2019 19:55
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.IAmNoName.LevelGenerator {
    public class LevelGeneration : MonoBehaviour {
        [SerializeField] private string seed = "seed";
        [SerializeField] private Transform[] startingPositions;
        [SerializeField] private GameObject[] startingRooms;
        [Header("Rooms")]
        [SerializeField] public GameObject[] leftRooms;
        [SerializeField] public GameObject[] rightRooms;
        [SerializeField] public GameObject[] topRooms;
        [SerializeField] public GameObject[] bottomRooms;
        [Space]
        [SerializeField] private LayerMask RoomMask;
        [SerializeField] private float moveAmount;
        [SerializeField] private float startTimeBtwSpawn = 0.25f;
        [Header("Level Size")]
        [SerializeField] private int maxRoomLegth;
        [SerializeField] private int maxRoom;
        // [SerializeField] private float minX;
        // [SerializeField] private float maxX, minY, maxY;
        [Header("Direction Percent")]
        [SerializeField] private float right;
        [SerializeField] private float left, up, down;

        
        [HideInInspector] public bool stopGeneration = false;

        private float _timeBtwRoom;
        private int _direction;
        private int _downCounter;
        private int _roomCount;
        private int _seed;

        private static LevelGeneration _instance;
        public static LevelGeneration Instance { get { return _instance; } }

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

            _seed = seed.GetHashCode();
            Random.InitState(_seed);
        }

        private void Start() {
            CreateStartRoom();

            _direction = Random.Range(0, 101);
            _timeBtwRoom = startTimeBtwSpawn;

            // define total percent
            left += right;
            up += left;
            down += up;
        }

        public void OnNewRoomCreated() {
            _roomCount++;
            if (_roomCount >= maxRoom) {
                stopGeneration = true;
            }
        }

        private void OnDestroy() {
            if (this == _instance) {
                _instance = null;
            }
        }

        //private void Update() {
        //    if (_timeBtwRoom <= 0 && !stopGeneration) {
        //        //Move();
        //        _timeBtwRoom = startTimeBtwSpawn;
        //    } else {
        //        _timeBtwRoom -= Time.deltaTime;
        //    }
        //}

        private void CreateStartRoom() {
            int randStartPos = Random.Range(0, startingPositions.Length);
            transform.position = startingPositions[randStartPos].position;

            int randStarRoom = Random.Range(0, startingRooms.Length);
            Instantiate(startingRooms[randStarRoom], transform.position, Quaternion.identity);
        }

        //private void newRoom(float x, float y,int roomLength, Vector3 previousPos) {
        //    if (_roomCount >= maxRoom) {
        //        return;
        //    }

        //    _roomCount++;

        //    while(++roomLength < maxRoomLegth ) {
        //        // init
        //        bool roomUsed = false;
        //        float xOffset = x - previousPos.x;
        //        float yOffset = y - previousPos.y;

        //        previousPos = new Vector3(x, y);

        //        // go frwd
        //        if (true) {

        //        }

        //    }
        //}

        /*
        private void Move() {
            if (_direction <= right) { // Move Right
                if (transform.position.x < maxX) {
                    _downCounter = 0;

                    Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                    transform.position = newPos;

                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);

                    _direction = Random.Range(1, 6);

                    if (_direction == 3) {
                        _direction = 2;
                    }
                    if (_direction == 4) {
                        _direction = 5;
                    }

                } else {
                    _direction = 5;
                }

            } else if (_direction <= left) { // move Left
                if (transform.position.x > minX) {
                    _downCounter = 0;

                    Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                    transform.position = newPos;

                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);

                    _direction = Random.Range(3, 6);
                } else {
                    _direction = 5;
                }

            } else if (_direction == 5) { // Move Down

                _downCounter++;

                if (transform.position.y > minY) {
                    RaycastHit hit;
                    bool roomUp = Physics.Raycast(transform.position, Vector3.up, out hit, 1, RoomMask);
                    Collider roomDetection = hit.collider;
                    if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3) {

                        if (_downCounter >= 2) {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            Instantiate(rooms[3], transform.position, Quaternion.identity);
                        } else {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();

                            int randBotRoom = Random.Range(1, 4);
                            if (randBotRoom == 2) {
                                randBotRoom = 1;
                            }
                            Instantiate(rooms[randBotRoom], transform.position, Quaternion.identity);
                        }

                    }

                    Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                    transform.position = newPos;

                    int rand = Random.Range(2, 4);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);

                    _direction = Random.Range(1, 6);

                } else {
                    // stop generation
                    stopGeneration = true;
                }
            }

        }
        */
    }
}