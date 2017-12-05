using UnityEngine;

public class PortalCamera : MonoBehaviour {

    public GameObject playerCamera;
    public MeshRenderer renderPlane;
	public Shader portalShader;
    public GameObject portal;
    public GameObject otherPortal;

    // Use this for initialization
    void Start () {
		var camera = GetComponent<Camera>();
        if (camera.targetTexture != null)
            camera.targetTexture.Release();

        camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

		renderPlane.material = new Material (portalShader);
        renderPlane.material.mainTexture = camera.targetTexture;	
	}
	
	// Each frame reposition the camera to mimic the players offset from the other portals position
	void Update () {
        // Get player cam position relative to our portal, in local co-ordinates (so, will take rotation into account)
		var playerCamRelativeToPortalPosLocal = portal.transform.InverseTransformPoint(playerCamera.transform.position);

		// Apply the offset relative to our portal to the other portal, and then convert to world co-ordinates
        transform.position = otherPortal.transform.TransformPoint(playerCamRelativeToPortalPosLocal);
		var oldRotation = transform.localRotation; //!!!:RotateAround modifies rotation as well as position, which we don't want; probably a better way to do this!
		transform.RotateAround(otherPortal.transform.position, otherPortal.transform.up, 180);
		transform.localRotation = oldRotation;

		//adjust rotation of camera
		var portalRotationalDifference = Quaternion.FromToRotation(portal.transform.forward, -otherPortal.transform.forward); //https://answers.unity.com/questions/702200/finding-the-rotation-between-two-gameobjects.html
		var newFacingDirection = portalRotationalDifference * playerCamera.transform.forward;
		transform.rotation = Quaternion.LookRotation(newFacingDirection, otherPortal.transform.up);
    }
}
