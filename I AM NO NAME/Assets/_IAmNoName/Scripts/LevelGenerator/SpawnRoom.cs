///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 09/12/2019 21:03
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.MaximilienGalea.Juiciness.Juiciness.Level {
    public class SpawnRoom : MonoBehaviour {

        [SerializeField] private LayerMask RoomMask;
        [SerializeField] private LevelGeneration levelGen;

		private void Update () {
            Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, RoomMask);

            if (roomDetection == null && levelGen.stopGeneration == true) {
                int rand = Random.Range(0, levelGen.rooms.Length);
                Instantiate(levelGen.rooms[rand], transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
		}
	}
}