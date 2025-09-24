using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 2;
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
    private bool _isSpeedEnabled = false;
    [SerializeField]
    private bool _isShieldEnabled = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Take current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn_Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the Player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
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
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

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
            _audioSource.Play();
        }
        else
        {
            Vector3 offset = new Vector3(0, 0.5f, 0);
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            _audioSource.Play();
        }
    }

    public void Damage()
    {
        if (_isShieldEnabled == true)
        {
            _isShieldEnabled = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

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
        _isSpeedEnabled = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _speed /= _speedMultiplier;
        _isSpeedEnabled = false;
    }

    public void EnableShield()
    {
        _isShieldEnabled = true;
        _shieldVisualizer.SetActive(true);
        StartCoroutine(ShieldPowerDownRoutine());
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _shieldVisualizer.SetActive(false);
        _isShieldEnabled = false;
    }

    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }
}
