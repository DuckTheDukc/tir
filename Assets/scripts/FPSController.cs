using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FPSController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 100f;
    public Transform playerCamera;
    public Transform weaponHolder;
    public GameObject weaponDropPrefab;
    public GameObject weaponHolderObj;
    private CharacterController controller;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Захват курсора
    }

    void Update()
    {
        // Вращение игрока по горизонтали
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Вращение камеры по вертикали
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Движение
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if  (Input.GetKeyDown(KeyCode.G) && weaponDropPrefab!=null)
            DropWeapon();
    
    }

    public void DropWeapon()
    {
       
        GameObject droppedWeapon = Instantiate(
            weaponDropPrefab,
            transform.position + transform.forward * 2f, 
            Quaternion.identity
        );
        droppedWeapon.SetActive( true );

        Rigidbody rb = droppedWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * 5f + Vector3.up * 2f, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse); 
        }
        weaponDropPrefab= null;
        Destroy(weaponHolderObj.transform.GetChild(0).gameObject);
    }
}