using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    public GameObject weaponPrefab;  // ������ ������ ��� ���
    public float pickupRadius = 1.5f;

    void Update()
    {
        // ���������, ��������� �� ����� ����� � ����� �� E
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
        // 1. ������� ������ � ����� ������
        GameObject newWeapon = Instantiate(
            weaponPrefab,
            player.GetComponent<FPSController>().weaponHolder,
            false
        );
        GameObject tempCopy = Instantiate(gameObject);
        tempCopy.SetActive(false);
        player.GetComponent<FPSController>().weaponDropPrefab = tempCopy;
        // 2. ����������� ������
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

        // 3. ������� ������ � �����
        this.gameObject.SetActive(false);
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}