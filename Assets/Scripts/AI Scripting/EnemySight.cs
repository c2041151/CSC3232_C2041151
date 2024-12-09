using System.Collections;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    AiAgent parentScript;           //the enemy controller parent script

    //Player/Enemy Refs
    public GameObject bodyRef;              //The reference of the enemys "body" 
    GameObject playerRef;

    //Audio Contoller
    AudioSource alertSound;                 //The sound played when enemies are alerted
    
    //Line of sight variables
    GameObject lineTarget;                  //target collider for enemy sight
    public GameObject lineOrigin;           //origin of enemy sight linecast
    bool playerInCone;                      //Wether or not player is in the view cone game object


    void Start(){
        //Initialising enemy defaults
        parentScript = gameObject.GetComponentInParent<AiAgent>();
        alertSound = gameObject.GetComponent<AudioSource>();
        playerRef = GameObject.Find("Test Player");
        lineTarget = playerRef.transform.GetChild(0).gameObject;
        lineOrigin = parentScript.transform.GetChild(0).gameObject;
        playerInCone = false;
    }

    //changes playerInCone to true when the player enters a given view cone
    private void OnTriggerEnter(Collider other){
        if (other.tag=="Player"){
            Debug.Log("Player in view cone");
            playerInCone = true; 
        }
    }

    //changes playerInCone to false when player leaves the vew cone
    private void OnTriggerExit(Collider other){
        if (other.tag == "Player"){
            playerInCone = false;
        }
    }
    void Update(){
        Debug.DrawLine(lineOrigin.transform.position, lineTarget.transform.position); //test line between player and enemy view point - also shows the physics linecast
        
        //if player is in the enemy view cone - check to see if there is a line of sight between the enemys "eyes" and the player
        if (playerInCone){
            RaycastHit hit; //hit provides information about what the raycast comes into contact with
            if (Physics.Linecast(lineOrigin.transform.position, lineTarget.transform.position, out hit)){
                Debug.Log(hit.collider);
                StartCoroutine(realiseTimer());
                //alertSound.Play();
            }
        }
    }

    IEnumerator realiseTimer(){
        yield return new WaitForSecondsRealtime(1);
        parentScript.playerInSights = true;
    }
}