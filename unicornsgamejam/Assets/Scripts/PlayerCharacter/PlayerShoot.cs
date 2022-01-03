using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //cached references
    private PlayerControls controls;
    private Animator animator;
    private PlayerMovement movement;

    //state
    private bool shooting;
    private Coroutine currentSpawnBulletInstance;

    //config
    [Header("Shooting")]
    [SerializeField]
    private float bulletSpawnInterval = 0.5f;
    [SerializeField]
    private float shootingRunModifier = 0.66f;

    [Header("Manual References")]
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform shootPoint;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Shoot.performed += context => StartShooting();
        controls.Gameplay.Shoot.canceled += context => StopShooting();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    private void StartShooting()
    {
        shooting = true;
        currentSpawnBulletInstance = StartCoroutine(SpawnBullet());
        movement.SetRunSpeedModifier(shootingRunModifier);

        //animation
        animator.SetBool("Shooting", true);
    }
    private void StopShooting()
    {
        shooting = false;
        StopCoroutine(currentSpawnBulletInstance);
        movement.ResetRunSpeedModifier();

        //animation
        animator.SetBool("Shooting", false);
    }
    private IEnumerator SpawnBullet()
    {
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        yield return new WaitForSeconds(bulletSpawnInterval);
        if (shooting) StartCoroutine(SpawnBullet());
    }


    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
