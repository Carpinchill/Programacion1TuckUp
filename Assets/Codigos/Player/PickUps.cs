using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum PickupType
{
    Health,
    EndLevel
}
public class PickUps : MonoBehaviour
{
    public PickupType pickupType; 
    public int healthRecover = 4; 

    public Movement movement; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (pickupType == PickupType.Health)
            {
                CollectHealth();
            }
            else if (pickupType == PickupType.EndLevel)
            {
                EndLevel();
            }
        }
    }

    void CollectHealth()
    {
        if (movement != null && movement.currentHealth >= movement.maxHealth)
        {
            movement.currentHealth += healthRecover;

            if (movement.currentHealth > movement.maxHealth)
            {
                movement.currentHealth = movement.maxHealth;
            }

            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }

    void EndLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

}
