using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Habilidades : MonoBehaviour
{
    public Movement movement;
    public Attack attack;
    public GameObject ShieldEffect;
    public GameObject BoostEffect;

    public Image attackBoostCooldownImage;
    public Image blockCooldownImage;
    public Image revivalCooldownImage;

    public float revivalDuration = 5f;
    public float healthRestorePercentage = 0.3f;
    public float staminaRestorePercentage = 0.7f;
    public float revivalCooldown = 220f;
    private float revivalEndTime = 0f;
    public float attackBoostDuration = 5f;
    public float attackBoostMultiplier = 2f;
    public float lifeCost = 4f;
    public float immuneDuration = 3f;

    public bool isRevivalActive = false;  
    public bool isImmune;

    [SerializeField]
    private float attackBoostCooldown = 8f;
    [SerializeField]
    private float blockCooldown = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && attackBoostCooldownImage.fillAmount == 0)
        {
            StartCoroutine(BoostAttack());
        }

        if (Input.GetKeyDown(KeyCode.Q) && blockCooldownImage.fillAmount == 0)
        {
            StartCoroutine(BlockNextHit());
        }
        if (Input.GetKeyDown(KeyCode.R) && revivalCooldownImage.fillAmount == 0)
        {
            StartCoroutine(ActivateRevival());
        }
    }

    IEnumerator BoostAttack()
    {
        if (movement.currentHealth > lifeCost)
        {
            movement.currentHealth -= lifeCost;
            attack.damage *= attackBoostMultiplier;
            attackBoostCooldownImage.fillAmount = 1;

            float totalCooldown = attackBoostDuration + attackBoostCooldown;
            float elapsed = 0f;

            while (elapsed < totalCooldown)
            {
                if (elapsed < attackBoostDuration)
                {
                    BoostEffect.SetActive(true);
                    yield return null;
                }
                else
                {
                    BoostEffect.SetActive(false);
                    attackBoostCooldownImage.fillAmount = 1 - ((elapsed - attackBoostDuration) / attackBoostCooldown);
                    yield return null;
                }

                elapsed += Time.deltaTime;
            }

            attack.damage /= attackBoostMultiplier;
            attackBoostCooldownImage.fillAmount = 0;
        }
    }

    IEnumerator BlockNextHit()
    {
        blockCooldownImage.fillAmount = 1;
        ShieldEffect.SetActive(true);
        float totalCooldown = immuneDuration + blockCooldown;
        float elapsed = 0f;       
        while (elapsed < totalCooldown)
        {
            if (elapsed < immuneDuration)
            {
                isImmune = true;
                yield return null;
            }
            else
            {
                ShieldEffect.SetActive(false);
                isImmune = false;
                blockCooldownImage.fillAmount = 1 - ((elapsed - immuneDuration) / blockCooldown);
                yield return null;
            }

            elapsed += Time.deltaTime;
        }

        blockCooldownImage.fillAmount = 0;
    }

    IEnumerator ActivateRevival()
    {
        if (isRevivalActive)
        {
            revivalCooldownImage.fillAmount = 1 - (Time.time - revivalEndTime) / revivalCooldown;
            if (Time.time >= revivalEndTime)
            {
                isRevivalActive = false;
                revivalCooldownImage.fillAmount = 0;
            }
        }
        isRevivalActive = true;
        revivalEndTime = Time.time + revivalCooldown + revivalDuration; 
        yield return new WaitForSeconds(revivalDuration);

    }

    public bool IsRevivalActive()
    {
        return isRevivalActive;
    }

    public float GetHealthRestorePercentage()
    {
        return healthRestorePercentage;
    }

    public float GetStaminaRestorePercentage()
    {
        return staminaRestorePercentage;
    }
}
