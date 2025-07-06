using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    //속도 변수
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    //상태변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = false;
    private bool isCrouch = false;

    //움직임체크 변수
    private Vector3 lastPos;

    //얼마나 앉을지 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    //땅착지여부 파악
    private CapsuleCollider capsuleCollider;

    //민감도
    [SerializeField]
    private float lookSensitivity;

    //카메라 변수
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    //필요한 컴포넌트들
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController;
    private Crosshair theCrosshair;
    private StatusController theStatusController;

    //달리기&겉기 애니메이션을 위해 불러옴
    [SerializeField]
    private Animator currentHand;

    private float lastCheckTime = 0f;
    private float checkInterval = 0.1f; // 0.1초마다 체크

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        theGunController = FindFirstObjectByType<GunController>();
        theCrosshair = FindFirstObjectByType<Crosshair>();
        theStatusController = FindFirstObjectByType<StatusController>();

        //변수 초기화 영역
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
        
    }

    void Update()
    {
        IsWalk();
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        if (Time.time - lastCheckTime >= checkInterval)
        {
            MoveCheck();
            lastCheckTime = Time.time;
        }
        CameraRotation();
        CharacterRotation();
    }
    void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
            {
                isWalk = true;
                
            }
            else
            {
                isWalk = false;
            }
            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;   
        }
            
        
    }
    void IsWalk()
    {
        if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f 
            || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f) && isGround)
        {
            currentHand.SetBool("Walk", true);
        }
        else
        {
            currentHand.SetBool("Walk", false);
        }
    }

    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        //수동으로 카메라 localPosition 변경
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
        //코루틴 함수를 이용한 부드러운 카메라 이동
        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.1f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if(count > 15)
            {
                break;
            }
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);

    }

    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        theCrosshair.JumpingAnimation(!isGround);
    }

    void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSp() > 0)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (isCrouch)
            Crouch();

        theStatusController.DecreaseStamina(100);
        myRigid.linearVelocity = transform.up * jumpForce;
    }

    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSp() > 0)
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSp() <= 0)
        {
            RunningCancel();
        }
    }

    void Running()
    {
        if (isCrouch)
            Crouch();

        theGunController.CancleFinesight(); 

        isRun = true;
        applySpeed = runSpeed;
        currentHand.SetBool("Run", true);
        theCrosshair.RunningAnimation(isRun);
        theStatusController.DecreaseStamina(10);
    }

    void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
        currentHand.SetBool("Run", false);
        theCrosshair.RunningAnimation(isRun);
    }
    void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }
    void CameraRotation()
    {
        //카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    void CharacterRotation()
    {
        //캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
