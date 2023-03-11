using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat : MonoBehaviour
{
    public float moveSpeed = 1;
    public float turnSpeed = 0.1f;
    public float jumpForce = 3000f;
    public float mouseX;
    public GameObject player;
    private Rigidbody rb;

    private bool isOnGround = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //rb.velocity = ((transform.right * horizontal) + (transform.forward * vertical)) * moveSpeed;

        if (Input.GetKey(KeyCode.W)){
            player.transform.position += player.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)){
            player.transform.position -= player.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)){
            player.transform.Rotate(0, -turnSpeed, 0);
        }
        if (Input.GetKey(KeyCode.D)){
            player.transform.Rotate(0, turnSpeed, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 dwn = transform.TransformDirection (Vector3.down);
            isOnGround = (Physics.Raycast(transform.position, dwn, 0.7f));
            if (isOnGround)
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
    }
}
    