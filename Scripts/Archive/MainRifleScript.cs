using UnityEngine;
using UnityEngine.InputSystem;

public class MainRifleScript : MonoBehaviour
{
    Camera cam;
    Vector2 mousePos;
    Vector3 point;
    public Transform gun;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float bulletDelay;
    float cooldown = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
    }

    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));   
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        point.z = gun.position.z;
        Vector3 vectorToTarget = point - gun.position;
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0,0,-90)*vectorToTarget;
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

		gun.rotation = targetRotation;

        if (Mouse.current.leftButton.isPressed == true && cooldown <= 0f) 
        {
            Shoot();
            cooldown = bulletDelay;
        }

        if (cooldown>0f)
        {
            cooldown -= Time.fixedDeltaTime;
        }

    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * -bulletForce, ForceMode2D.Impulse);
    }
}
