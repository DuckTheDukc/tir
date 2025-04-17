using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunRaycast : MonoBehaviour
{
    public float damage = 10f;          // Урон за выстрел
    public float range = 100f;          // Дальность стрельбы
    public float fireRate = 15f;        // Скорострельность (выстрелов в секунду)
    public Camera fpsCam;               // Камера игрока (откуда летит луч)
    public ParticleSystem muzzleFlash;  // Эффект дульного огня
    public GameObject impactEffect;     // Эффект попадания (например, искры)

    private float nextTimeToFire = 0f;   // Таймер для скорострельности

    void Update()
    {
        // Если зажата ЛКМ и прошло достаточно времени
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // 1. Проигрываем дульный огонь
        muzzleFlash.Play();

        // 2. Пускаем луч из центра камеры
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            // 4. Создаём эффект попадания
            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f); // Удаляем через 2 секунды
        }
    }
}