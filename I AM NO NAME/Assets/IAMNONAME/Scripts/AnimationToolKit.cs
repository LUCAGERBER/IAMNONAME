///-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 08/11/2019 16:09
///-----------------------------------------------------------------

using Com.DefaultCompany.HackSlash.ProjectName;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.RushProject.Rush {
	public class AnimationToolKit : MonoBehaviour {

        private Dictionary<string, ParticleSystem> _particleDico = new Dictionary<string, ParticleSystem>();

        private Dictionary<string, AudioSource> _soundDico = new Dictionary<string, AudioSource>();
		private void Start ()
        {
            ParticleSystem[] foundParticles = GetComponentsInChildren<ParticleSystem>(true);
            AudioSource[] foundSound = GetComponents<AudioSource>();

            for (int i = 0; i < foundParticles.Length; i++)
            {
                if(!_particleDico.ContainsKey(foundParticles[i].name)) _particleDico.Add(foundParticles[i].name, foundParticles[i]);
            }

            for (int i = 0; i < foundSound.Length; i++)
            {
                if(!_soundDico.ContainsKey(foundSound[i].clip.name)) _soundDico.Add(foundSound[i].clip.name, foundSound[i]);
            }
		}

        public void PlayParticle(string particleName)
        {
            if (_particleDico.ContainsKey(particleName)) _particleDico[particleName].Play();
        }

        public void PlaySound(string soundName)
        {
            if (_soundDico.ContainsKey(soundName))
            {
                _soundDico[soundName].PlayOneShot(_soundDico[soundName].clip);
            }
        }

        public void ScreenShake(float magnitude)
        {
            if(FindObjectsOfType<CameraShake>().Length == 0)
            {
                Debug.Log("<color=red><size=21>Missing CameraShake</size></color>");
                return;
            }

            float duration = .15f;

            StartCoroutine(CameraShake.Instance.Shake(duration, magnitude));
        }

        public void HitStop(float duration)
        {
            if (FindObjectsOfType<TimeManager>().Length == 0)
            {
                Debug.Log("<color=red><size=21>Missing TimeManager</size></color>");
                return;
            }

            TimeManager.Instance.HitStop(duration);
        }

        public void DestroyOnEnd(float time)
        {
            Invoke("DestroyTimed", time);
        }

        private void DestroyTimed()
        {
            Destroy(gameObject);
        }
    }
}