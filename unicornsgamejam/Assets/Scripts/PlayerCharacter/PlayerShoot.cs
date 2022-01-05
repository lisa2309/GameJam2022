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

    [SerializeField]
    private GameObject mushroom;
    public GameObject point;
    GameObject[] points;
    public int numberOfPoints;
    public float spaceBetweenPoints;
    Vector2 direction;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Shoot.performed += context => StartShooting();
        controls.Gameplay.Shoot.canceled += context => StopShooting();
        controls.Gameplay.Mouse.performed += x => inputMouse = x.ReadValue<Vector2>();
    }

    private void Update()
    {
        Vector3 mousePos = inputMouse;
        Vector3 shootPos = Camera.main.WorldToScreenPoint(shootPoint.position);
        mousePos.x = mousePos.x - shootPos.x;
        mousePos.y = mousePos.y - shootPos.y;
        direction = new Vector2(mousePos.x, mousePos.y);
        float shootAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if(inputMouse.x < shootPoint.position.x)
        {
            shootPoint.rotation = Quaternion.Euler(new Vector3(180f, 0f, -shootAngle));
        } else {
            shootPoint.rotation = Quaternion.Euler(new Vector3(0f, 0f, shootAngle));
        }

        for(int i = 0; i< numberOfPoints; i++)
        {
            points[i].transform.position= PointPosition(i * spaceBetweenPoints);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        player = GameObject.Find("PlayerCharacter");
        points = new GameObject[numberOfPoints];
        for(int i = 0; i< numberOfPoints; i++)
        {
            points[i] = Instantiate(point, shootPoint.position, Quaternion.identity);
        }
    }

    private void StartShooting()
    {
        if(player.GetComponent<PlayerMovement>().isBuried == true){
        shooting = true;
        currentSpawnBulletInstance = StartCoroutine(SpawnBullet());
        movement.SetRunSpeedModifier(shootingRunModifier);

        //animation
        animator.SetBool("Shooting", true);
        }
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
        Instantiate(mushroom, transform.position, transform.rotation);
        yield return new WaitForSeconds(bulletSpawnInterval);
        if (shooting) StartCoroutine(SpawnBullet());
    }

    Vector2 PointPosition(float t)
    {
        Vector2 position = (Vector2)shootPoint.position + direction.normalized * bulletPrefab.GetComponent<Bullet>().velocity * t + 0.5f * Physics2D.gravity * (t * t);
        return position;
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
