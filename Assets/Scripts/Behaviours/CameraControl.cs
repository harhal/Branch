using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public static CameraControl MainCamera;

    [SerializeField]
    float Speed = 1;
    [SerializeField]
    float ZoomSpeed = 1;
    [SerializeField]
    Vector3 BottomLeftFarBoundPoint = new Vector3(-50, -50, -70);
    [SerializeField]
    Vector3 TopRightNearBoundPoint = new Vector3(50, 70, -30);
    [SerializeField]
    float StableSreenPurcent = 90;
    [SerializeField]
    Vector3 VectorUp = Vector3.up;
    [SerializeField]
    Vector3 VectorRight = Vector3.right;
    public new Camera camera;
    public Vector3 FixedOffset = new Vector3(2, 12, 0);

    bool IsFixed = false;

    public bool EnableKeyboardControl = true;
    public bool EnableMouseControl = true;
    public bool EnableGamepadControl = true;
    public bool EnableTouchscreenControl = true;

    public Vector3 Destination;


    // Use this for initialization
    void Awake () {
        camera = GetComponent<Camera>();
        if (MainCamera == null)
            MainCamera = this;
    }

    void HorisontalMove(Vector2 offset)
    {
        if (Destination.x <= BottomLeftFarBoundPoint.x && offset.x < 0) offset.x = 0;
        if (Destination.y <= BottomLeftFarBoundPoint.y && offset.y < 0) offset.y = 0;
        if (Destination.x >= TopRightNearBoundPoint.x && offset.x > 0) offset.x = 0;
        if (Destination.y >= TopRightNearBoundPoint.y && offset.y > 0) offset.y = 0;
        Vector3 CamOffset = (VectorRight * offset.x + VectorUp * offset.y) * Speed * Time.deltaTime;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = 0;
        CamOffset = Quaternion.Euler(rotation) * CamOffset;
        Destination += CamOffset;
    }

    void VertiacalMove(float offset, Vector3 target)
    {
        if (Destination.z <= BottomLeftFarBoundPoint.z && offset < 0) return;
        if (Destination.z >= TopRightNearBoundPoint.z && offset > 0) return;
        Vector3 forwardOffset = target * offset* ZoomSpeed * Time.deltaTime;
        Destination += forwardOffset;
    }

    void ClampMove()
    {
        Vector3 pos = Destination;
        pos.x = Mathf.Clamp(pos.x, BottomLeftFarBoundPoint.x, TopRightNearBoundPoint.x);
        pos.y = Mathf.Clamp(pos.y, BottomLeftFarBoundPoint.y, TopRightNearBoundPoint.y);
        pos.z = Mathf.Clamp(pos.z, BottomLeftFarBoundPoint.z, TopRightNearBoundPoint.z);
        Destination = pos;
    }

    public void Free()
    {
        IsFixed = false;
    }

    public void FocusAt(Vector3 Focus)
    {
        Destination = Focus - transform.rotation * FixedOffset;
        IsFixed = true;
    }

	void Update () {
        if (!IsFixed)
        {
            //KeyboardControl
            if (EnableKeyboardControl)
            {
                HorisontalMove(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
                VertiacalMove(Input.GetAxis("Keyboard Zoom"), transform.forward);
            }
            //MouseControl
            if (EnableMouseControl)
            {
                VertiacalMove(Input.GetAxis("Mouse ScrollWheel"), camera.ScreenPointToRay(Input.mousePosition).direction);
                Vector2 camOffset = Vector2.zero;
                float stablePurcent = StableSreenPurcent / 100;
                float triggerPurcent = (1 - stablePurcent) / 2;
                camOffset.x += Mathf.Clamp(Input.mousePosition.x / triggerPurcent / Screen.width - 1, -1, 0);
                camOffset.x += Mathf.Clamp(Input.mousePosition.x / triggerPurcent / Screen.width - 1 - stablePurcent / triggerPurcent, 0, 1);
                camOffset.y += Mathf.Clamp(Input.mousePosition.y / triggerPurcent / Screen.height - 1, -1, 0);
                camOffset.y += Mathf.Clamp(Input.mousePosition.y / triggerPurcent / Screen.height - 1 - stablePurcent / triggerPurcent, 0, 1);
                HorisontalMove(camOffset);
            }
            ClampMove();
        }
        if ((transform.position - Destination).magnitude > 1)
            transform.position += (Destination - transform.position) * 0.7f;
        else
            transform.position = Destination;
        /*if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            Vector2 mouseOffset = (lastMousePos - (Vector2)Input.mousePosition) * MouseTouchOffsetFactor;
            HorisontalMove(mouseOffset);
        }*/
        //lastMousePos = Input.mousePosition;
    }                                                                                                                                                                            
}
