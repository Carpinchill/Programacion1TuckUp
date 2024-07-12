using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Habilidades : MonoBehaviour
{
    public Movement movement;
    public Attack attack;

    public Image attackBoostCooldownImage;
    public Image blockCooldownImage;

    public float attackBoostDuration = 5f;
    public float attackBoostMultiplier = 2f;
    public float lifeCost = 4f;
    public float blockDuration = 3f;
    public bool isBlocking;

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
                    yield return null;
                }
                else
                {
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

        float totalCooldown = blockDuration + blockCooldown;
        float elapsed = 0f;       
        while (elapsed < totalCooldown)
        {
            if (elapsed < blockDuration)
            {
                isBlocking = true;
                yield return null;
            }
            else
            {
                isBlocking = false;
                blockCooldownImage.fillAmount = 1 - ((elapsed - blockDuration) / blockCooldown);
                yield return null;
            }

            elapsed += Time.deltaTime;
        }

        blockCooldownImage.fillAmount = 0;
    }
}