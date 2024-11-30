using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{

    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab; // Prefab pocisku
    [SerializeField] private Transform bulletSpawnPoint; // Punkt spawnu pocisku
    [SerializeField] private float bulletSpeed = 20f; // Pr�dko�� pocisku
    [SerializeField] private float fireRate = 0.5f; // Czas mi�dzy strza�ami (w sekundach)

    private float nextFireTime = 0f; // Czas, kiedy gracz mo�e ponownie strzeli�


    void Update()
    {

        // Sprawdzamy, czy gracz klikn�� lewy przycisk myszy i mo�e strzeli�
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate; // Ustawiamy czas kolejnego mo�liwego strza�u
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            // Tworzymy pocisk w pozycji i rotacji punktu spawnu
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            // Nadajemy pociskowi pr�dko��
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
            }

            // Opcjonalne: zniszczenie pocisku po pewnym czasie
            Destroy(bullet, 5f); // Usuwa pocisk po 5 sekundach
        }
        else
        {
            Debug.LogWarning("Bullet prefab or spawn point is not assigned!");
        }
    }
}
