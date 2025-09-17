using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    Vector2 cameraRotation;
    Vector3 cameraOffset;
    public Vector3 respawnPoint;
    InputAction lookVector;
    Camera playerCam;

    Rigidbody rb;
    Ray ray;
    Ray jumpRay;

    float verticalMove;
    float horizontalMove;

    public float speed = 5f;
    public float jumpHeight = 10f;
    public float Xsensitivty = 1.0f;
    public float Ysensitivty = 1.0f;
    public float camRotationLimit = 90.0f;
    public float climbingTouchDistance = 1f;
    public float jumpDetectionDistance = 1.1f;

    public int health = 5;
    public int maxHealth = 5;

    public void Start()
    {
        ray = new Ray(transform.position, transform.forward);
        jumpRay = new Ray(transform.position, -transform.up);
        cameraOffset = new Vector3(0, .5f, .5f);
        respawnPoint = new Vector3(0, 1, 0);
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;
        lookVector = GetComponent<PlayerInput>().currentActionMap.FindAction("Look");
        cameraRotation = Vector2.zero;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (health<= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        /* Camera Rotation System
        //playerCam.transform.position = transform.position + cameraOffset;

        cameraRotation.x += lookVector.ReadValue<Vector2>().x * Xsensitivty;
        cameraRotation.y += lookVector.ReadValue<Vector2>().y * Ysensitivty;

        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -camRotationLimit, camRotationLimit);

        //playerCam.transform.rotation = Quaternion.Euler(-cameraRotation.y, cameraRotation.x, 0);
        */

        Quaternion playerRotation = Quaternion.identity;
        playerRotation.y = playerCam.transform.rotation.y;
        playerRotation.w = playerCam.transform.rotation.w;
        transform.localRotation = playerRotation;

        ray.origin = transform.position;
        ray.direction = transform.forward;

        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        // Movement System
        Vector3 temp = rb.velocity;

        if (Physics.Raycast(ray, climbingTouchDistance))
            temp.y = verticalMove * speed;
        else
            temp.x = verticalMove * speed;
        
        temp.z = horizontalMove * speed;

        rb.velocity = (temp.x * transform.forward) +
                            (temp.y * transform.up) +
                            (temp.z * transform.right);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();

        verticalMove = inputAxis.y;
        horizontalMove = inputAxis.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "killzone")
            health = 0;

        
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "health") && (health < maxHealth))
        {
            health++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "hazard")
            health--;
    }

    public void Jump()
    {
        if(Physics.Raycast(jumpRay, jumpDetectionDistance))
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
    }
}
    
