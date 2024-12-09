using Cinemachine;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("references")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public GameObject combatLookAtRight;
    public GameObject combatLookAtLeft;
    public CinemachineFreeLook cam;

    public float rotationSpeed;

    void Start(){
        combatLookAtLeft.SetActive(false);
    }
    
    void Update()
    {
        //rotation orientation
        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        //rotate player object
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = orientation.forward * verticalInput +orientation.right *horizontalInput;

        if(inputDirection != Vector3.zero){
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
        SwapShoulders();
    }

    /// <summary>
    /// swaps shoulder view on left alt
    /// </summary>
    private void SwapShoulders(){
        if(Input.GetKeyDown(KeyCode.LeftAlt) && combatLookAtRight.activeInHierarchy){
            combatLookAtLeft.SetActive(true);
            cam.LookAt = combatLookAtLeft.transform;
            combatLookAtRight.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.LeftAlt) && combatLookAtLeft.activeInHierarchy){
            combatLookAtRight.SetActive(true);
            cam.LookAt = combatLookAtRight.transform;
            combatLookAtLeft.SetActive(false);
        }
    }
}
