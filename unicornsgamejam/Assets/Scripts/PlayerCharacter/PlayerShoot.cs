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
    private Vector2 inputMouse;
    public GameObject player;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Shoot.performed += context => StartShooting();
        controls.Gameplay.Shoot.canceled += context => StopShooting();
        controls.Gameplay.Mouse.performed += x => inputMouse = x.ReadValue<Vector2>();
    }

    private void Update()
    {
        Debug.Log("Mouse: " + inputMouse);
        Debug.Log("Player: " + transform.position);
        Vector3 mousePos = inputMouse;
        Vector3 shootPos = Camera.main.WorldToScreenPoint(shootPoint.position);
        mousePos.x = mousePos.x - shootPos.x;
        mousePos.y = mousePos.y - shootPos.y;
        float shootAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if(inputMouse.x < shootPoint.position.x)
        {
            shootPoint.rotation = Quaternion.Euler(new Vector3(180f, 0f, -shootAngle));
        } else {
            shootPoint.rotation = Quaternion.Euler(new Vector3(0f, 0f, shootAngle));
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        player = GameObject.Find("PlayerCharacter");
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
