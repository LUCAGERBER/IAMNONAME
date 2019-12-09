///-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 04/12/2019 14:30
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace Com.DefaultCompany.HackSlash.ProjectName {
	public class TimeManager : MonoBehaviour {
		private static TimeManager instance;
		public static TimeManager Instance { get { return instance; } }

        [SerializeField] private float slowDownFactor = .05f;
        //[SerializeField] private float slowDownLength = 0.1f;

        public bool waiting = false;

		
		private void Awake(){
			if (instance){
				Destroy(gameObject);
				return;
			}
			
			instance = this;
		}

        private void Update()
        {
            //Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
            //Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
            if (waiting) return;
            Time.timeScale = 1;
        }

        public void SlowTime()
        {
            waiting = true;
            Time.timeScale = slowDownFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }

        public void HitStop(float duration)
        {
            if (waiting) return;
            Time.timeScale = 0;
            StartCoroutine(Wait(duration));
        }

        IEnumerator Wait(float duration)
        {
            waiting = true;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1;
            waiting = false;
        }
		
		private void OnDestroy(){
			if (this == instance) instance = null;
		}
	}
}