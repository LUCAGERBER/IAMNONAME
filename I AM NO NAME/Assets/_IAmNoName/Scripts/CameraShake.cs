///-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 04/12/2019 16:08
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.IAmNoName
{
    public class CameraShake : MonoBehaviour
    {

        private static CameraShake instance;
        public static CameraShake Instance { get { return instance; } }

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        public IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 originalPos = transform.localPosition;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = originalPos;

        }

        private void OnDestroy()
        {
            if (this == instance) instance = null;
        }
    }
}