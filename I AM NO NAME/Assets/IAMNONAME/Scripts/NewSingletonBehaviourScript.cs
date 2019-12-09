///-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 04/12/2019 16:22
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.DefaultCompany.HackSlash.ProjectName {
	public class NewSingletonBehaviourScript : MonoBehaviour {
		private static NewSingletonBehaviourScript instance;
		public static NewSingletonBehaviourScript Instance { get { return instance; } }
		
		private void Awake(){
			if (instance){
				Destroy(gameObject);
				return;
			}
			
			instance = this;
		}
		
		private void Start () {
			
		}
		
		private void Update () {
			
		}
		
		private void OnDestroy(){
			if (this == instance) instance = null;
		}
	}
}