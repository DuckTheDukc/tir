using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunRaycast : MonoBehaviour
{
    public float damage = 10f;          // ���� �� �������
    public float range = 100f;          // ��������� ��������
    public float fireRate = 15f;        // ���������������� (��������� � �������)
    public Camera fpsCam;               // ������ ������ (������ ����� ���)
    public GameObject gunShotEffectPrefab;  // ������ � ����������
    public GameObject impactEffect;     // ������ ��������� (��������, �����)

    private float nextTimeToFire = 0f;   // ������ ��� ����������������


    void Awake()
    {
       fpsCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    void Update()
    {
        // ���� ������ ��� � ������ ���������� �������
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // 1. ����������� ������� �����
        GameObject effect = Instantiate(gunShotEffectPrefab, transform.position, transform.rotation);
        Destroy(effect, 2f);
        foreach (ParticleSystem ps in effect.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }
        // 2. ������� ��� �� ������ ������
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            // 4. ������ ������ ���������
            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f); // ������� ����� 2 �������

            // ���������, ������ �� � ����� ��������
        }
    }
}