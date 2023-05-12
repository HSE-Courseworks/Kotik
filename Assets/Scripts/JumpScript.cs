using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    public GameObject player;

    //[SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private LayerMask jumpableLayerMask;
    [SerializeField] private LayerMask floorMask;

    public bool isJumping = false;
    public Vector3? jumpTarget = null;
    private Vector3 jumpStartPos = Vector3.zero;
    public AnimationCurve jumpCurve;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!isJumping && IsGrounded() && Input.GetKeyDown(KeyCode.E) && !Physics.Raycast(transform.position, Vector3.up, 5f))
        {
            Debug.Log("Update.Check passed");
            
            Vector3[] raycastOrigins = new Vector3[3]
            {
                transform.position + transform.forward * 0.5f,
                transform.position + transform.forward * 0.5f + transform.up * 0.5f,
                transform.position + transform.forward * 0.5f + transform.up * -0.5f
            };
            
            foreach (Vector3 origin in raycastOrigins)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(origin, transform.forward, out hitInfo, 5f, jumpableLayerMask))
                {
                    Debug.Log("Update.Raycast(forward) passed");
                    Vector3 targetPos = hitInfo.point + hitInfo.normal * col.bounds.extents.y;
                    if (hitInfo.collider != null)
                    {
                        Debug.Log("Update.isJumping = True");
                        isJumping = true;
                        jumpTarget = targetPos;
                        jumpStartPos = transform.position;
                        break;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            Debug.Log("FixedUpdete.isJumping passed");
            float jumpProgress = (transform.position - jumpStartPos).magnitude / (jumpTarget.Value - jumpStartPos).magnitude;
            float jumpHeight = jumpCurve.Evaluate(jumpProgress);
            Vector3 targetPos = new Vector3(jumpTarget.Value.x, jumpTarget.Value.y + jumpHeight, jumpTarget.Value.z);

            Vector3 jumpDir = (targetPos - transform.position).normalized;
            float jumpDistance = (targetPos - transform.position).magnitude;
            
            Debug.Log("FixedUpdete.jumpDistanse = " + jumpDistance);
            if (jumpDistance < 0.05f)
            {
                isJumping = false;
                jumpTarget = null;
            }
            else
            {
                rb.velocity = jumpDir * jumpSpeed;
            }
        }
        /*else
        {
            float horizontalDirection = Input.GetAxis("Horizontal");
            float verticalDirection = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalDirection, 0f, verticalDirection).normalized;

            rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
        }*/
    }
    
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.125f,
            jumpableLayerMask + floorMask);
        //return Physics.Raycast(transform.position, transform.down, col.bounds.extents.y + 0.1f, jumpableLayerMask);
    }
    
    
    
    
    // public KeyCode jumpKey = KeyCode.E;
    // public float detectionRadius;
    // public float maxJumpHeight;
    // public LayerMask jumpableLayers;
    // public LayerMask notPlayerMask;
    //
    // public bool canJump;
    // public GameObject player;
    // private Rigidbody rb;
    // private Collider col;
    // public bool isOnGround = true;
    // public Collider toJump;
    // public Collider currentObject = null;
    //
    // [SerializeField] private AnimationCurve _yAnimation;
    // private float _expiredTime;
    // private float _duration = 1f;
    // private float height;
    //
    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    //     col = GetComponent<Collider>();
    // }
    //
    // void Update()
    // {
    //     if (canJump && (Input.GetKeyDown(jumpKey) || !isOnGround))
    //     {
    //         Jump();
    //     }
    // }
    //
    // void FixedUpdate()
    // {
    //     toJump = CheckJumpable();
    // }
    //
    // Collider CheckJumpable()
    // {
    //     Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, jumpableLayers);
    //
    //     Vector3 dwn = transform.TransformDirection(Vector3.down);
    //     isOnGround = (Physics.Raycast(transform.position, dwn, 0.125f));
    //     
    //     if (!isOnGround) return null;
    //
    //     RaycastHit hitDown;
    //     if (Physics.Raycast(transform.position, Vector3.down, out hitDown))
    //     {
    //         if (hitDown.collider != null)
    //         {
    //             currentObject = hitDown.collider;
    //         }
    //         else
    //         {
    //             currentObject = null;
    //         }
    //     }
    //
    //     canJump = false;
    //     
    //     RaycastHit hitUp;
    //     if (Physics.Raycast(transform.position, Vector3.up, out hitUp))
    //     {
    //         if (hitUp.collider != null)
    //         {
    //             return null;
    //         }
    //     }
    //     
    //     /*RaycastHit hitForward;
    //     if (Physics.Raycast(transform.position, Vector3.forward, out hit))
    //     {
    //         if (hitForward.collider != null)
    //         {
    //             canJump = true;
    //             return hitForward.collider;
    //         }
    //         else
    //         {
    //             return null;
    //         }
    //     }*/
    //     
    //     foreach (Collider collider in colliders)
    //     {
    //         if (collider != col 
    //             && collider != currentObject 
    //             && (collider.transform.position.y - transform.position.y) < maxJumpHeight)
    //         {
    //             canJump = true;
    //             return collider;
    //         }
    //     }
    //     
    //     return null;
    // }
    //
    // void Jump()
    // {
    //     if (toJump == null)
    //         return;
    //     
    //     float height = (toJump.transform.localScale.y - transform.position.y) + 0.2f;
    //     
    //     // Отключаем возможность прыгать до завершения текущего прыжка
    //     isOnGround = false;
    //     
    //     // Вычисляем позицию на вершине объекта, на который нужно запрыгнуть
    //     Vector3 jumpPosition = toJump.transform.position + new Vector3(0f, toJump.transform.localScale.y, 0f);
    //     
    //     // Вычисляем направление прыжка
    //     Vector3 jumpDirection = (jumpPosition - transform.position).normalized;
    //     
    //     // Выполняем прыжок с помощью силы
    //     rb.AddForce(jumpDirection, ForceMode.Impulse);
    //
    //     _expiredTime += Time.deltaTime;
    //     if (_expiredTime > _duration)
    //         _expiredTime = 0f;
    //     float progress = _expiredTime / _duration;
    //     //transform.position = new Vector3(jumpDirection.x, _yAnimation.Evaluate(progress) * height, jumpDirection.y);
    //
    //     //rb.AddForce(Vector3.up * jumpForce * 3, ForceMode.Impulse);
    // }
}
