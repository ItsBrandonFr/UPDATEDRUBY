using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public ParticleSystem smokeEffect;
    public AudioClip hitSound; // Add this line for the audio clip

    private new Rigidbody2D rigidbody2D;
    private static int count = 0;
    private float timer;
    private int direction = 1;
    private bool broken = true;
    private Animator animator;
    private AudioSource audioSource; // Add this line for audio source

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        timer = changeTime;
        animator = GetComponent<Animator>();

        // Add the following lines for audio initialization
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    public void SetCountText()
    {
        Debug.Log("Total Enemies Defeated: " + count);

        if (countText != null)
        {
            countText.text = "Total Enemies Defeated: " + count;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on countText. Make sure it's assigned in the Unity Editor.");
        }

        if (count >= 2)
        {
            winTextObject.SetActive(true);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Enemy collision with: " + other.gameObject.name);

        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
            PlayHitSound(); // Play audio when player hits the enemy
        }

        Projectile projectile = other.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            Debug.Log("Enemy hit by projectile!");
            Fix();
        }
    }

    void PlayHitSound()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    public void Fix()
    {
        if (!broken) return;

        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();

        count = count + 1;
        SetCountText();
        PlayHitSound();
        
        Debug.Log("Total Enemies Defeated: " + count);
    }
}
