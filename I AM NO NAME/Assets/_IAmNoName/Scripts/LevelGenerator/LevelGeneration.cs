///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 09/12/2019 19:55
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.IAmNoName.LevelGenerator {
    public class LevelGeneration : MonoBehaviour {
        [SerializeField] private Transform[] startingPositions;
        [SerializeField] public GameObject[] rooms; // 0 = LR, 1 = LRB, 2 = LRT, 3 = LRTB
        [SerializeField] private LayerMask RoomMask;
        [SerializeField] private float moveAmount;
        [SerializeField] private float startTimeBtwSpawn = 0.25f;
        [Header("Level Size")]
        [SerializeField] private float minX;
        [SerializeField] private float maxX, minY, maxY;
        [Header("Direction Percent")]
        [SerializeField] private float right;
        [SerializeField] private float left, up, down;


        [HideInInspector] public bool stopGeneration = false;

        private float timeBtwRoom;
        private int direction;
        private int downCounter;

        private void Start() {
            int randStartPos = Random.Range(0, startingPositions.Length);
            transform.position = startingPositions[randStartPos].position;
            Instantiate(rooms[0], transform.position, Quaternion.identity);

            direction = Random.Range(1, 6);
            timeBtwRoom = startTimeBtwSpawn;
        }

        private void Update() {
            if (timeBtwRoom <= 0 && !stopGeneration) {
                Move();
                timeBtwRoom = startTimeBtwSpawn;
            } else {
                timeBtwRoom -= Time.deltaTime;
            }
        }


        private void Move() {
            if (direction <= 2) { // Move Right
                if (transform.position.x < maxX) {
                    downCounter = 0;

                    Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                    transform.position = newPos;

                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);

                    direction = Random.Range(1, 6);

                    if (direction == 3) {
                        direction = 2;
                    }
                    if (direction == 4) {
                        direction = 5;
                    }

                } else {
                    direction = 5;
                }

            } else if (direction <= 4) { // move Left
                if (transform.position.x > minX) {
                    downCounter = 0;

                    Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                    transform.position = newPos;

                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);

                    direction = Random.Range(3, 6);
                } else {
                    direction = 5;
                }

            } else if (direction == 5) { // Move Down

                downCounter++;

                if (transform.position.y > minY) {
                    RaycastHit hit;
                    bool roomUp = Physics.Raycast(transform.position, Vector3.up, out hit, 1, RoomMask);
                    Collider roomDetection = hit.collider;
                    if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3) {

                        if (downCounter >= 2) {
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

                    direction = Random.Range(1, 6);

                } else {
                    // stop generation
                    stopGeneration = true;
                }
            }

        }
    }
}