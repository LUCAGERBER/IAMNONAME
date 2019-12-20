///-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 04/12/2019 14:17
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.IAmNoName {
	public class Player : MonoBehaviour {

        [SerializeField] private float speed = 5;
        [SerializeField] private float dashSpeed = 20;
        [SerializeField] private float dashLength = 1;
        [SerializeField] private float jumpStrength = 5;
        [SerializeField] private float jumpLength = 2;
        [SerializeField] private float jumpHeight;

        [SerializeField] private AudioClip swordDraw;
        [SerializeField] private AudioClip swordReturned;
        [SerializeField] private AudioClip swordSlash;

        [SerializeField] private string jumpButton;

        [SerializeField] private LayerMask groundLayer;

        private float previousSpeed;
        private float elapsedTime;
        private float slowDelay = .1f;

        private AudioSource myAudioSource;

        private Rigidbody rb;

        private string Horizontal = "Horizontal";
        private string Vertical = "Vertical";

        private bool jumpKey;
        private bool isGrounded;



        private void OnTriggerEnter(Collider other)
        {
            other.GetComponentInParent<Enemy>().Kill();
        }

        private void Start ()
        {
            previousSpeed = speed;
            myAudioSource = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
		}
		
		private void Update () 
        {
            //if(Input.GetKeyDown(KeyCode.Space)) myAudioSource.PlayOneShot(swordDraw);

            jumpKey = Input.GetButton(jumpButton);

            if (Input.GetKeyDown(KeyCode.LeftShift)) TimeManager.Instance.SlowTime();
            else if(Input.GetKeyUp(KeyCode.LeftShift)) TimeManager.Instance.ResetTime();

            if (Input.GetMouseButtonDown(1)) Dash();
		}

        private void FixedUpdate()
        {
            Move();

            isGrounded = Physics.Raycast(transform.position, -transform.up, transform.localScale.x/2, groundLayer);

            if (jumpKey && isGrounded) Jump();
        }

        private void Dash()
        {
            TimeManager.Instance.ResetTime();
            previousSpeed = speed;
            speed = dashSpeed;
            myAudioSource.PlayOneShot(swordSlash);
            //Invoke("ReturnSword", .2f);
        }

        private void Jump()
        {
            StartCoroutine(JumpCoroutine());
        }

        private IEnumerator JumpCoroutine()
        {
            float elapsed = 0f;
            float baseHeight = rb.velocity.y;

            rb.AddForce(new Vector3(0, jumpStrength, 0), ForceMode.Impulse);

            while (elapsed <= jumpLength)
            {
                rb.velocity = new Vector3(0, Mathf.Clamp(rb.velocity.y, baseHeight, baseHeight + jumpHeight), rb.velocity.z);
                Debug.Log(rb.velocity.y);
                elapsed += Time.deltaTime;
                yield return null;
            }
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