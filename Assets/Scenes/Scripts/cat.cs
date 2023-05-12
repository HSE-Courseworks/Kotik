using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat : MonoBehaviour
{
    public float moveSpeed = 1;
    public float turnSpeed = 0.1f;
<<<<<<< Updated upstream:Assets/Scenes/Scripts/cat.cs
    public float jumpForce = 3000f;
=======
    public float modificator = 1f;
    public float jumpForce = 30f;
>>>>>>> Stashed changes:Assets/Scripts/Cat.cs
    public float mouseX;
    public GameObject player;

    private Rigidbody rb;
	private Animator mAnimator;
    private bool isOnGround = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }
    
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //rb.velocity = ((transform.right * horizontal) + (transform.forward * vertical)) * moveSpeed;

<<<<<<< Updated upstream:Assets/Scenes/Scripts/cat.cs
        if (Input.GetKey(KeyCode.W)){
            player.transform.position += player.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)){
            player.transform.position -= player.transform.forward * moveSpeed * Time.deltaTime;
=======
		// ============ Бежать + красться ============
        /*if (Input.GetKey(KeyCode.LeftShift))
        {
            modificator = 2f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            modificator = 0.5f;
        }
        else
        {
            modificator = 1f;
        }*/
        

		// =========== Прыжок ============
        Vector3 dwn = transform.TransformDirection (Vector3.down);
		isOnGround = (Physics.Raycast(transform.position, dwn, 0.1f));
		if (isOnGround)
			mAnimator.SetBool("isJumping", false);
		else 
			mAnimator.SetBool("isJumping", true);
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
			mAnimator.SetBool("isJumping", true);
			mAnimator.SetBool("isWalking", false);
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }

		// ========== Ходьба ===========
        if (Input.GetKey(KeyCode.W)){
            player.transform.position += player.transform.forward * moveSpeed * modificator * Time.deltaTime;
			mAnimator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.S)){
            player.transform.position -= player.transform.forward * moveSpeed * modificator * Time.deltaTime;
			mAnimator.SetBool("isWalking", true);
>>>>>>> Stashed changes:Assets/Scripts/Cat.cs
        }
		else mAnimator.SetBool("isWalking", false);

		if (mAnimator.GetBool("isJumping"))
			mAnimator.SetBool("isWalking", false);

		// ========= Повороты ==========
        if (Input.GetKey(KeyCode.A)){
            player.transform.Rotate(0, -turnSpeed, 0);
        }
        if (Input.GetKey(KeyCode.D)){
            player.transform.Rotate(0, turnSpeed, 0);
        }
    }
}
    