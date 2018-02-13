using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField]
    float Speed = 1;
    [SerializeField]
    float MouseTouchOffsetFactor = 1;
    [SerializeField]
    float VerticalSpeed = 1;
    [SerializeField]
    float DeltaAngle = 90;
    [SerializeField]
    float MinHeight = 30;
    [SerializeField]
    float MaxHeight = 70;
    [SerializeField]
    float StableSreenPurcent = 90;
    new Camera camera;
    Vector2 lastMousePos;


	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
        lastMousePos = Input.mousePosition;
    }

    void HorisontalMove(Vector2 offset)
    {
        Vector3 CamOffset = new Vector3(offset.x, 0, offset.y) * Speed * Time.deltaTime;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = 0;
        CamOffset = Quaternion.Euler(rotation) * CamOffset;
        transform.position += CamOffset;
    }

    void VertiacalMove(float offset, Vector3 target)
    {
        Vector3 forwardOffset = target * offset* VerticalSpeed * Time.deltaTime;
        if ((transform.position + forwardOffset).y > MinHeight && (transform.position + forwardOffset).y < MaxHeight)
            transform.position += forwardOffset;
    }

    void Rotate(float offset)
    {
        Vector3 rotator = transform.rotation.eulerAngles;
        rotator.y += offset * DeltaAngle * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotator);
    }

	void Update () {
        //KeyboardControl
        HorisontalMove(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        VertiacalMove(Input.GetAxis("Keyboard Zoom"), transform.forward);
        Rotate(Input.GetAxis("Yaw"));
        //MouseControl
        VertiacalMove(Input.GetAxis("Mouse ScrollWheel"), camera.ScreenPointToRay(Input.mousePosition).direction);
        Vector2 camOffset = Vector2.zero;
        float stablePurcent = StableSreenPurcent / 100;
        float triggerPurcent = (1 - stablePurcent) / 2;
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        camOffset.x += Mathf.Clamp(Input.mousePosition.x / triggerPurcent / Screen.width  - 1                                 , -1, 0);
        camOffset.x += Mathf.Clamp(Input.mousePosition.x / triggerPurcent / Screen.width  - 1 - stablePurcent / triggerPurcent,  0, 1);
        camOffset.y += Mathf.Clamp(Input.mousePosition.y / triggerPurcent / Screen.height - 1                                 , -1, 0);
        camOffset.y += Mathf.Clamp(Input.mousePosition.y / triggerPurcent / Screen.height - 1 - stablePurcent / triggerPurcent,  0, 1);
        HorisontalMove(camOffset);
        /*if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            Vector2 mouseOffset = (lastMousePos - (Vector2)Input.mousePosition) * MouseTouchOffsetFactor;
            HorisontalMove(mouseOffset);
        }*/
        //lastMousePos = Input.mousePosition;
    }                                                                                                                                                                            
}
