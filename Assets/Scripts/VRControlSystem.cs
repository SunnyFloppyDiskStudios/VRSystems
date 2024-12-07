using UnityEngine;
using UnityEngine.XR;

public class VRControlSystem : MonoBehaviour {
    // player
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private Rigidbody playerRB;
    
    [SerializeField] private GameObject camera;
    
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;

    // movement
    private Vector2 joystickResult;
    
    private float joystickX;
    private float joystickY;
    
    public KeyCode jumpButton = KeyCode.JoystickButton0;

    // forces
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    
    private void Start() {
        // initialise rotations and positions
        camera.transform.rotation = InputTracking.GetLocalRotation(XRNode.Head);
        camera.transform.position = playerModel.transform.position;
        playerModel.transform.rotation = Quaternion.Euler(0, camera.transform.rotation.y, 0);

        leftController.transform.position = InputTracking.GetLocalPosition(XRNode.LeftHand);
        rightController.transform.position = InputTracking.GetLocalPosition(XRNode.RightHand);
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
        camera.transform.rotation = InputTracking.GetLocalRotation(XRNode.Head);
        playerModel.transform.position = camera.transform.position;
        playerModel.transform.rotation = Quaternion.Euler(0, camera.transform.rotation.y, 0);
        
        // controllers
        leftController.transform.position = InputTracking.GetLocalPosition(XRNode.LeftHand);
        rightController.transform.position = InputTracking.GetLocalPosition(XRNode.RightHand);
        leftController.transform.rotation = InputTracking.GetLocalRotation(XRNode.LeftHand);
        rightController.transform.rotation = InputTracking.GetLocalRotation(XRNode.RightHand);
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
