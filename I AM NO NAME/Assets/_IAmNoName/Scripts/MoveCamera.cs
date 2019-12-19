/////-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 04/12/2019 17:49
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.IAmNoName
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target.transform.position.z);
        }
    }
}