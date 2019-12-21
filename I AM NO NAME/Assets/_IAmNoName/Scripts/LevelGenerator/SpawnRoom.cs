///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 09/12/2019 21:03
///-----------------------------------------------------------------

using Com.IsartDigital.IAmNoName.LevelGenerator;
using UnityEngine;

namespace Com.IsartDigital.IAmNoName.LevelGenerator {
    public class SpawnRoom : MonoBehaviour {

        [SerializeField] private LayerMask RoomMask;

        public event System.Action OnRoomCreated;

        private Vector3 distanceFromCenter;
        Collider[] colliders;
        private void Start() {
            if (LevelGeneration.Instance.stopGeneration) {
                return;
            }

            colliders = Physics.OverlapSphere(transform.position, 3, RoomMask);

            if (colliders.Length > 1) {
                Destroy(gameObject);
            }

            OnRoomCreated += LevelGeneration.Instance.OnNewRoomCreated;



            distanceFromCenter = transform.position - transform.parent.position;
            Debug.Log(distanceFromCenter);
            Invoke("CreateNewRoom", 0.1f);
            //CreateNewRoom();
        }

        private void CreateNewRoom() {
            GameObject nextRoom = null;

            // create right or left
            if (Mathf.Abs(distanceFromCenter.x) > Mathf.Abs(distanceFromCenter.y)) {
                if (distanceFromCenter.x > 0) {
                    int rand = Random.Range(0, LevelGeneration.Instance.leftRooms.Length);
                    nextRoom = Instantiate(LevelGeneration.Instance.leftRooms[rand], transform.position, Quaternion.identity);
                }
                // room with rigth open
                else {
                    int rand = Random.Range(0, LevelGeneration.Instance.rightRooms.Length);
                    nextRoom = Instantiate(LevelGeneration.Instance.rightRooms[rand], transform.position, Quaternion.identity);
                }
                // create up or down
            } else {
                if (distanceFromCenter.y > 0) {
                    int rand = Random.Range(0, LevelGeneration.Instance.bottomRooms.Length);
                    nextRoom = Instantiate(LevelGeneration.Instance.bottomRooms[rand], transform.position, Quaternion.identity);
                }
                // room with rigth open
                else {
                    int rand = Random.Range(0, LevelGeneration.Instance.topRooms.Length);
                    nextRoom = Instantiate(LevelGeneration.Instance.topRooms[rand], transform.position, Quaternion.identity);
                }
            }

            Vector3 nextRoomPos = nextRoom.transform.position - transform.parent.position;
            nextRoom.transform.position += nextRoomPos;
            OnRoomCreated?.Invoke();
            Destroy(gameObject);
        }

        private void OnDestroy() {
            //OnRoomCreated = null;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Room")) {
                Destroy(gameObject);
            }
        }
    }
}