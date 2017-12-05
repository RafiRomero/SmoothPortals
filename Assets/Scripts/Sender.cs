using UnityEngine;

public class Sender : MonoBehaviour {

    public GameObject receiver;

    void Start () {
    }

    private PortalUser portalUser;
    void FixedUpdate() //https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html "FixedUpdate should be used instead of Update when dealing with Rigidbody" (so, for physical objects, you might want this)
    {
        if (portalUser != null) {
            var currentDot = Vector3.Dot(transform.up, portalUser.transform.position - transform.position);

            if (currentDot < 0) // only transport the player once he's moved across plane
            {
                // transport them to the equivalent position in the other portal
				Vector3 playerRelativeToPortalPosLocal = transform.InverseTransformPoint(portalUser.transform.position);
				playerRelativeToPortalPosLocal.x = -playerRelativeToPortalPosLocal.x; //Prevent "mirroring"
				var newPosition = receiver.transform.TransformPoint(playerRelativeToPortalPosLocal);

				// And sort out new camera facing
				var receiverForwardAxis = receiver.transform.up;	//Collision-plane has a rotation of -90 in X, which transposes the axes we'd normally consider
				var senderForwardAxis = transform.up;				//Collision-plane has a rotation of -90 in X, which transposes the axes we'd normally consider
				var receiverUpAxis = receiver.transform.forward;	//Collision-plane has a rotation of -90 in X, which transposes the axes we'd normally consider
				var portalRotationalDifference = Quaternion.FromToRotation(senderForwardAxis, -receiverForwardAxis); //https://answers.unity.com/questions/702200/finding-the-rotation-between-two-gameobjects.html
				var newFacingDirection = portalRotationalDifference * portalUser.transform.forward;
				var newRotation = Quaternion.LookRotation(newFacingDirection, receiverUpAxis);

				portalUser.Teleport(newPosition, newRotation);

				portalUser = null;
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
		    portalUser = other.GetComponentInParent<PortalUser>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
		    portalUser = null;
        }
    }
}
