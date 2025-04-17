using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    public GameObject weaponPrefab;  // Префаб оружия для рук
    public float pickupRadius = 1.5f;

    void Update()
    {
        // Проверяем, находится ли игрок рядом и нажал ли E
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
            {
                PickUp(collider.transform);
                break;
            }
        }
    }

    void PickUp(Transform player)
    {
        // 1. Создаем оружие в руках игрока
        GameObject newWeapon = Instantiate(
            weaponPrefab,
            player.GetComponent<FPSController>().weaponHolder,
            false
        );
        GameObject tempCopy = Instantiate(gameObject);
        tempCopy.SetActive(false);
        player.GetComponent<FPSController>().weaponDropPrefab = tempCopy;
        // 2. Настраиваем оружие
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

        // 3. Удаляем оружие с земли
        this.gameObject.SetActive(false);
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}