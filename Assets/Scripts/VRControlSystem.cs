using UnityEngine;
using UnityEngine.XR;

public class VRControlSystem : MonoBehaviour {
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerModel;
    
    [SerializeField] private GameObject camera;

    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;

    private Vector2 joystickResult;
    
    private float joystickX;
    private float joystickY;
    
    void Start() {
        camera.transform.rotation = InputTracking.GetLocalRotation(XRNode.Head);
        playerModel.transform.position = camera.transform.position;
        playerModel.transform.rotation = Quaternion.Euler(0, camera.transform.rotation.y, 0);

        leftController.transform.position = InputTracking.GetLocalPosition(XRNode.LeftHand);
        rightController.transform.position = InputTracking.GetLocalPosition(XRNode.RightHand);
    }

    void ResetPosition() {
        Vector3 resetPos = new Vector3(0, 0, 0);
        Quaternion resetRot = Quaternion.Euler(0, 0, 0);

        foreach (Transform child in playerPrefab.transform) {
            child.position = resetPos;
            child.rotation = resetRot;
        }
        
        playerPrefab.transform.position = resetPos;
        playerPrefab.transform.rotation = resetRot;
    }
    

    void Update() {
        camera.transform.rotation = InputTracking.GetLocalRotation(XRNode.Head);
        playerModel.transform.position = camera.transform.position;
        playerModel.transform.rotation = Quaternion.Euler(0, camera.transform.rotation.y, 0);
        leftController.transform.position = InputTracking.GetLocalPosition(XRNode.LeftHand);
        rightController.transform.position = InputTracking.GetLocalPosition(XRNode.RightHand);
        leftController.transform.rotation = InputTracking.GetLocalRotation(XRNode.LeftHand);
        rightController.transform.rotation = InputTracking.GetLocalRotation(XRNode.RightHand);
        
        joystickResult.x = Input.GetAxis("Horizontal");
        joystickResult.y = Input.GetAxis("Vertical");
    }
}
