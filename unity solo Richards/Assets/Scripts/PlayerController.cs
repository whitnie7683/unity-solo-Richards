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

    float verticalMove;
    float horizontalMove;

    public float speed = 5f;
    public float jumpHeight = 10f;
    public float Xsensitivty = 1.0f;
    public float Ysensitivty = 1.0f;
    public float camRotationLimit = 90.0f;

    public int health = 5;
    public int maxHealth = 5;

    public void Start()
    {
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
        // Camera Rotation System
        playerCam.transform.position = transform.position + cameraOffset;

        cameraRotation.x += lookVector.ReadValue<Vector2>().x * Xsensitivty;
        cameraRotation.y += lookVector.ReadValue<Vector2>().y * Ysensitivty;

        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -camRotationLimit, camRotationLimit);

        playerCam.transform.rotation = Quaternion.Euler(-cameraRotation.y, cameraRotation.x, 0);
        transform.localRotation = Quaternion.AngleAxis(cameraRotation.x, Vector3.up);


        // Movement System
        Vector3 temp = rb.velocity;

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
        
    }
}