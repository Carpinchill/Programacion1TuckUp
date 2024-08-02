using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using Unity.Collections.LowLevel.Unsafe;

public class Movement : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------------------------------------------------
    public Attack attack;
    public Habilidades habilidades;
    private Rigidbody2D rb2d;
    public GameObject DashEffect;
    public GameObject ShieldEffect;
    public GameObject BoostEffect;
    public GameObject BlockEffect;
    private Animator animator;
    public Image health;
    public Image stamina;
    public TMP_Text shardsCounter;

    public static string currentSceneName;

    public float speed = 5f;
    public int maxHealth = 20;
    public float currentHealth;
    public float maxStamina = 10;
    public float currentStamina;
    public float shards;
    private float lastStaminaUseTime;
    public float staminaRecoveryDelay = 2f;
    public float staminaRecoveryRate = 4f;
    public float dashStaminaCost = 2f;
    public float damageReduction;

    public bool isDashing = false;
    public bool isBlocking = false;
    [SerializeField]
    private float dashDuration;
    [SerializeField]
    private float dashForce;
    public float dashCooldown = 1.2f;
    private float lastDashTime;

    public float lastH, lastV;

    private AudioSource audioSource;
    public AudioClip playerHurtSound, dashSound;
    public float minPitch = 0.80f;
    public float maxPitch = 1.25f;

    //---------------------------------- A W A K E ----------------------------------------------------------------------------

    //cuando inicia pone el primer frame, setea la salud al maximo, desactiva el efecto del dash y las hitbox y agarra el rigidbody
    private void Awake()
    {
        DashEffect.SetActive(false);
        ShieldEffect.SetActive(false);
        BoostEffect.SetActive(false);
        BlockEffect.SetActive(false);
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        animator = GetComponent<Animator>();
    }

    //---------------------------------- F. U P D A T E ----------------------------------------------------------------------------


    private void FixedUpdate()
    {
        Move();
        UpdateHealthBar();
        UpdateStaminaBar();
        shardsCounter.text = shards.ToString();
        if (Time.time >= lastStaminaUseTime + staminaRecoveryDelay)
        {
            RecoverStamina();
        }
    }

    //---------------------------------- L. U P D A T E ----------------------------------------------------------------------------


    //setea las animaciones correspondientes
    private void LateUpdate()
    {
        animator.SetFloat("LastH", lastH);
        animator.SetFloat("LastV", lastV);
    }

    //---------------------------------- M E T O D O S ----------------------------------------------------------------------------



    //con esto se mueve y guarda la ultima direccion hacia donde fue para el dash y las animaciones
    void Move()
    {
        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");

        rb2d.MovePosition((Vector2)transform.position + speed * Time.fixedDeltaTime * new Vector2(axisH, axisV).normalized);

        if(axisH != 0 || axisV != 0)
        { 
            lastH = axisH;
            lastV = axisV;
        }
    }


    //chequea la ultima direccion y la pasa a la corutina para saber hacia donde dashear
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + dashCooldown && currentStamina >= dashStaminaCost) 
        {
            float dashH = lastH != 0 ? lastH : Input.GetAxisRaw("Horizontal");
            float dashV = lastV != 0 ? lastV : Input.GetAxisRaw("Vertical");
            Vector2 dashDirection = new Vector2(dashH, dashV).normalized;
            
            audioSource.PlayOneShot(dashSound);
            StartCoroutine(Dash(dashDirection));
        }
        HandleBlockInput();
    }

    

    //este es el codigo para el dash
    IEnumerator Dash(Vector2 direction)
    {
        lastDashTime = Time.time;
        isDashing = true;
        DashEffect.SetActive(true);       
        rb2d.velocity = direction * dashForce;
        enabled = false;
        attack.enabled = false;
        ConsumeStamina(dashStaminaCost);

        yield return new WaitForSeconds(dashDuration);

        rb2d.velocity = Vector2.zero;
        isDashing = false;
        enabled = true; 
        DashEffect.SetActive(false);
        attack.enabled = true;
    }
    public void ConsumeStamina(float amount)
    {
        currentStamina -= amount;
        lastStaminaUseTime = Time.time;
        if (currentStamina < 0) currentStamina = 0;
    }
    private void RecoverStamina()
    {
        currentStamina += staminaRecoveryRate * Time.deltaTime;
        if (currentStamina > maxStamina) currentStamina = maxStamina;
    }
    public void TakeDamage(float damage, Vector2 impactSource, float knockbackForce)
    {
        bool applyDamage = true;

        if (isBlocking)
        {
            float actualDamage = damage * damageReduction;
            currentHealth -= actualDamage;
            ConsumeStamina(3f);
        }
        else if (isDashing == false && !habilidades.isImmune)
        {
            Vector2 knockbackDirection = (transform.position - (Vector3)impactSource).normalized;
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(playerHurtSound);
            ApplyKnockback(knockbackDirection, knockbackForce);

            float actualDamage = damage;

            //si el buff está activo y el daño sería fatal
            if (habilidades.IsRevivalActive() && currentHealth - actualDamage <= 0)
            {
                //calcular la restauración de salud y stamina
                float healthRestore = maxHealth * habilidades.GetHealthRestorePercentage();
                float staminaRestore = maxStamina * habilidades.GetStaminaRestorePercentage();

                //restaurar salud y stamina, y evitar la muerte
                currentHealth = Mathf.Min(currentHealth + healthRestore, maxHealth);
                currentStamina = Mathf.Min(currentStamina + staminaRestore, maxStamina);

                //cancelar el daño
                applyDamage = false;
            }
            else
            {
                currentHealth -= actualDamage;
            }
        }

        //si no hay buff activo y la salud llega a 0, el personaje muere
        if (applyDamage && currentHealth <= 0)
        {
            Die();
        }

    }
    public void UpdateHealthBar()
    {
        float fillAmount = currentHealth / maxHealth;
        health.fillAmount = fillAmount;
    }
    public void UpdateStaminaBar()
    {
        float fillAmount = currentStamina / maxStamina;
        stamina.fillAmount = fillAmount;
    }

    private void ApplyKnockback(Vector2 direction, float force)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
    }

    void HandleBlockInput()
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            StartBlocking();
        }
        else
        {
            StopBlocking();
        }
    }

    void StartBlocking()
    {
        if (!isBlocking)
        {
            BlockEffect.SetActive(true);
            isBlocking = true;
            speed *= 0.6f;
            damageReduction = 0.7f;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            if (horizontal != 0 || vertical != 0)
            {
                lastH = horizontal;
                lastV = vertical;
            }
        }
    }

    void StopBlocking()
    {
        if (isBlocking)
        {
            BlockEffect.SetActive(false);
            isBlocking = false;
            speed = 5f;
            damageReduction = 0f;
        }
    }


    private void Die()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        enabled = false;
        attack.enabled = false;
        SceneManager.LoadScene("Muerte");
    }
    
}