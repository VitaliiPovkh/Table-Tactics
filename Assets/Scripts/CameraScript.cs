using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float minMovementSpeed = 15;
    [SerializeField] private float maxMovementSpeed = 100;

    [SerializeField] private float minZoom = 20;
    [SerializeField] private float maxZoom = 65;
    [SerializeField] private float zoomSpeed = 20;

    [SerializeField] private float borderWidth;
    public float AvarageCamSpeed { get => (minMovementSpeed + maxMovementSpeed) / 2; }


    private Vector2 startMousePosition;

    private float maxVertical = 70f;
    private float minVertical = -286;

    private float maxHorizontal = 416f;
    private float minHorizontal = -97f;

    private void LateUpdate()
    {
        WheelMovement();
        KeysMovement();
        BorderMovement();
        CameraZoom();

        Vector3 fixedPosition = transform.position;
        if (transform.position.x > maxHorizontal)
        {
            fixedPosition = new Vector3(maxHorizontal, fixedPosition.y, fixedPosition.z);
        }
        if (transform.position.x < minHorizontal)
        {
            fixedPosition = new Vector3(minHorizontal, fixedPosition.y, fixedPosition.z);
        }
        if (transform.position.y > maxVertical)
        {
            fixedPosition = new Vector3(fixedPosition.x, maxVertical, fixedPosition.z);
        }
        if (transform.position.y < minVertical)
        {
            fixedPosition = new Vector3(fixedPosition.x, minVertical, fixedPosition.z);
        }
        transform.position = fixedPosition;
    }

    private void WheelMovement()
    {
        if (Input.GetMouseButtonDown(2))
        {
            startMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            float xDist = Mathf.Min(Screen.width - Input.mousePosition.x, Input.mousePosition.x);
            float yDist = Mathf.Min(Screen.height - Input.mousePosition.y, Input.mousePosition.y);

            float speedX = Mathf.Lerp(minMovementSpeed, maxMovementSpeed, Mathf.InverseLerp(Screen.width / 2, 0f, xDist));
            float speedY = Mathf.Lerp(minMovementSpeed, maxMovementSpeed, Mathf.InverseLerp(Screen.height / 2, 0f, yDist));
            float speed = Mathf.Clamp(Mathf.Max(speedX, speedY), minMovementSpeed, maxMovementSpeed);

            Vector3 moveDir = ((Vector2)Input.mousePosition - startMousePosition).normalized;
            transform.position += speed * Time.deltaTime * moveDir;
        }
    }

    private void KeysMovement()
    {
        Vector3 direction = new Vector3() 
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical"),
            z = 0f 
        }.normalized;
        transform.position += AvarageCamSpeed * Time.deltaTime * direction;
    }

    private void BorderMovement()
    {
        float deltaX = 0, deltaY = 0;

        if (Input.mousePosition.x < borderWidth)
        {
            deltaX = -1.0f;
        }
        else if (Input.mousePosition.x > Screen.width - borderWidth)
        {
            deltaX = 1.0f;
        }

        if (Input.mousePosition.y < borderWidth)
        {
            deltaY = -1.0f;
        }
        else if (Input.mousePosition.y > Screen.height - borderWidth)
        {
            deltaY = 1.0f;
        }

        transform.position += AvarageCamSpeed * Time.deltaTime * new Vector3(deltaX, deltaY, 0);
    }

    private void CameraZoom()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (zoomDelta != 0)
        {
            float newZoom = Camera.main.orthographicSize - zoomDelta;
            Camera.main.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
        }
    }


}
