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

    // devices
    public InputDevice leftController;
    public InputDevice rightController;
    public InputDevice playerHeadset;
    
    public KeyCode jumpButton = KeyCode.JoystickButton0;

    // movement
    private Vector2 joystickResult;
    
    private float joystickX;
    private float joystickY;
    
    // forces
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    // Initialise all player devices
    private void InitialiseInputDevices() {
        if (!leftController.isValid) {
            InitialiseInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref leftController);
        }
        if (!rightController.isValid) {
            InitialiseInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref rightController);
        }
        if (!playerHeadset.isValid) {
            InitialiseInputDevice(InputDeviceCharacteristics.HeadMounted, ref playerHeadset);
        }
    }

    // Initialise device as it's loaded
    private void InitialiseInputDevice(InputDeviceCharacteristics characteristics, ref InputDevice inputDevice) {
        List<InputDevice> devices = new List<InputDevice>();
        
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);

        if (devices.Count == 0) {
            inputDevice = devices[0];
        }
    }
    
    private void Start() {
        // Initialise controllers and HMD
        InitialiseInputDevices();
        
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
        // player body/look
        if (playerHeadset.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion headsetR))
        {
            camera.transform.rotation = headsetR;
        }
        
        camera.transform.position = playerModel.transform.position;
        playerModel.transform.rotation = Quaternion.Euler(0, camera.transform.rotation.y, 0);
        
        // controllers
        if (leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 leftCP)) {
            leftControllerObject.transform.position = leftCP;
        }

        if (rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 rightCP)) {
            rightControllerObject.transform.position = rightCP;
        }

        if (leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion leftCR)) {
            leftControllerObject.transform.rotation = leftCR;
        }
        
        if (leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rightCR)) {
            rightControllerObject.transform.rotation = rightCR;
        }
    }

    private void FixedUpdate() {
        // movement
        joystickResult.x = Input.GetAxis("Horizontal");
        joystickResult.y = Input.GetAxis("Vertical");
        
        if (joystickResult.x != 0 || joystickResult.y != 0) {MovePlayer(joystickResult);}
        if (Input.GetKeyDown(jumpButton)) {JumpPlayer();}
    }

    private void MovePlayer(Vector2 moveVector) {
        Vector3 forceVector = new Vector3(moveVector.x, 0, moveVector.y);
        
        playerRB.AddForce(forceVector * moveSpeed);
    }

    private void JumpPlayer() {
        playerRB.AddForce(transform.up * jumpForce);
    }
}
