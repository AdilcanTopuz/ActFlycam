using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class ActFlycam : MonoBehaviour
{
    [Range(0.01f, 0.1f)]
    public float smooth = 1f;
    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private bool isOrtho = false;
    private Camera cam;

    public bool useRotateKey;

    void Start()
    {
        rotationX = transform.eulerAngles.y;
        cam = GetComponent<Camera>();
        if (cam != null)
        {
            isOrtho = cam.orthographic;
        }

    }

    void Update()
    {

        // Cache deltaTime!
        var deltaTime = Time.deltaTime;
        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * smooth;
        rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * smooth;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        var tempRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        tempRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        if (!useRotateKey)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, tempRotation, smooth * 6.0f);

        }else if(useRotateKey && Input.GetMouseButton(1))
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, tempRotation, smooth * 6.0f);
        }



        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * smooth;
            transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * smooth;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * smooth;
            transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * smooth;
        }
        else
        {
            if (isOrtho)
            {
                cam.orthographicSize *= (1.0f - Input.GetAxis("Vertical") * smooth);
            }
            else
            {
                transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * smooth;
            }
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * smooth;
        }

        if (Input.GetKey(KeyCode.Q)) { transform.position -= transform.up * climbSpeed * smooth; }
        if (Input.GetKey(KeyCode.E)) { transform.position += transform.up * climbSpeed * smooth; }
    }
}

