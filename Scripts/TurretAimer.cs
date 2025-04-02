using UnityEngine;

public class TurretAimer : MonoBehaviour
{
    public Transform baseRotator;
    public Transform pivotRotator;
    public Transform firePoint;

    private Camera cam;
    public Transform currentTarget;

    void Start()
    {
        cam = Camera.main;
    }

    public void PlayerLookAt()
    {
        // 1. Create a horizontal plane at the pivot's height.
        float planeY = baseRotator.position.y;
        Plane horizontalPlane = new Plane(Vector3.up, new Vector3(0, planeY, 0));

        // 2. Raycast  from the mouse position into the world.
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float distanceToPlane;
        Vector3 mousePlanePos = baseRotator.position; // Default value if raycast fails.

        if (horizontalPlane.Raycast(ray, out distanceToPlane))
        {
            mousePlanePos = ray.GetPoint(distanceToPlane);
        }

        // 3. Compute the horizontal direction from the pivot to the mouse.
        Vector3 direction1 = -(mousePlanePos - baseRotator.position);
        direction1.y = 0; // We only need the horizontal component for yaw.
        
        // Avoid rotating if the direction is too small.
        if (direction1.sqrMagnitude < 0.0001f)
            return;

        // 4. Compute the desired yaw rotation so that the forward direction points along 'direction'.
        Quaternion desiredRotation = Quaternion.LookRotation(direction1, Vector3.up);

        // 5. Set the turret base's rotation to match the desired yaw.
        // Preserve any local pitch/roll (if needed) by only modifying the y-component.
        Vector3 currentEuler = baseRotator.rotation.eulerAngles;
        baseRotator.rotation = Quaternion.Euler(currentEuler.x, desiredRotation.eulerAngles.y, currentEuler.z);
    



        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,  Input.mousePosition.y, cam.transform.position.y-1));

        // Calculate the direction from the child object to the third object
        Vector3 direction2 = mouseWorldPos - firePoint.position;

        // Calculate the rotation that would make the child object look at the third object
        Quaternion targetRotation = Quaternion.LookRotation(direction2);

        // Calculate the current rotation of the child object relative to the parent object
        Quaternion currentRelativeRotation = Quaternion.Inverse(pivotRotator.rotation) * firePoint.rotation;

        // Calculate the new rotation for the parent object
        Quaternion topRotation = targetRotation * Quaternion.Inverse(currentRelativeRotation);

        // Set the rotation of the parent object
        pivotRotator.rotation = topRotation;
    }
    public void AutoLookAt(Vector3 pos)
    {
        Vector3 planePos = pos;
        planePos.y = baseRotator.position.y;
        // 3. Compute the horizontal direction from the pivot to the mouse.
        Vector3 direction1 = -(planePos - baseRotator.position);
        direction1.y = 0; // We only need the horizontal component for yaw.
        
        // Avoid rotating if the direction is too small.
        if (direction1.sqrMagnitude < 0.0001f)
            return;

        // 4. Compute the desired yaw rotation so that the forward direction points along 'direction'.
        Quaternion desiredRotation = Quaternion.LookRotation(direction1, Vector3.up);

        // 5. Set the turret base's rotation to match the desired yaw.
        // Preserve any local pitch/roll (if needed) by only modifying the y-component.
        Vector3 currentEuler = baseRotator.rotation.eulerAngles;
        baseRotator.rotation = Quaternion.Euler(currentEuler.x, desiredRotation.eulerAngles.y, currentEuler.z);

    }
}

/*
 * 
        // Calculate the direction from the child object to the third object
        Vector3 direction2 = pos - firePoint.position;

        // Calculate the rotation that would make the child object look at the third object
        Quaternion targetRotation = Quaternion.LookRotation(direction2);

        // Calculate the current rotation of the child object relative to the parent object
        Quaternion currentRelativeRotation = Quaternion.Inverse(pivotRotator.rotation) * firePoint.rotation;

        // Calculate the new rotation for the parent object
        Quaternion topRotation = targetRotation * Quaternion.Inverse(currentRelativeRotation);

        // Set the rotation of the parent object
        pivotRotator.rotation = topRotation;
*/