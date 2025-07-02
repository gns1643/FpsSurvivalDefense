using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 활성화 여부
    public static bool isActivate = true;
    //현재 총
    [SerializeField]
    private Gun currentGun;

    //연사 속도
    private float currentFireRate;

    //효과음
    private AudioSource audioSource;

    //상태 변수
    private bool isReload = false;

    [HideInInspector]
    public bool isFineSightMode = false;

    //원래 포지션
    private Vector3 originPos;

    //충돌 정보
    private RaycastHit hitInfo;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    //피격 이펙트
    [SerializeField]
    private GameObject hitEffectPrefab;

    private void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrosshair = FindFirstObjectByType<Crosshair>();

        WeaponManager.currentWeapon = currentGun.transform;
        WeaponManager.currentWeaponAnim = currentGun.anim;
    }
    void Update()
    {
        if (isActivate)
        {
            GunFireRateCal();
            TryFire();
            TryReload();
            TryFineSight();
        }
            
    }

    void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2") && !isReload)
        {
            
            FineSight();
        }
    }

    public void CancleFinesight()
    {
        if (isFineSightMode)
        {

            FineSight();
        }
    }

    void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMod", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActiveCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactiveCoroutine());
        }
    }

    IEnumerator FineSightActiveCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineInsightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition,
                currentGun.fineInsightOriginPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator FineSightDeactiveCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition,
                originPos, 0.2f);
            yield return null;
        }
    }

    void TryReload()
    {
        if (!isReload && Input.GetKeyDown(KeyCode.R) && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancleFinesight();
            StartCoroutine(ReloadCoroution());
        }
    }
    void GunFireRateCal()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 1초에 1씩 감소
        }
    }

    void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();

        }
    }

    void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();
            }
            else
            {
                CancleFinesight();
                StartCoroutine(ReloadCoroution());
            }
        }
        
        
    }

    IEnumerator ReloadCoroution()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;
            
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;
            //??

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount) 
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
        else
        {
            Debug.Log("총알 없어잉");
        }
    }

    void Shoot()
    {
        theCrosshair.FireAnimation();
        currentFireRate = currentGun.fireRate; // 연사 속도 재계산
        currentGun.currentBulletCount -= 1;
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Hit();

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }
    void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward + new Vector3
            (Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy)
            , Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy), 0), out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, 
            currentGun.fineInsightOriginPos.y, currentGun.fineInsightOriginPos.z);

        if (!isFineSightMode) // 일반 상태 반동
        {
            currentGun.transform.localPosition = originPos;

            // 반동 시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //원 위치
            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else // 정조준 상태 반동
        {
            currentGun.transform.localPosition = currentGun.fineInsightOriginPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //원 위치
            while (currentGun.transform.localPosition != currentGun.fineInsightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineInsightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    public void CancleReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    public void GunChange(Gun _gun)
    {
        if(WeaponManager.currentWeapon != null)
        {//무기 변경중이 아닐시 실행
            WeaponManager.currentWeapon.gameObject.SetActive(false); //기존 무기 비활성화
        }

        currentGun = _gun; //무기 바꿈
        WeaponManager.currentWeapon = currentGun.transform;
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero; //바꾼 무기 좌표 초기화
        currentGun.gameObject.SetActive(true); //바꾼 무기 활성화

        isActivate = true; //활성화 여부 변경
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }
}
