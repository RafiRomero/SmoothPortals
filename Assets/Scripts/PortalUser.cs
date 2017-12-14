using UnityEngine;

public class PortalUser : MonoBehaviour {

	[SerializeField] protected Collider _teleporationTrigger = null;

	public virtual Collider TeleporationTrigger {get { return _teleporationTrigger;} }
	public virtual bool IsTeleporationTrigger (Collider other) { return other == TeleporationTrigger; }

	public virtual void Start() {
		if (_teleporationTrigger == null) {
			var childColliders = gameObject.GetComponentsInChildren<Collider>();
			foreach (var collider in childColliders) {
				if ( collider.bounds.size.magnitude >= ( _teleporationTrigger == null ? collider : _teleporationTrigger ).bounds.size.magnitude )
					_teleporationTrigger = collider;
			}
		}
		
		if (_teleporationTrigger == null)
			Debug.LogError("PortalUser component '" + name + "' does not have a _teleporationTrigger collider, nor could it find one.");
	}

	public virtual void Teleport(Vector3 destinationPosition, Quaternion newRotation) {
		transform.position = destinationPosition;
		transform.rotation = newRotation;
	}
}
