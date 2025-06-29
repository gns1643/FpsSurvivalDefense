using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //���� ��
    [SerializeField]
    private Gun currentGun;

    //���� �ӵ�
    private float currentFireRate;

    //ȿ����
    private AudioSource audioSource;

    //���� ����
    private bool isReload = false;

    [HideInInspector]
    public bool isFineSightMode = false;

    //���� ������
    private Vector3 originPos;

    //�浹 ����
    private RaycastHit hitInfo;

    [SerializeField]
    private Camera theCam;

    //�ǰ� ����Ʈ
    [SerializeField]
    private GameObject hitEffectPrefab;

    private void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        GunFireRateCal();
        TryFire();
        TryReload();
        TryFineSight();
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
        Debug.Log(isFineSightMode);
        currentGun.anim.SetBool("FineSightMod", isFineSightMode);

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
            currentFireRate -= Time.deltaTime; // 1�ʿ� 1�� ����
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
            Debug.Log("�Ѿ� ������");
        }
    }

    void Shoot()
    {
        currentFireRate = currentGun.fireRate; // ���� �ӵ� ����
        currentGun.currentBulletCount -= 1;
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Hit();

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }
    void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo, currentGun.range))
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

        if (!isFineSightMode) // �Ϲ� ���� �ݵ�
        {
            currentGun.transform.localPosition = originPos;

            // �ݵ� ����
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //�� ��ġ
            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else // ������ ���� �ݵ�
        {
            currentGun.transform.localPosition = currentGun.fineInsightOriginPos;

            // �ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //�� ��ġ
            while (currentGun.transform.localPosition != currentGun.fineInsightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineInsightOriginPos, 0.1f);
                yield return null;
            }
        }
    }
}
