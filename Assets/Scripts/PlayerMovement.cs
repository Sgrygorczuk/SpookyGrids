 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Comtrol
    [SerializeField] private float movementTime = 0.2f;
    [SerializeField] private float jumpTime = 0.5f;
    [SerializeField] private float walkHeight = 0.5f;
    [SerializeField] private float jumpHeight = 1.5f;
    private bool _isMoving;
    private Vector3 _origin, _jump, _destination;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    private int _spriteIndex = 0;
    
    private Collider _checkCollider;
    private bool _oneUnitHit;
    private bool _twoUnitHit;
    RaycastHit m_Hit;
    private float maxDistance = 1;

    // Start is called before the first frame update
    private void Start()
    {
        _checkCollider = GetComponent<Collider>();
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
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
            StartCoroutine(RotatePlayer(new Vector3(0,-90,0),  -1));
        }
        else if (Input.GetButtonDown("Right"))
        {
            StartCoroutine(RotatePlayer(new Vector3(0,90,0), 1));
        }
        else if (Input.GetButtonDown("Down"))
        {
            StartCoroutine(RotatePlayer(new Vector3(0,180,0),  2));  
        }
        else if (Input.GetButtonDown("Up") && !_oneUnitHit)
        {
            StartCoroutine(MovePlayer(movementTime, 1, walkHeight));
        }
        else if (Input.GetButtonDown("Jump") && !_twoUnitHit)
        {
            StartCoroutine(MovePlayer(jumpTime, 2, jumpHeight));
        }
    }

    private IEnumerator MovePlayer(float time, float space, float height)
    {
        _isMoving = true;

        float elapsedTime = 0;

        var tran = transform;
        _origin = tran.position;
        _destination = _origin + tran.forward * space;
        _jump = _origin + tran.forward * (space / 2);
        _jump.y = _origin.y + height;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(_origin, _jump, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;
        transform.position = _jump;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(_jump, _destination, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _destination;
        _isMoving = false;
            
        DetectObstacles();
    }

    private void DetectObstacles()
    {
        _oneUnitHit = Physics.BoxCast(_checkCollider.bounds.center + Vector3.up/2, transform.localScale/4, transform.forward, out m_Hit, transform.rotation, maxDistance);
        _twoUnitHit = Physics.BoxCast(_checkCollider.bounds.center + Vector3.up/2 + transform.forward, transform.localScale/4, transform.forward, out m_Hit, transform.rotation, maxDistance);
    }
    
    


    //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    private void OnDrawGizmos()
    {
        //Check if there has been a hit yet
        if (_oneUnitHit)
        {
            Gizmos.color = Color.red;
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position + Vector3.up/2, transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance+ Vector3.up/2, transform.localScale/4);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            Gizmos.color = Color.blue;
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position+ Vector3.up/2, transform.forward * maxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * maxDistance+ Vector3.up/2, transform.localScale/4);
        }
        
        //Check if there has been a hit yet
        if (_twoUnitHit)
        {
            Gizmos.color = Color.red;
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position + Vector3.up/2 + transform.forward * 1, transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance+ Vector3.up/2 + transform.forward * 1f, transform.localScale/4);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            Gizmos.color = Color.blue;
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position+ Vector3.up/2 + transform.forward * 1f, transform.forward * maxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * maxDistance+ Vector3.up/2 + transform.forward * 1, transform.localScale/4);
        }
    }
    private IEnumerator RotatePlayer(Vector3 direction, int indexIncrement)
    {
        RotationSprite(indexIncrement);
        
        _origin = transform.eulerAngles;
        _destination = _origin + direction;
        
        transform.eulerAngles = _destination;
        _spriteRenderer.transform.rotation = Quaternion.Euler(0,0,0);
        yield return null;
        _isMoving = false;
        
        DetectObstacles();
    }

    private void RotationSprite(int indexIncrement)
    {
        _spriteIndex += indexIncrement;
        if (_spriteIndex < 0)
        {
            _spriteIndex = sprites.Length - 1;
        }
        else if (_spriteIndex == sprites.Length)
        {
            _spriteIndex = 0;
        }
        else if (_spriteIndex > sprites.Length)
        {
            _spriteIndex = 1;
        }

        _spriteRenderer.sprite = sprites[_spriteIndex];

        _isMoving = true;
    }
    
}
