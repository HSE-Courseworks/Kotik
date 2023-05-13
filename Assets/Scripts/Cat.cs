using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float turnSpeed = 0.1f;
    public float modificator = 1f;
    public float jumpForce = 5f;
    public float jumpTime = 1.458f;
    public float mouseX;
    public GameObject player;
    public AnimationCurve jumpCurve;

    private Rigidbody rb;
	private Animator mAnimator;

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
        
        if (rb.velocity.y < -0.1f && !IsGrounded())
            mAnimator.SetTrigger("isFalling");

		// =========== Прыжок ============
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            StartCoroutine(Jump());
        }

        // ========== Ходьба ===========
        if (Input.GetKey(KeyCode.W) && (rb.constraints & RigidbodyConstraints.FreezePositionX) == 0){
            player.transform.position += player.transform.forward * moveSpeed * modificator * Time.deltaTime;
			mAnimator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.S) && (rb.constraints & RigidbodyConstraints.FreezePositionX) == 0){
            player.transform.position -= player.transform.forward * moveSpeed * modificator * Time.deltaTime;
			mAnimator.SetBool("isWalking", true);
        }
		else mAnimator.SetBool("isWalking", false);

		if (mAnimator.GetBool("isJumping"))
			mAnimator.SetBool("isWalking", false);

		// ========= Повороты ==========
        if (Input.GetKey(KeyCode.A) && (rb.constraints & RigidbodyConstraints.FreezeRotationY) == 0){
            player.transform.Rotate(0, -turnSpeed * modificator, 0);
            mAnimator.SetBool("isRotatingR", false);
            mAnimator.SetBool("isRotatingL", true);
        }
        else if (Input.GetKey(KeyCode.D) && (rb.constraints & RigidbodyConstraints.FreezeRotationY) == 0){
            mAnimator.SetBool("isRotatingL", false);
            mAnimator.SetBool("isRotatingR", true);
            player.transform.Rotate(0, turnSpeed * modificator, 0);
        }
        else
        {
            mAnimator.SetBool("isRotatingL", false);
            mAnimator.SetBool("isRotatingR", false);
        }
    }
    
    private IEnumerator Jump()
    {
        float jumpTimer = 0f;
        mAnimator.SetBool("isJumping", true);
        rb.constraints = RigidbodyConstraints.FreezePositionX | 
                         RigidbodyConstraints.FreezePositionZ | 
                         RigidbodyConstraints.FreezeRotation; // Замораживаем движение объекта по оси X и Z до прыжка

        float boost = 0f;
        // Анимация отталкивания
        while (jumpTimer < jumpTime)
        {
            Debug.Log(jumpTimer);
            if (Input.GetKey(KeyCode.Space)) 
                boost += 0.01f;
            float jumpPower = jumpCurve.Evaluate(jumpTimer / jumpTime); // Получаем значение кривой для изменения силы прыжка со временем
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse); // Накладываем силу отталкивания по оси Y на Rigidbody объекта
            jumpTimer += Time.deltaTime;
            yield return null;
        }

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        // После окончания анимации отталкивания, наносим силу для прыжка в определенном направлении
        rb.AddForce(transform.up * (jumpForce + boost), ForceMode.Impulse);
        boost = 0f;
        
        // Ожидаем приземления
        while (!IsGrounded())
        {
            var yVelocity = rb.velocity.y;
            if (yVelocity > -0.2f && yVelocity < 0.2f)
                mAnimator.SetTrigger("isOnTop");
            else if (yVelocity < -0.2f)
                mAnimator.SetTrigger("isFalling");
            yield return null;
        }

        // Размораживаем движение объекта по оси X и Z после приземления
        if (IsGrounded())
        {
            mAnimator.SetBool("isJumping", false);
        }
    }

    // Проверка, находится ли объект на земле
    private bool IsGrounded()
    {
        Vector3 dwn = transform.TransformDirection (Vector3.down);
        return Physics.Raycast(transform.position, dwn, 0.1f);
    }
}