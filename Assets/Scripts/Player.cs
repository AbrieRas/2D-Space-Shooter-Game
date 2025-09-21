using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Take current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame; typically 60 frames per second
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;

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
}
