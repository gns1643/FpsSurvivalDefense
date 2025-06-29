using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        GunFireRateCal();
        TryFire();
    }
    void GunFireRateCal()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 1ÃÊ¿¡ 1¾¿ °¨¼Ò
        }
    }

    void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();

        }
    }

    void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    void Shoot()
    {
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Debug.Log("»Ñ½¹»Ñ½¹");
    }
    void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
