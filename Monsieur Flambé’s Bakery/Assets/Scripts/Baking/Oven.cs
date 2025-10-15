using UnityEngine;
using System.Collections;
using UnityEngine.UI; // for progress bar UI

public class Oven : MonoBehaviour
{
    [Header("Baking Settings")]
    public GameObject bakedCakePrefab;
    public Transform spawnPoint;
    public float bakeTime = 5f;

    [Header("Effects")]
    public AudioSource audioSource;      
    public AudioClip dingSound;          
    public ParticleSystem steamEffect;   
    public Slider progressBar;           

    private bool isBaking = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isBaking) return;

        PickupItem mixture = other.GetComponent<PickupItem>();
        MixingBowl bowl = other.GetComponent<MixingBowl>();

        if (mixture == null || bowl == null) return;
        if (!bowl.IsMixed()) return;

        PlayerInteractable player = FindObjectOfType<PlayerInteractable>();
        if (player != null && player.heldItem == mixture)
            player.DropItem();

        StartCoroutine(BakeRoutine(mixture, player));
    }

    private IEnumerator BakeRoutine(PickupItem mixture, PlayerInteractable player)
    {
        isBaking = true;
        Debug.Log("Baking started...");

        //Enable visual/audio effects
        if (steamEffect != null) steamEffect.Play();
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.value = 0f;
        }

        float elapsed = 0f;

        //While baking time not done
        while (elapsed < bakeTime)
        {
            elapsed += Time.deltaTime;

            //Update UI progress
            if (progressBar != null)
                progressBar.value = Mathf.Clamp01(elapsed / bakeTime);

            yield return null;
        }

        Debug.Log("Baking finished!");

        // Stop effects
        if (steamEffect != null) steamEffect.Stop();
        if (progressBar != null)
            progressBar.gameObject.SetActive(false);

        // Play ding sound
        if (audioSource != null && dingSound != null)
            audioSource.PlayOneShot(dingSound);

        // Spawn baked cake
        if (bakedCakePrefab != null && spawnPoint != null)
        {
            GameObject cake = Instantiate(bakedCakePrefab, spawnPoint.position, spawnPoint.rotation);

            PickupItem cakePickup = cake.GetComponent<PickupItem>();
            if (cakePickup == null)
                cakePickup = cake.AddComponent<PickupItem>();

            Rigidbody rb = cake.GetComponent<Rigidbody>();
            if (rb == null)
                rb = cake.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;

            if (cake.GetComponent<Collider>() == null)
                cake.AddComponent<BoxCollider>();

            cake.transform.localScale = bakedCakePrefab.transform.localScale;

            if (player != null && player.hand != null)
            {
                if (player.heldItem != null)
                    player.DropItem();

                cakePickup.PickUp(player.hand);
                player.heldItem = cakePickup;
            }
        }

        Destroy(mixture.gameObject);
        isBaking = false;
    }
}
