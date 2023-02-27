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


    private void LateUpdate()
    {
        WheelMovement();
        KeysMovement();
        BorderMovement();
        CameraZoom();
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
