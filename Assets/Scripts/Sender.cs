using UnityEngine;

public class Sender : MonoBehaviour {

    public GameObject receiver;

    private PortalUser _portalUser;	//###:TODO - really need a list or map of these, so multiple objects can be colliding with the portal at once (e.g,. large rectangle entres but does not teleport thorugh portal; then sphere enters)
    void FixedUpdate() //https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html "FixedUpdate should be used instead of Update when dealing with Rigidbody" (so, for physical objects, you might want this)
    {
        if (_portalUser != null) {
            var currentDot = Vector3.Dot(transform.up, _portalUser.transform.position - transform.position);

            if (currentDot < 0) // only transport the player once they've moved across plane
            {
                // transport them to the equivalent position in the other portal
				Vector3 playerRelativeToPortalPosLocal = transform.InverseTransformPoint(_portalUser.transform.position);
				playerRelativeToPortalPosLocal.x = -playerRelativeToPortalPosLocal.x; //Prevent "mirroring"
				playerRelativeToPortalPosLocal.y = -playerRelativeToPortalPosLocal.y; //We have penerated the sender portal; need to "pop out of" the receiver portal; we use y-axis as forward as collision plane has a -90 degree rotation in X, which transposes Y, which we'd normally use, into X
				var newPosition = receiver.transform.TransformPoint(playerRelativeToPortalPosLocal);

				// And sort out new camera facing
				var receiverForwardAxis = receiver.transform.up;	//Collision-plane has a rotation of -90 in X, which transposes the axes we'd normally consider
				var senderForwardAxis = transform.up;				//Collision-plane has a rotation of -90 in X, which transposes the axes we'd normally consider
				var receiverUpAxis = receiver.transform.forward;	//Collision-plane has a rotation of -90 in X, which transposes the axes we'd normally consider
				var portalRotationalDifference = Quaternion.FromToRotation(senderForwardAxis, -receiverForwardAxis); //https://answers.unity.com/questions/702200/finding-the-rotation-between-two-gameobjects.html
				var newFacingDirection = portalRotationalDifference * _portalUser.transform.forward;
				var newRotation = Quaternion.LookRotation(newFacingDirection, receiverUpAxis);

				_portalUser.Teleport(newPosition, newRotation);

				_portalUser = null;
            }
        }
    }

	private PortalUser FindPortalUser(Collider other) {
		PortalUser portalUser = null;

		var portalUserCandidate = other.transform;
		do {
			portalUser = portalUserCandidate.GetComponent<PortalUser>();
			if (portalUser != null)
				break;
			portalUserCandidate = portalUserCandidate.transform.parent;
        }
		while (portalUserCandidate != null);

		return portalUser;
	}

    void OnTriggerEnter(Collider other)
    {
		var portalUser = FindPortalUser(other);
		if ( portalUser != null && portalUser.IsTeleporationTrigger( other ) )
			_portalUser = portalUser;
    }

    void OnTriggerExit(Collider other)
    {
		var portalUser = FindPortalUser(other);
		if ( portalUser != null && portalUser.IsTeleporationTrigger( other ) )
			_portalUser = null;
    }
}
