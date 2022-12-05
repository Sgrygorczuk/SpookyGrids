/*
 * Camera Switching is Used by the player to detect if they walked into a new trigger to change which camera is
 * currently in use. It disables all the other cameras in the scene and only keeps the one the player just encountered.
 *
 *  === Make sure the triggers are at least 1 Unit away from each other so that the player doesn't keep re-triggering
 *  the camera they walked away from ===
 */
using UnityEngine;

public class CameraSwitching : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //Game Object that holds the Camera and Camera Trigger Lists 
    private GameObject _cameras;
    
    //==================================================================================================================
    // Methods  
    //==================================================================================================================
    
    //Hooks up the list that holds all the cameras 
    private void Start()
    {
        _cameras = GameObject.Find("Camera_Controls").transform.Find("Cameras").gameObject;
    }

    //Listens for the player to enter a new camera trigger 
    private void OnTriggerEnter(Collider other)
    {
        SwitchCamera(other.gameObject.transform.GetSiblingIndex());
    }

    //Takes the Sibling Index of the given trigger, turns off all the other cameras in the list and turns on the one
    //That the trigger corresponds to. 
    private void SwitchCamera(int index)
    {
        for (var i = 0; i < _cameras.transform.childCount; i++)
        {
            _cameras.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        _cameras.transform.GetChild(index).gameObject.SetActive(true);
    }
}
