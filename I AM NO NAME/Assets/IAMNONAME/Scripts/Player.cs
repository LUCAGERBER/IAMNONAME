///-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 04/12/2019 14:17
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.DefaultCompany.HackSlash.ProjectName {
	public class Player : MonoBehaviour {

        [SerializeField] private float speed = 5;
        [SerializeField] private float dashSpeed = 20;
        [SerializeField] private float dashLength = 1;

        [SerializeField] private AudioClip swordDraw;
        [SerializeField] private AudioClip swordReturned;
        [SerializeField] private AudioClip swordSlash;

        private float previousSpeed;
        private float elapsedTime;
        private AudioSource myAudioSource;

        private float elapsedSlowMoTime;
        private float slowDelay = .1f;


        private string Horizontal = "Horizontal";
        private string Vertical = "Vertical";

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponentInParent<Enemy>().Kill();
        }

        private void Start ()
        {
            previousSpeed = speed;
            myAudioSource = GetComponent<AudioSource>();
		}
		
		private void Update () 
        {
            //if(Input.GetKeyDown(KeyCode.Space)) myAudioSource.PlayOneShot(swordDraw);

            if (Input.GetKey(KeyCode.Space))
            {
                elapsedSlowMoTime += Time.deltaTime;

                if(elapsedSlowMoTime>= slowDelay) TimeManager.Instance.SlowTime();
                
            }

            if (Input.GetKeyUp(KeyCode.Space)) Dash();
		}

        private void FixedUpdate()
        {
            Move();
        }

        private void Dash()
        {
            TimeManager.Instance.waiting = false;
            previousSpeed = speed;
            speed = dashSpeed;
            myAudioSource.PlayOneShot(swordSlash);
            elapsedSlowMoTime = 0;
            //Invoke("ReturnSword", .2f);
        }

        public void ReturnSword()
        {
            myAudioSource.PlayOneShot(swordReturned);
        }

        private void Move()
        {
            if(previousSpeed != speed)
            {
                elapsedTime += Time.fixedDeltaTime;
                if(elapsedTime >= dashLength)
                {
                    speed = previousSpeed;
                    elapsedTime = 0;
                }
            }

            transform.position += transform.forward * Input.GetAxis(Horizontal) * speed * Time.fixedDeltaTime;
        }
    }
}