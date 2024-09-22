using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public float w_speed, wb_speed, olw_speed, rn_speed, ro_speed;
    public float jumpForce = 5f; // ความสูงในการกระโดด
    public bool walking;
    public Transform playerTrans;
    private bool isJumping = false; // สถานะการกระโดด

    void Start()
    {
        // บันทึกค่าความเร็วเริ่มต้นไว้
        olw_speed = w_speed;
    }

    
	void FixedUpdate(){
		if(Input.GetKey(KeyCode.W)){
			playerRigid.velocity = transform.forward * w_speed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.S)){
			playerRigid.velocity = -transform.forward * wb_speed * Time.deltaTime;
		}
	}
    void Update()
    {
        // เริ่มเดินไปข้างหน้า
        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            playerAnim.SetTrigger("walk");
            playerAnim.ResetTrigger("Idle");
            walking = true;
        }
        // หยุดเดิน
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.ResetTrigger("walk");
            playerAnim.SetTrigger("Idle");
            walking = false;
        }
        // เริ่มเดินถอยหลัง
        if (Input.GetKeyDown(KeyCode.S) && !isJumping)
        {
            playerAnim.SetTrigger("WalkingBackwards");
            playerAnim.ResetTrigger("Idle");
        }
        // หยุดเดินถอยหลัง
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.ResetTrigger("WalkingBackwards");
            playerAnim.SetTrigger("Idle");
        }

        // หมุนซ้าย (A)
        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Rotate(0, -ro_speed * Time.deltaTime, 0);
        }
        // หมุนขวา (D)
        if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Rotate(0, ro_speed * Time.deltaTime, 0);
        }


        // การวิ่ง (Shift + W)
        if (walking)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                w_speed += rn_speed;
                playerAnim.SetTrigger("Running");
                playerAnim.ResetTrigger("walk");
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                w_speed = olw_speed;
                playerAnim.ResetTrigger("Running");
                playerAnim.SetTrigger("walk");
            }
        }

        // การกระโดด
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) // เช็คว่าตัวละครยังไม่กระโดด
        {
            playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetTrigger("MutantJumping"); // เพิ่มการเรียกอนิเมชันกระโดด
            isJumping = true; // ตั้งค่าสถานะการกระโดด
        }

        // รีเซ็ตสถานะการกระโดดเมื่ออยู่บนพื้น
        if (Mathf.Abs(playerRigid.velocity.y) < 0.01f)
        {
            isJumping = false; // รีเซ็ตสถานะเมื่อกลับมาที่พื้น
        }
    }
}
