using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum PickupType
{
    Health,
    EndLevel,
    Shards
}
public class PickUps : MonoBehaviour
{

    //---------------------------------- V A R I A B L E S ----------------------------------------------------------------------------

    public PickupType pickupType; 
    public int healthRecover = 4; 

    public Movement movement;    

    private AudioSource audioSource;
    public AudioClip CryxHSound, ShardsSound;

    //---------------------------------- A W A K E ----------------------------------------------------------------------------
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1f;
    }

    //---------------------------------- M E T H O D S ----------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision) //logica de recoleccion segun que tipo de pick up es
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
            else if (pickupType == PickupType.Shards)
            {
                CollectShards();
            }
        }
    }

    void CollectHealth()
    {
        if (movement != null && movement.currentHealth < movement.maxHealth) //si la vida actual es menor a la vida maxima
        {            
            movement.currentHealth += healthRecover; //recupera la vida correspondiente
            audioSource.PlayOneShot(CryxHSound);
            
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //setea la escena activa y le suma 1 
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex); //carga la siguiente escena
        }
    }

    void CollectShards()
    {
        Movement movement = FindObjectOfType<Movement>(); //busca los scripts de ataque y movimiento
        Attack attack = FindObjectOfType<Attack>();

        if (movement != null)
        {
            audioSource.PlayOneShot(ShardsSound);
            movement.shards++; //suma fragmentos y sube el ataque
            attack.damage = +5;
        }
        Destroy(gameObject);
    }
}
