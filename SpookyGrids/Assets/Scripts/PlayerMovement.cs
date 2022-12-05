 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementTime = 0.2f; 
    [SerializeField] private float jumpTime = 1f; 
    [SerializeField] private float rotateTime = 0.5f; 
    private bool _isMoving;
    private Vector3 _origin, _destination;

    [SerializeField] private float jumpForce = 250f; 
    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerInputAction();
    }

    private void PlayerInputAction()
    {
        if (_isMoving) { return; }
        if (Input.GetButtonDown("Left"))
        {
            StartCoroutine(RotatePlayer(new Vector3(0,-90,0), rotateTime));
        }
        else if (Input.GetButtonDown("Right"))
        {
            StartCoroutine(RotatePlayer(new Vector3(0,90,0), rotateTime));
        }
        else if (Input.GetButtonDown("Down"))
        {
            StartCoroutine(RotatePlayer(new Vector3(0,180,0), rotateTime * 2));  
        }
        else if (Input.GetButtonDown("Up"))
        {
            StartCoroutine(MovePlayer());
        }
        else if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(JumpPlayer());
        }
    }

    private IEnumerator MovePlayer()
    {
        _isMoving = true;

        float elapsedTime = 0;

        _origin = transform.position;
        _destination = _origin + transform.forward;

        while (elapsedTime < movementTime)
        {
            _rigidbody.MovePosition(Vector3.Lerp(_origin, _destination, elapsedTime / movementTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _destination;
        _isMoving = false;
    }
    
    private IEnumerator JumpPlayer()
    {
        _isMoving = true;

        float elapsedTime = 0;
        
        _rigidbody.AddForce(new Vector3(0,jumpForce,0));
        
        _origin = transform.position;
        _destination = _origin + 2 * transform.forward;

        while (elapsedTime < jumpTime)
        {
            var movement = Vector3.Lerp(_origin, _destination, elapsedTime / jumpTime);
            movement.y = transform.position.y;
            _rigidbody.MovePosition(movement);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _destination;
        _isMoving = false;
    }
    
    private IEnumerator RotatePlayer(Vector3 direction, float time)
    {
        _isMoving = true;

        float elapsedTime = 0;
        
        _origin = transform.eulerAngles;
        _destination = _origin + direction;

        while (elapsedTime < time)
        {
            transform.eulerAngles = Vector3.Lerp(_origin, _destination, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = _destination;
        _isMoving = false;
    }
    
}
