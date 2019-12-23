///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 09/12/2019 20:47
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.IAmNoName.LevelGenerator {
	public class RoomType : MonoBehaviour {

        [SerializeField] private Transform[] bottomExit;
        [SerializeField] private Transform[] topExit;
        [SerializeField] private Transform[] leftExit;
        [SerializeField] private Transform[] rigtExit;

        public void RoomDestruction() {
            Destroy(gameObject);
        }
	}
}