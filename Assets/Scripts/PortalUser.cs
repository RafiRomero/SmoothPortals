using UnityEngine;

public class PortalUser : MonoBehaviour {
	public virtual void Teleport(Vector3 destinationPosition, Quaternion newRotation) {
		transform.position = destinationPosition;
		transform.rotation = newRotation;
	}
}
