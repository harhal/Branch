using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField]
    float Speed = 1;
    [SerializeField]
    float ZoomSpeed = 1;
    [SerializeField]
    float MinDistance = 30;
    [SerializeField]
    float MaxDistance = 70;
    [SerializeField]
    float StableSreenPurcent = 90;
    [SerializeField]
    Vector3 VectorUp = Vector3.up;
    [SerializeField]
    Vector3 VectorRight = Vector3.right;
    new Camera camera;


	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
    }

    void HorisontalMove(Vector2 offset)
    {
        Vector3 CamOffset = (VectorRight * offset.x + VectorUp * offset.y) * Speed * Time.deltaTime;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = 0;
        CamOffset = Quaternion.Euler(rotation) * CamOffset;
        transform.position += CamOffset;
    }

    void VertiacalMove(float offset, Vector3 target)
    {
        Vector3 forwardOffset = target * offset* ZoomSpeed * Time.deltaTime;
        if (Mathf.Abs((transform.position + forwardOffset - target).z) < MinDistance && offset > 0) return;
        if (Mathf.Abs((transform.position + forwardOffset - target).z) > MaxDistance && offset < 0) return;
            transform.position += forwardOffset;
    }

	void Update () {
        //KeyboardControl
        HorisontalMove(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        VertiacalMove(Input.GetAxis("Keyboard Zoom"), transform.forward);
        //MouseControl
        VertiacalMove(Input.GetAxis("Mouse ScrollWheel"), camera.ScreenPointToRay(Input.mousePosition).direction);
        Vector2 camOffset = Vector2.zero;
        float stablePurcent = StableSreenPurcent / 100;
        float triggerPurcent = (1 - stablePurcent) / 2;
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
