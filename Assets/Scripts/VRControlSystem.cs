using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRControlSystem : MonoBehaviour {
    // player
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private GameObject groundCheck;
    
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
    public float jumpForce = 50f;
    
    private void Update() {
        Quaternion headsetRotation = InputTracking.GetLocalRotation(XRNode.Head);
        camera.transform.rotation = headsetRotation;
        camera.transform.position = playerRB.transform.position;
        playerModel.transform.rotation = Quaternion.Euler(0, headsetRotation.eulerAngles.y, 0);
    }

    private void FixedUpdate() {
        MovePlayer();
        if (Input.GetKey(jumpButton)) {JumpPlayer();}
        
        print(playerRB.linearVelocity);
    }

    private void MovePlayer() {
        // movement
        joystickResult.x = Input.GetAxis("Horizontal");
        joystickResult.y = Input.GetAxis("Vertical");

        if (joystickResult.x == 0 || joystickResult.y == 0) {
            playerRB.linearVelocity = Vector2.zero;
        }
        
        playerRB.AddForce(transform.right * (joystickResult.x * moveSpeed));
        playerRB.AddForce(transform.forward * (joystickResult.y * moveSpeed));
    }

    private void JumpPlayer() {
        bool isGrounded = Physics.CheckSphere(groundCheck.transform.position, 0.2f);

        if (isGrounded) {
            playerRB.AddForce(transform.up * jumpForce);
        }
    }
}
