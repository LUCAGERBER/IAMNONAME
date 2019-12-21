///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 09/12/2019 21:03
///-----------------------------------------------------------------

using Com.IsartDigital.IAmNoName.LevelGenerator;
using UnityEngine;

namespace Com.IsartDigital.IAmNoName.LevelGenerator {
    public class SpawnRoom : MonoBehaviour {

        [SerializeField] private LayerMask RoomMask;
        [SerializeField] private LayerMask exitMask;

        public event System.Action OnRoomCreated;

        private Vector3 distanceFromCenter;
        private Vector3 roomSize;
        private Collider[] colliders;
        private void Start() {
            if (LevelGenerator.Instance.stopGeneration) {
                Destroy(gameObject);
                return;
            }

            roomSize = transform.parent.GetComponent<BoxCollider>().size;

            colliders = Physics.OverlapSphere(transform.position, 1f, RoomMask);

            if (colliders.Length > 0) {
                Destroy(gameObject);
                return;
            }

            if (Physics.OverlapSphere(transform.position, 1f, exitMask).Length > 1) {
                SpawnWall();
                return;
            }

            OnRoomCreated += LevelGenerator.Instance.OnNewRoomCreated;

            distanceFromCenter = transform.position - transform.parent.position;
            //Debug.Log(distanceFromCenter);
            Invoke("CreateNewRoom", 0.1f);
            //CreateNewRoom();
        }

        private void SpawnWall() {
            Debug.Log("wall");
            Instantiate(LevelGenerator.Instance.walls[0], transform.position, Quaternion.identity);
        }

        private void CreateNewRoom() {
            GameObject nextRoom = null;
            Vector3 nextRoomPos = Vector3.zero;
            Vector3 nextRoomSize = Vector3.zero;
            // create right or left
            if (Mathf.Abs(distanceFromCenter.x) > Mathf.Abs(distanceFromCenter.y)) {
                if (distanceFromCenter.x > 0) {
                    int rand = Random.Range(0, LevelGenerator.Instance.leftRooms.Length);
                    nextRoom = Instantiate(LevelGenerator.Instance.leftRooms[rand], transform.position, Quaternion.identity);

                    nextRoomSize = nextRoom.GetComponent<BoxCollider>().size;

                    nextRoomPos = transform.parent.position + new Vector3(roomSize.x / 2 + nextRoomSize.x / 2, 0, 0);

                    nextRoom.transform.position = nextRoomPos;
                }
                // room with rigth open
                else {
                    int rand = Random.Range(0, LevelGenerator.Instance.rightRooms.Length);
                    nextRoom = Instantiate(LevelGenerator.Instance.rightRooms[rand], transform.position, Quaternion.identity);

                    nextRoomSize = nextRoom.GetComponent<BoxCollider>().size;

                    nextRoomPos = transform.parent.position - new Vector3(roomSize.x / 2 + nextRoomSize.x / 2, 0, 0);


                    nextRoom.transform.position = nextRoomPos;
                }
                // create up or down
            } else {
                if (distanceFromCenter.y > 0) {
                    int rand = Random.Range(0, LevelGenerator.Instance.bottomRooms.Length);
                    nextRoom = Instantiate(LevelGenerator.Instance.bottomRooms[rand], transform.position, Quaternion.identity);

                    nextRoomSize = nextRoom.GetComponent<BoxCollider>().size;

                    nextRoomPos = transform.parent.position + new Vector3(0, roomSize.y / 2 + nextRoomSize.y / 2, 0);

                    nextRoom.transform.position = nextRoomPos;
                }
                // room with rigth open
                else {
                    int rand = Random.Range(0, LevelGenerator.Instance.topRooms.Length);
                    nextRoom = Instantiate(LevelGenerator.Instance.topRooms[rand], transform.position, Quaternion.identity);

                    nextRoomSize = nextRoom.GetComponent<BoxCollider>().size;

                    nextRoomPos = transform.parent.position - new Vector3(0, roomSize.y / 2 + nextRoomSize.y / 2, 0);

                    nextRoom.transform.position = nextRoomPos;
                }
            }

            OnRoomCreated?.Invoke();
            Destroy(gameObject);
        }

        private void OnDestroy() {
            //OnRoomCreated = null;
        }

    }
}