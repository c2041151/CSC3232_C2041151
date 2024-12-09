using System.Collections;
using System.Data;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    [Header("Movement")]
    public float moveSpeed = 15;
    
    public Animator animator;
    public Transform orientation;

    [Header("Aiming And Shooting")]
    public CinemachineFreeLook cam;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private ParticleSystem muzzleFlash;

    [Header("Audio")]
    [SerializeField] private AudioSource reloadSound;
    [SerializeField] private AudioSource gunSounds;
    private float reloading;
    float ammoInGun;
    float totalAmmo;

    [Header("UI Elements")]
    [SerializeField] GameObject crosshair;
    [SerializeField] Canvas victoryScreen;
    [SerializeField] Canvas deathScreen;
    Slider healthBar;
    [SerializeField] Gradient healthBarGradient;
    [SerializeField] Image healthBarFill;
    [SerializeField] GameObject finishPrompt;
    [SerializeField] TMP_Text ammoInGunText;
    [SerializeField] TMP_Text totalAmmoText;
    
    bool isAiming;
    float horizontalInput;
    float verticalInput;

    bool inVictoryArea;
    Vector3 moveDirection;
    Vector2 screenCentre = new Vector2(Screen.width/2,Screen.height/2);
    
    Rigidbody rb;
    private Camera mainCam;
    private float damageDealt = 20f;

    void Start(){
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        mainCam = GetComponentInChildren<Camera>();
        healthBar = GetComponentInChildren<Slider>();

        //sets timescale to 1 in case of restart on death - otherwise the game is effectively unplayable after death
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;

        ammoInGun = 20;
        totalAmmo = 80;
        reloading = 0;
        healthBar.maxValue = health;
        healthBar.value = health;
        healthBarFill.color = healthBarGradient.Evaluate(1f);
        totalAmmoText.text = totalAmmo.ToString();
        ammoInGunText.text = ammoInGun.ToString();

        inVictoryArea = false;
        victoryScreen.enabled = false;
        deathScreen.enabled = false;
        crosshair.SetActive(false);
        finishPrompt.SetActive(false);
    }

    void Update(){
        InputHandler();
        Aiming();
        if(Input.GetKeyDown(KeyCode.Mouse0) && ammoInGun >0 && Time.time > reloading){
            Shooting();
        }
        if(Input.GetKeyDown(KeyCode.R) && ammoInGun <20 && totalAmmo >0){
            if(totalAmmo+ammoInGun >= 20){
                totalAmmo = totalAmmo+ammoInGun;
                ammoInGun = 20;
                totalAmmo = totalAmmo-20;
                reloading = Time.time + 2;
            }
            else{
                totalAmmo = totalAmmo + ammoInGun;
                ammoInGun = totalAmmo;
                totalAmmo = 0;
                Debug.Log("no more ammo");
                reloading = Time.time + 2;
            }
            totalAmmoText.text = totalAmmo.ToString();
            ammoInGunText.text = ammoInGun.ToString();
            reloadSound.Play();
        }
        //if the player is in the victory area and interacts, start the victory coroutine
        if(inVictoryArea == true){
            Debug.Log("In Victory Area");
            if(Input.GetKeyDown(KeyCode.E)){
                StartCoroutine(Victory(5));
            }
        }
    }

    void FixedUpdate(){
        MovePlayer();
    }

    private void InputHandler(){
        //grab player input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer(){
        //move direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //change animation speed based on user input, reset to 0 otherwise
        if(isAiming == true){
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){
                animator.SetFloat("Speed", 1);
            }
            else{
                animator.SetFloat("Speed", 0);
            }
            animator.SetBool("IsAiming", true);
        }
        else if(isAiming == false){
            animator.SetBool("IsAiming", false);
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && Input.GetKey(KeyCode.LeftShift)){
                animator.SetFloat("Speed", 5);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift)){
                animator.SetFloat("Speed", 2.5f);
            }
            else{
                animator.SetFloat("Speed", 0);
            }
        }


        //change sprint speed on keypress 
        if(Input.GetKey(KeyCode.LeftShift) && isAiming == false){
            moveSpeed = 30;
        }
        else if (isAiming == true){
            moveSpeed = 7.5f;
        }
        else{
            moveSpeed = 15;
        }

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void Aiming(){
        //pause check
        if(Time.timeScale == 1){
            if(Input.GetKey(KeyCode.Mouse1)){
                cam.m_Lens.FieldOfView = 40;
                isAiming = true;
                crosshair.SetActive(true);
            }
            if(Input.GetKeyUp(KeyCode.Mouse1)){
                cam.m_Lens.FieldOfView = 60;
                isAiming = false;
                crosshair.SetActive(false);
            }
        }
    }

    private void Shooting(){
        //pause check
        if(Time.timeScale == 1){
            //fire ray and gather information, damage if hit enemy
            Ray ray = mainCam.ScreenPointToRay(screenCentre);
            muzzleFlash.Play();
            gunSounds.Play();
            ammoInGun--;
            ammoInGunText.text = ammoInGun.ToString();
            Debug.Log(ammoInGun);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask)){
                debugTransform.position = hit.point;
                if(hit.transform.gameObject.tag=="Enemy"){
                    hit.transform.GetComponent<EnemyManager>().takeDamage(damageDealt);
                }
            }
        }
    }
    public void takeDamage(float damage){
        health -= damage;
        healthBar.value = health;
        healthBarFill.color = healthBarGradient.Evaluate(healthBar.normalizedValue);

        if (health <= 0){
            StartCoroutine(OnDeath(5));
        }
    }
    IEnumerator Victory(float seconds){
        Time.timeScale = 0;
        victoryScreen.enabled = true;
        finishPrompt.SetActive(false);
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator OnDeath(float seconds){
        Time.timeScale = 0;
        deathScreen.enabled = true;
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "VictoryArea"){
            inVictoryArea = true;
            finishPrompt.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other){
        if (other.tag == "VictoryArea"){
            inVictoryArea = false;
            finishPrompt.SetActive(false);
        }
    }
}
