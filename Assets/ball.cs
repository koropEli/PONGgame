using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;

public class ball : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public float Speed = 6f;
    public UIManager UIManager;

    public int RightPlayerScore;
    public int LeftPlayerScore;

    public static event Action BallReset;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        SendBallRandomDirection();

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the ball object.");
        }
        else
        {
            Debug.Log("SpriteRenderer successfully found on the ball object.");
        }
    }

    private void SendBallRandomDirection()
    {
        BallReset?.Invoke();

        rigidbody2D.velocity = Vector3.zero;
        rigidbody2D.isKinematic = true;
        transform.position = Vector3.zero;
        rigidbody2D.isKinematic = false;

        Vector2 newBallVector = new Vector2();
        newBallVector.x = Random.Range(-1f, 1f);
        newBallVector.y = Random.Range(-1f, 1f);
        rigidbody2D.velocity = newBallVector.normalized * Speed;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SendBallRandomDirection();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<movement>() == null)
            return;

        collision.gameObject.GetComponent<movement>().speed *= 1.1f;
        rigidbody2D.velocity *= 1.1F;

        Debug.Log("The ball collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("The ball collided with a wall with the 'Wall' tag.");
            ChangeColor();
        }
        else
        {
            Debug.Log("Collision object: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.x > 0)
        {
            Debug.Log("Player Left +1");
            LeftPlayerScore++;
            UIManager.SetLeftPlayerScoreText(LeftPlayerScore.ToString());
        }
        else
        {
            Debug.Log("Player Right +1");
            RightPlayerScore++;
            UIManager.SetRightPlayerScoreText(RightPlayerScore.ToString());
        }

        SendBallRandomDirection();
    }

    private SpriteRenderer spriteRenderer;

    void ChangeColor()
    {
        Color newColor = new Color(Random.value, Random.value, Random.value);
        spriteRenderer.color = newColor;

        Debug.Log("Ball color changed to: " + newColor);
    }
}
