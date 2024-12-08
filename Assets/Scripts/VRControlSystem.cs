using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRControlSystem : MonoBehaviour {
    // player
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private Rigidbody playerRB;
    
    [SerializeField] private new GameObject camera;
    
    [SerializeField] private GameObject leftControllerObject;
    [SerializeField] private GameObject rightControllerObject;
    
    public KeyCode jumpButton = KeyCode.JoystickButton0;

    // movement
    private Vector2 joystickResult;
    
    private float joystickX;
    private float joystickY;
    
    // forces
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    
    private void Start() {
        // initialise rotations and positions
        camera.transform.rotation = Quaternion.identity;
        camera.transform.position = playerModel.transform.position;
        playerModel.transform.rotation = Quaternion.identity;

        leftControllerObject.transform.position = Vector3.zero;
        rightControllerObject.transform.position = Vector3.zero;
    }

    private void ResetPosition() {
        Vector3 resetPos = Vector3.zero;
        Quaternion resetRot = Quaternion.identity;

        foreach (Transform child in playerPrefab.transform) {
            child.position = resetPos;
            child.rotation = resetRot;
        }
        
        playerPrefab.transform.position = resetPos;
        playerPrefab.transform.rotation = resetRot;
    }
    
    private void Update() {
        Quaternion headsetRotation = InputTracking.GetLocalRotation(XRNode.Head);
        camera.transform.rotation = headsetRotation;
        playerModel.transform.position = playerRB.transform.position;
        playerModel.transform.rotation = Quaternion.Euler(0, headsetRotation.eulerAngles.y, 0);
    }

    private void FixedUpdate() {
        MovePlayer();
        if (Input.GetKeyDown(jumpButton)) {JumpPlayer();}
    }

    private void MovePlayer() {
        // movement
        joystickResult.x = Input.GetAxis("Horizontal");
        joystickResult.y = Input.GetAxis("Vertical");
        
        playerRB.AddForce(transform.right * (joystickResult.x * moveSpeed));
        playerRB.AddForce(transform.forward * (joystickResult.y * moveSpeed));
    }

    private void JumpPlayer() {
        playerRB.AddForce(transform.up * jumpForce);
    }
}
