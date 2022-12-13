using System.Collections;
using UnityEngine;

public class CameraSwitching : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //Game Object that holds the Camera and Camera Trigger Lists 
    [SerializeField] private Camera[] cameras;
    private Vector3 _exitPosition;
    private Animator _animator;
    private PlayerMovement _playerMovement;
    
    //==================================================================================================================
    // Methods  
    //==================================================================================================================
    
    //Hooks up the list that holds all the cameras 
    private void Start()
    {
        _exitPosition = transform.GetChild(0).transform.position;
        _animator = GameObject.Find("TransitionCanvas").transform.GetChild(0).GetComponent<Animator>();
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    //Listens for the player to enter a new camera trigger 
    private void OnTriggerEnter(Collider other)
    {
        _playerMovement.PlayerMoving();
        StartCoroutine(MovePlayer(other));
    }

    private IEnumerator MovePlayer(Component other)
    {
        _animator.Play("InAndOut");
        yield return new WaitForSeconds(0.66f);
        cameras[1].enabled = true;
        cameras[0].enabled = false;
        other.transform.position = _exitPosition;
        yield return new WaitForSeconds(0.5f);
        _playerMovement.PlayerNoLongerMoving();
    }
}
