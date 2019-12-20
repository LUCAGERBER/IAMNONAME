///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 09/12/2019 19:45
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.MaximilienGalea.Juiciness.Juiciness.Level {
    public class SpawnObject : MonoBehaviour {

        [SerializeField] GameObject[] objects;

        private void Start() {
            int rand = Random.Range(0, objects.Length);
            GameObject instance = Instantiate(objects[rand], transform.position, Quaternion.identity);
            instance.transform.parent = transform;
        }

    }
}