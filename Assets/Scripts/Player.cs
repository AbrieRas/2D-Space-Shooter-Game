using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotEnabled = false;
    [SerializeField]
    private bool _isShieldEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        // Take current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn_Manager is NULL.");
        }
    }

    // Update is called once per frame; typically 60 frames per second
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime;

        transform.Translate(direction);

        float maxAllowedHeight = 0;
        float minAllowedHeight = -3.8f;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minAllowedHeight, maxAllowedHeight), 0);

        float currentPlayerX = transform.position.x;
        float maxAllowedWidth = 9.7f;
        float minAllowedWidth = -9.7f;

        if (currentPlayerX >= maxAllowedWidth)
        {
            transform.position = new Vector3(minAllowedWidth, transform.position.y, 0);
        }
        else if (currentPlayerX <= minAllowedWidth)
        {
            transform.position = new Vector3(maxAllowedWidth, transform.position.y, 0);
        }
    }

    void FireLaser ()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotEnabled)
        {
            Vector3 tripleShotOffset = new Vector3(-0.462f, 0.037f, 0);
            Instantiate(_tripleShotPrefab, transform.position + tripleShotOffset, Quaternion.identity);
        }
        else
        {
            Vector3 offset = new Vector3(0, 0.5f, 0);
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        }
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void EnableTripleShot()
    {
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotEnabled = false;
    }

    public void EnableSpeed()
    {
        _speed *= 2;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _speed /= 2;
    }

    public void EnableShield()
    {
        _isShieldEnabled = true;
        StartCoroutine(ShieldPowerDownRoutine());
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isShieldEnabled = false;
    }
}
