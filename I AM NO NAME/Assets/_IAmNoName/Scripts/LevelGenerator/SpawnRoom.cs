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

        public event System.Action<GameObject> OnRoomCreated;

        private Vector3 _distanceFromCenter;
        private Vector3 _roomSize;
        private Collider[] _colliders;
        private void Start() {
            _colliders = Physics.OverlapSphere(transform.position, 1f, RoomMask);

            if (_colliders.Length > 0) {
                Destroy(gameObject);
                return;
            }

            if (LevelGenerator.Instance.stopGeneration) {
                SpawnWall();
                Destroy(gameObject);
                return;
            }

            _roomSize = transform.parent.GetComponent<BoxCollider>().size;



            if (Physics.OverlapSphere(transform.position, 2f, exitMask).Length > 1) {
                SpawnWall();
                return;
            }

            OnRoomCreated += LevelGenerator.Instance.OnNewRoomCreated;

            _distanceFromCenter = transform.position - transform.parent.position;
            Invoke("CreateNewRoom", LevelGenerator.Instance.timeBetweenSpawn);
        }

        private void SpawnWall() {

            GameObject wall = Instantiate(LevelGenerator.Instance.walls[0], transform.position, Quaternion.identity);
            wall.transform.SetParent(transform.parent);
            Destroy(gameObject);
        }

        private void CreateNewRoom() {
            GameObject nextRoom = null;
            Vector3 nextRoomPos = Vector3.zero;
            Vector3 nextRoomSize = Vector3.zero;
            // create right or left
            if (Mathf.Abs(_distanceFromCenter.x) > Mathf.Abs(_distanceFromCenter.y)) {
                if (_distanceFromCenter.x > 0) {
                    int rand = Random.Range(0, LevelGenerator.Instance.leftRooms.Length);
                    nextRoom = Instantiate(LevelGenerator.Instance.leftRooms[rand], transform.position, Quaternion.identity);

                    RoomType nextRoomScript = nextRoom.GetComponent<RoomType>();
                    int rand2 = Random.Range(0, nextRoomScript.leftExit.Length);
                    Vector3 nextRoomEntrancePosition = nextRoomScript.leftExit[rand2].position - nextRoom.transform.position;

                    nextRoomSize = nextRoom.GetComponent<BoxCollider>().size;

                    nextRoomPos = transform.parent.position + new Vector3(_roomSize.x / 2 + nextRoomSize.x / 2, _distanceFromCenter.y - nextRoomEntrancePosition.y, 0);
                }
                // room with rigth open
                else {
                    int rand = Random.Range(0, LevelGenerator.Instance.rightRooms.Length);
                    nextRoom = Instantiate(LevelGenerator.Instance.rightRooms[rand], transform.position, Quaternion.identity);

                    RoomType nextRoomScript = nextRoom.GetComponent<RoomType>();
                    int rand2 = Random.Range(0, nextRoomScript.rightExit.Length);
                    Vector3 nextRoomEntrancePosition = nextRoomScript.rightExit[rand2].position - nextRoom.transform.position;
                    
                    nextRoomSize = nextRoom.GetComponent<BoxCollider>().size;

                    nextRoomPos = transform.parent.position - new Vector3(_roomSize.x / 2 + nextRoomSize.x / 2, - _distanceFromCenter.y + nextRoomEntrancePosition.y, 0);
                }
                // create up or down
            } else {
                if (_distanceFromCenter.y > 0) {
                    int rand = Random.Range(0, LevelGenerator.Instance.bottomRooms.Length);
                    nextRoom = Instantiate(LevelGenerator.Instance.bottomRooms[rand], transform.position, Quaternion.identity);

                    RoomType nextRoomScript = nextRoom.GetComponent<RoomType>();
                    int rand2 = Random.Range(0, nextRoomScript.bottomExit.Length);
                    Vector3 nextRoomEntrancePosition = nextRoomScript.bottomExit[rand2].position - nextRoom.transform.position;

                    nextRoomSize = nextRoom.GetComponent<BoxCollider>().size;

                    nextRoomPos = transform.parent.position + new Vector3(_distanceFromCenter.x - nextRoomEntrancePosition.x, _roomSize.y / 2 + nextRoomSize.y / 2, 0);
                } else {
                    int rand = Random.Range(0, LevelGenerator.Instance.topRooms.Length);
                    nextRoom = Instantiate(LevelGenerator.Instance.topRooms[rand], transform.position, Quaternion.identity);

                    RoomType nextRoomScript = nextRoom.GetComponent<RoomType>();
                    int rand2 = Random.Range(0, nextRoomScript.topExit.Length);
                    Vector3 nextRoomEntrancePosition = nextRoomScript.topExit[rand2].position - nextRoom.transform.position;

                    nextRoomSize = nextRoom.GetComponent<BoxCollider>().size;

                    nextRoomPos = transform.parent.position - new Vector3(-_distanceFromCenter.x - nextRoomEntrancePosition.x, _roomSize.y / 2 + nextRoomSize.y / 2, 0);
                }


            }
            nextRoom.transform.position = nextRoomPos;
            nextRoom.transform.SetParent(transform.parent);

            OnRoomCreated?.Invoke(nextRoom);
            Destroy(gameObject);
        }

        private void OnDestroy() {
            OnRoomCreated = null;
        }

    }
}