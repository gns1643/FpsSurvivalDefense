using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    //크로스헤어의 상태에 따른 총의 정확도 
    private float gunAccuracy;

    //크로스헤어 비활성화를 위한 부모 객체
    [SerializeField]
    private GameObject go_CrosshairHUD;
    [SerializeField]
    private GunController theGunController;

    public void WalkingAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            WeaponManager.currentWeaponAnim.SetBool("Walk", _flag);
            animator.SetBool("Walking", _flag);
        }
    }
    public void RunningAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            WeaponManager.currentWeaponAnim.SetBool("Run", _flag);
            animator.SetBool("Running", _flag);
        }
            
    }
    public void JumpingAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {

        }
        
            animator.SetBool("Running", _flag);
    }
    public void CrouchingAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            animator.SetBool("Crouching", _flag);
        }
    }
    public void FineSightAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            animator.SetBool("Finesight", _flag);
        }
    }
    public void FireAnimation()
    {
        if (!GameManager.isWater)
        {
            if (animator.GetBool("Walking"))
            {
                animator.SetTrigger("Walk_Fire");
            }
            else if (animator.GetBool("Crouching"))
            {
                animator.SetTrigger("Crouch_Fire");
            }
            else
            {
                animator.SetTrigger("Idle_Fire");
            }
        }
    }

    public float GetAccuracy()
    {
        if (animator.GetBool("Walking"))
        {
            gunAccuracy = 0.08f;
        }
        else if (animator.GetBool("Crouching"))
        {
            gunAccuracy = 0.02f;
        }
        else if (theGunController.GetFineSightMode())
        {
            gunAccuracy = 0.001f;
        }
        else
        {
            gunAccuracy = 0.04f;
        }
        return gunAccuracy;
    }
}
