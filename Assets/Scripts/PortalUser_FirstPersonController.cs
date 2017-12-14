using UnityEngine;

public class PortalUser_FirstPersonController : PortalUser {
	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController; 
	public override void Start() {
		base.Start();
		fpsController = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
	}

	public override void Teleport(Vector3 destinationPosition, Quaternion rotationQuaternion) {
		base.Teleport(destinationPosition, rotationQuaternion);

		transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation; //I think you're going to have to modify FirstPersonController to make this work smoothly with a lerp

		//This is a nasty hack for the standard assets FirstPersonController.
		//
		//By default, the FirstPersonController locks the player and camera rotation to the mouse position. This is usually fine, but
		//when portalling, it will instantly reset the rotation I applied in the base-class to update the player-facing based on portal orientation.
		//
		//I don't really want to change the code from the unity standard assets package in this example project.
		//So, I'm using this hack to:
		//	temporarily disable the FirstPersonController (this prevents Update() calling RotateView, which in turn would call m_MouseLook.LookRotation, and then mess up the rotation applied in the base-class);
		//	call "Start" - All I really want to do is call "FirstPersonController.m_MouseLook.Init(transform , m_Camera.transform);", but that isn't exposed, so I'm hacking it this way
		//	re-enable the FirstPersonController - now that m_MouseLook.Init has been called, it will all work just fine. 
		//
		//You should implement something nicer:)
		fpsController.enabled = false;
		fpsController.Invoke("Start", 0);
		fpsController.enabled = true;
	}
}
