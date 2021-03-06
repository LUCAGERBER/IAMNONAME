///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 09/12/2019 20:47
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.IAmNoName.LevelGenerator {
	public class RoomType : MonoBehaviour {

        [SerializeField] public Transform[] bottomExit;
        [SerializeField] public Transform[] topExit;
        [SerializeField] public Transform[] leftExit;
        [SerializeField] public Transform[] rightExit;

        public void RoomDestruction() {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == gameObject.layer) {
                Debug.Log("<size=22><color=red>Overlap</color></size>");
            }
        }
    }
}