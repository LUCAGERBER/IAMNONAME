///-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 04/12/2019 14:17
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.IAmNoName {
	public class Player : MonoBehaviour {

		private float currentSpeed = 5;
		private bool isRunning = false;
		[Header("Run values")]
        [SerializeField] private float speed = 5;
		// Vitesse maximale d'impulsion avant la course
        [SerializeField] private float impulseSpeed = 10;
		// G�re le temps pendant lequel on est � impulseSpeed avant de revenir � speed
		[SerializeField] private float impulseLength = 1f;

		[Space]
        [SerializeField] private float dashSpeed = 20;
        [SerializeField] private float dashLength = 1;

        //G�re la vitesse de mont� du saut
        [SerializeField] private float jumpStrength = 5;

        //G�re le temps en suspension avant de redescendre 
        [SerializeField] private float jumpLength = 2;

        //G�re la hauteur max du saut
        [SerializeField] private float jumpHeight;

        //G�re la vitesse de descente du saut
        [SerializeField] private float jumpDownStrength = 1;

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
			currentSpeed = impulseSpeed;
            previousSpeed = currentSpeed;
            myAudioSource = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
		}
		
		private void Update () 
        {
            //if(Input.GetKeyDown(KeyCode.Space)) myAudioSource.PlayOneShot(swordDraw);

            jumpKey = Input.GetButton(jumpButton);

            if (Input.GetKeyDown(KeyCode.LeftShift)) TimeManager.Instance.SlowTime();
            else if(Input.GetKeyUp(KeyCode.LeftShift)) TimeManager.Instance.ResetTime();

			//if (Input.GetMouseButtonDown(1)) Dash();

			if (!isRunning && Input.GetAxis(Horizontal) != 0f) Run();
			else if (Input.GetAxis(Horizontal) == 0f) isRunning = false;
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
            previousSpeed = currentSpeed;
            currentSpeed = dashSpeed;
            myAudioSource.PlayOneShot(swordSlash);
            //Invoke("ReturnSword", .2f);
        }

        private void Jump()
        {
            StartCoroutine(JumpCoroutine());
        }

        //Coroutine permet de g�rer s�par�ment mont�, stagnation et descente du saut.
        private IEnumerator JumpCoroutine()
        {
            float elapsed = 0f;
            float baseHeight = rb.velocity.y;

            rb.AddForce(new Vector3(0, jumpStrength, 0), ForceMode.Impulse);

            while (elapsed <= jumpLength)
            {
                rb.velocity = new Vector3(0, Mathf.Clamp(rb.velocity.y, baseHeight, baseHeight + jumpHeight), rb.velocity.z);
                elapsed += Time.deltaTime;
                yield return null;
            }

            rb.AddForce(new Vector3(0, jumpDownStrength, 0), ForceMode.Impulse);
		}

		private void Run()
		{
			Debug.Log("Start run");
			isRunning = true;
			previousSpeed = currentSpeed = impulseSpeed;
			StartCoroutine(RunCoroutine());
		}

		private IEnumerator RunCoroutine()
		{
			float elapsed = 0f;
			float hValue = 0f;

			while (hValue != 1f)
			{
				Debug.Log("Waiting max input");
				hValue = Mathf.Abs(Input.GetAxis(Horizontal));

				if (Input.GetAxis(Horizontal) == 0f)
				{
					Debug.Log("Quit run before max input");
					elapsed = impulseLength;
					hValue = 1f;
				}

				yield return null;
			}

			while (elapsed <= impulseLength)
			{
				elapsed += Time.deltaTime;
				Debug.Log("while at maxInput");
				yield return null;
			}

			previousSpeed = currentSpeed = speed;
		}

		public void ReturnSword()
        {
            myAudioSource.PlayOneShot(swordReturned);
        }

		private void Move()
        {
            if(previousSpeed != currentSpeed)
            {
                elapsedTime += Time.fixedDeltaTime;
                if(elapsedTime >= dashLength)
                {
                    currentSpeed = previousSpeed;
                    elapsedTime = 0;
                }
            }

            transform.position += transform.forward * Input.GetAxis(Horizontal) * currentSpeed * Time.fixedDeltaTime;
        }
    }
}