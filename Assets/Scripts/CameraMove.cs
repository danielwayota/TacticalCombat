using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Zoom")]
    public float zoomSpeed = 12f;
    public float maxZoom = 16;
    public float minZoom = 4;

    private Camera cameraComp;

    void Start()
    {
        this.cameraComp = this.GetComponent<Camera>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            var direction = (new Vector3(h, v, 0)).normalized;
            this.transform.position = this.transform.position + (direction * this.moveSpeed * Time.deltaTime);
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            float zoomDelta = (Input.mouseScrollDelta.y * (-1)) * this.zoomSpeed * Time.deltaTime;

            float nextZoom = this.cameraComp.orthographicSize + zoomDelta;
            this.cameraComp.orthographicSize = Mathf.Clamp(nextZoom, this.minZoom, this.maxZoom);
        }
    }
}
