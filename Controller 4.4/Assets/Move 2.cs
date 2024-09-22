using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller1 : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public float moveSpeed = 5f; // ความเร็วในการเดิน
    public float runSpeed = 8f;  // ความเร็วในการวิ่ง
    public float jumpForce = 5f; // ความสูงในการกระโดด
    private bool isGrounded; // สถานะการอยู่บนพื้น

    void Update()
    {
        // ตรวจสอบว่าตัวละครอยู่บนพื้น
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f); // ปรับระยะตามขนาดตัวละคร

        // รับค่า Input จาก W, A, S, D
        float h = Input.GetAxis("Horizontal"); // ค่าการเคลื่อนที่แกน X (A, D)
        float v = Input.GetAxis("Vertical");   // ค่าการเคลื่อนที่แกน Z (W, S)

        // ตรวจสอบว่ากด Shift หรือไม่
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        // สร้างเวกเตอร์การเคลื่อนที่
        Vector3 moveDirection = new Vector3(h, 0, v).normalized;

        // ถ้ามีการเคลื่อนที่
        if (moveDirection.magnitude > 0.1f)
        {
            // หันหน้าไปทางที่เคลื่อนที่
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // หมุนตัวอย่างนุ่มนวล
            
            // เริ่มการเดินหรือวิ่ง
            if (Input.GetKey(KeyCode.LeftShift)) // ถ้ากด Shift ให้วิ่ง
            {
                playerAnim.SetTrigger("Running");
                playerAnim.ResetTrigger("walk");
            }
            else // ถ้าไม่กด Shift ให้เดิน
            {
                playerAnim.SetTrigger("walk");
                playerAnim.ResetTrigger("Running");
            }
            playerAnim.ResetTrigger("Idle");

            // เคลื่อนที่ตัวละคร
            playerRigid.velocity = new Vector3(moveDirection.x * currentSpeed, playerRigid.velocity.y, moveDirection.z * currentSpeed);
        }
        else
        {
            // ถ้าหยุดเดินหรือวิ่ง
            playerAnim.ResetTrigger("walk");
            playerAnim.ResetTrigger("Running");
            playerAnim.SetTrigger("Idle");
            playerRigid.velocity = new Vector3(0, playerRigid.velocity.y, 0); // หยุดการเคลื่อนที่เมื่อไม่มีการกดปุ่ม
        }

        // การกระโดด
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // เช็คว่าตัวละครอยู่บนพื้น
        {
            playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetTrigger("MutantJumping"); // เรียกอนิเมชันกระโดด
        }
    }
}
