/*using UnityEngine;
using UnityEngine.InputSystem;

public class MainWeaponScript : MonoBehaviour
{
    
    public Transform firePoint;

    public MainWeaponData weaponData;

    public Transform rifleBase;
    public Transform riflePivot;
    Camera cam;
    float cooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!weaponData) return;

        if(weaponData.weaponType == MainWeaponData.WeaponType.Launcher)
        {
            var launcherData = weaponData.data as MainWeaponData.Launcher;
            if (launcherData == null) return;

            LookAt();

            if (Mouse.current.leftButton.isPressed == true && cooldown <= 0f) 
            {
                launcherData.Shoot(firePoint);
                cooldown = launcherData.bulletDelay;
            }

            if (cooldown>0f)
            {
                cooldown -= Time.fixedDeltaTime;
            }
        }

        else if(weaponData.weaponType == MainWeaponData.WeaponType.Laser)
        {
            ;
        }
        else if(weaponData.weaponType == MainWeaponData.WeaponType.Shield)
        {
            ;
        }

    }

      ol code      void LookAt(Vector3 lookPos)
            {
                Vector3 vectorToTarget = worldPos - weapon.position;
                //Vector3 rotatedVectorToTarget = Quaternion.Euler(0,0,90)*vectorToTarget;
                Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
                weapon.rotation = targetRotation.T;
                WeaponBase.rotation = 
            }

    public void LookAt()
    {
        // 1. Create a horizontal plane at the pivot's height.
        float planeY = rifleBase.position.y;
        Plane horizontalPlane = new Plane(Vector3.up, new Vector3(0, planeY, 0));

        // 2. Raycast  from the mouse position into the world.
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float distanceToPlane;
        Vector3 mousePlanePos = rifleBase.position; // Default value if raycast fails.

        if (horizontalPlane.Raycast(ray, out distanceToPlane))
        {
            mousePlanePos = ray.GetPoint(distanceToPlane);
        }

        // 3. Compute the horizontal direction from the pivot to the mouse.
        Vector3 direction1 = -(mousePlanePos - rifleBase.position);
        direction1.y = 0; // We only need the horizontal component for yaw.
        
        // Avoid rotating if the direction is too small.
        if (direction1.sqrMagnitude < 0.0001f)
            return;

        // 4. Compute the desired yaw rotation so that the forward direction points along 'direction'.
        Quaternion desiredRotation = Quaternion.LookRotation(direction1, Vector3.up);

        // 5. Set the turret base's rotation to match the desired yaw.
        // Preserve any local pitch/roll (if needed) by only modifying the y-component.
        Vector3 currentEuler = rifleBase.rotation.eulerAngles;
        rifleBase.rotation = Quaternion.Euler(currentEuler.x, desiredRotation.eulerAngles.y, currentEuler.z);
    



        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,  Input.mousePosition.y, cam.transform.position.y-1));

        // Calculate the direction from the child object to the third object
        Vector3 direction2 = mouseWorldPos - firePoint.position;

        // Calculate the rotation that would make the child object look at the third object
        Quaternion targetRotation = Quaternion.LookRotation(direction2);

        // Calculate the current rotation of the child object relative to the parent object
        Quaternion currentRelativeRotation = Quaternion.Inverse(riflePivot.rotation) * firePoint.rotation;

        // Calculate the new rotation for the parent object
        Quaternion topRotation = targetRotation * Quaternion.Inverse(currentRelativeRotation);

        // Set the rotation of the parent object
        riflePivot.rotation = topRotation;


    }
}
*/
/* Code of Shame

        Vector3 pointA,pointB,pointC,pivotD;
        firePoint.GetLocalPositionAndRotation(out pointA, out _);
        sightPoint.GetLocalPositionAndRotation(out pointB, out _);
        pointC = localMousePos;
        pivotD = Vector3.zero;
        
        // Step 1: Compute coefficients for the trigonometric equation:
        // eq: A*cos(theta) + B*sin(theta) = C
        // Note: These coefficients are computed using the coordinates of pointC, pointD, pointB, and pointA.
        float coeffA = (pointC.z - pivotD.z) * (pointB.y - pointA.y) - (pointC.y - pivotD.y) * (pointB.z - pointA.z);
        float coeffB = (pointC.y - pivotD.y) * (pointB.y - pointA.y) + (pointC.z - pivotD.z) * (pointB.z - pointA.z);
        float constC = (pointA.z - pivotD.z) * (pointB.y - pointA.y) - (pointA.y - pivotD.y) * (pointB.z - pointA.z);
        // Step 2: Calculate the magnitude R of the vector (coeffA, coeffB)
        float R = Mathf.Sqrt(coeffA * coeffA + coeffB * coeffB);
        if (Mathf.Approximately(R, 0f))
        {
            Debug.LogError("Invalid configuration: R is zero, cannot compute rotation.");
        }
        
        // Step 3: Ensure the value (constC / R) is in the valid range [-1, 1] for arccos.
        float ratio = constC / R;
        if (ratio < -1f || ratio > 1f)
        {
            Debug.LogError("No solution exists for the given points (|constC| > R).");
        }
        
        // Step 4: Find the angle phi such that:
        // cos(phi) = coeffA / R and sin(phi) = coeffB / R.
        // This is done using the atan2 function.
        float phi = Mathf.Atan2(coeffB, coeffA);
        
        // Step 5: Solve for theta using the identity:
        // R * cos(theta - phi) = constC  -->  cos(theta - phi) = constC / R
        // There are two possible solutions: theta - phi = arccos(ratio) or theta - phi = -arccos(ratio).
        float acosValue = Mathf.Acos(ratio);
        
        // Here we choose one solution: theta = phi + arccos(ratio).
        // (Depending on your needs, you might also consider the other solution.)
        float theta = phi + acosValue;
        
        // Convert theta from radians to degrees for clarity.
        float thetaDegrees = theta * Mathf.Rad2Deg;
        Debug.Log(theta + " " + thetaDegrees);

        riflePivot.localRotation = Quaternion.Euler(thetaDegrees, 0f, 0f);
*/
