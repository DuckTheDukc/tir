using System.Collections;
using UnityEngine;

public class GunRaycast : MonoBehaviour
{
    [Header("�������� ���������")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public Camera fpsCam;
    public GameObject gunShotEffectPrefab;
    public GameObject impactEffect;
    public AudioClip gunshotSound;
    [Range(0, 1)] public float gunshotVolume = 0.5f;
    private AudioSource audioSource;

    [Header("������")]
    public float recoilForce = 0.1f;    // ���� ������ (�������� ������)
    public float recoilUpward = 0.05f;  // ������������ ������
    public float recoilDuration = 0.1f; // ����� ��������
    public float cameraShakeIntensity = 0.1f; // ������ ������
    public float spreadAngle = 1f;      // ������� ���� ��� ��������

    private float nextTimeToFire = 0f;
    private Vector3 originalPosition;   // �������� ������� ������
    private Transform weaponTransform;
    [Header("������������ ������")]
    public float rotationRecoil = 2f;    // ���� �������� �� X
    public float sideRotationRecoil = 1f; // ��������� �������� �� Y
    public float rotationReturnSpeed = 8f; // �������� ��������

    private Quaternion originalRotation; // �������� ��������
    private float currentRecoilX;       // ������� ������ �� X
    private float currentRecoilY;       // ������� ������ �� Y
    void Awake()
    {
        fpsCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        weaponTransform = transform;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        // ������� ������� ������ ����� ������
        if (Time.time >= nextTimeToFire - 0.1f)
        {
            weaponTransform.localPosition = Vector3.Lerp(
                weaponTransform.localPosition,
                originalPosition,
                Time.deltaTime * 10f
            );
        }
        currentRecoilX = Mathf.Lerp(currentRecoilX, 0, Time.deltaTime * rotationReturnSpeed);
        currentRecoilY = Mathf.Lerp(currentRecoilY, 0, Time.deltaTime * rotationReturnSpeed);

        weaponTransform.localRotation = originalRotation *
            Quaternion.Euler(-currentRecoilX, currentRecoilY, 0);
    }

    void Shoot()
    {
        if (gunshotSound != null)
        {
            audioSource.PlayOneShot(gunshotSound, gunshotVolume);
        }
        // 1. ������ ��������
        GameObject effect = Instantiate(gunShotEffectPrefab, weaponTransform.position, weaponTransform.rotation);
        Destroy(effect, 2f);
        foreach (ParticleSystem ps in effect.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }

        // 2. ��������� ������
        ApplyRecoil();

        // 3. ������ �������� � ������ ������
        Vector3 shootDirection = fpsCam.transform.forward;
        shootDirection += new Vector3(
            Random.Range(-spreadAngle, spreadAngle),
            Random.Range(-spreadAngle, spreadAngle),
            0
        ) * 0.01f;

        // 4. �������
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, shootDirection, out hit, range))
        {   
            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            Target target = hit.collider.GetComponent<Target>();
            if (target != null)
            {
                target.Hit(); // �������� ����� Hit � ���������� ������
                Debug.Log("��������� � ������: " + hit.collider.name);
            }

            Destroy(impact, 2f);
        }
    }

    void ApplyRecoil()
    {
        // �������� ������ �����
        weaponTransform.localPosition -= Vector3.forward * recoilForce;
        // ��������� ������������ ������
        weaponTransform.localPosition += Vector3.up * recoilUpward;

        // ������ ������
        StartCoroutine(CameraShake());

        // ����� �������� �������� (�����������)
        weaponTransform.localRotation *= Quaternion.Euler(
            -recoilForce * 10f,
            Random.Range(-2f, 2f),
            0
        );
        currentRecoilX += rotationRecoil;
        currentRecoilY += Random.Range(-sideRotationRecoil, sideRotationRecoil);
    }

    IEnumerator CameraShake()
    {
        Vector3 originalCamPos = fpsCam.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < recoilDuration)
        {
            fpsCam.transform.localPosition = originalCamPos +
                Random.insideUnitSphere * cameraShakeIntensity;

            elapsed += Time.deltaTime;
            yield return null;
        }

        fpsCam.transform.localPosition = originalCamPos;
    }


}