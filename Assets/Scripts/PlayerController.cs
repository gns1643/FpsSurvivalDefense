using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    //�ӵ� ����
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    //���º���
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = false;
    private bool isCrouch = false;

    //������üũ ����
    private Vector3 lastPos;

    //�󸶳� ������ ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    //���������� �ľ�
    private CapsuleCollider capsuleCollider;

    //�ΰ���
    [SerializeField]
    private float lookSensitivity;

    //ī�޶� ����
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    //�ʿ��� ������Ʈ��
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController;
    private Crosshair theCrosshair;
    private StatusController theStatusController;

    //�޸���&�ѱ� �ִϸ��̼��� ���� �ҷ���
    [SerializeField]
    private Animator currentHand;

    private float lastCheckTime = 0f;
    private float checkInterval = 0.1f; // 0.1�ʸ��� üũ

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        theGunController = FindFirstObjectByType<GunController>();
        theCrosshair = FindFirstObjectByType<Crosshair>();
        theStatusController = FindFirstObjectByType<StatusController>();

        //���� �ʱ�ȭ ����
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
        //�������� ī�޶� localPosition ����
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
        //�ڷ�ƾ �Լ��� �̿��� �ε巯�� ī�޶� �̵�
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
        //ī�޶� ȸ��
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    void CharacterRotation()
    {
        //ĳ���� ȸ��
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
