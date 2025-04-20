using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞뒤 움직임 속도
    public float rotateSpeed = 180f; // 플레이어 입력을 알려주는 컴포넌트 

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트

    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        //회전 실행
        Rotate();
        // 움직임 실행
        Move();
        
        playerAnimator.SetFloat("Move", playerInput.move);
    }


    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move()
    {
        Vector3 moveDistance = transform.forward * (playerInput.move * moveSpeed * Time.deltaTime);
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    //입력값에 다라 캐릭터를 좌우로 회전
    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(0, turn, 0)); 
    }
}