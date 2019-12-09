///-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 04/12/2019 14:51
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.DefaultCompany.HackSlash.ProjectName {
	public class Enemy : MonoBehaviour {

        [SerializeField] private float speed = 5f;

        private float elapsedTime = 0;
        private int roundChangeTime = 3;

        private bool isDying = false;
		private void FixedUpdate ()
        {
            if (isDying) return;
            LookAround();
            Move();
		}

        private void Move()
        {
            transform.position += transform.forward * speed * Time.fixedDeltaTime;
        }

        private void LookAround()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= roundChangeTime)
            {
                speed *= -1;
                elapsedTime = 0;
            }
        }

        public void Kill()
        {
            transform.rotation = Quaternion.AngleAxis(Random.Range(0, 180), transform.up);
            GetComponent<Animator>().SetTrigger("Death");
            StartCoroutine(CameraShake.Instance.Shake(.15f,.4f));
            TimeManager.Instance.HitStop(.1f);
            isDying = true;
        }
    }
}