///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 09/12/2019 20:47
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.MaximilienGalea.Juiciness.Juiciness.Level {
	public class RoomType : MonoBehaviour {

        public int type;

        public void RoomDestruction() {
            Destroy(gameObject);
        }
	}
}