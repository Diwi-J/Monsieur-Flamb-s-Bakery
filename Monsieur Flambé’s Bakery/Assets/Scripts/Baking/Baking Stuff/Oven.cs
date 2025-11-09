using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Oven : MonoBehaviour
{
    [Header("Baking Settings")]
    public GameObject bakedCakePrefab;      //Prefab of the finished cake.
    public Transform spawnPoint;            //Where to spawn the cake.
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

        //Check for MixingBowl with mixture.
        PickupItem mixtureItem = other.GetComponent<PickupItem>();
        MixingBowl bowl = other.GetComponent<MixingBowl>();

        if (mixtureItem == null || bowl == null) return;
        if (!bowl.IsMixed()) return;

        //Drop the bowl if the player is holding it.
        PlayerInteractable player = FindObjectOfType<PlayerInteractable>();
        if (player != null && player.heldItem == mixtureItem)
            player.DropItem();

        StartCoroutine(BakeRoutine(bowl));
    }

    private IEnumerator BakeRoutine(MixingBowl bowl)
    {
        isBaking = true;
        Debug.Log("Baking started...");

        //Enable visual/audio effects.
        if (steamEffect != null) steamEffect.Play();
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.value = 0f;
        }

        float elapsed = 0f;
        while (elapsed < bakeTime)
        {
            elapsed += Time.deltaTime;

            if (progressBar != null)
                progressBar.value = Mathf.Clamp01(elapsed / bakeTime);

            yield return null;
        }

        Debug.Log("Baking finished!");

        //Stop effects.
        if (steamEffect != null) steamEffect.Stop();
        if (progressBar != null)
            progressBar.gameObject.SetActive(false);

        if (audioSource != null && dingSound != null)
            audioSource.PlayOneShot(dingSound);

        //Spawn the baked cake in the world, ready to pick up manually.
        if (bakedCakePrefab != null && spawnPoint != null)
        {
            GameObject cake = Instantiate(bakedCakePrefab, spawnPoint.position, spawnPoint.rotation);

            //Ensure it has PickupItem.
            PickupItem cakePickup = cake.GetComponent<PickupItem>();
            if (cakePickup == null)
                cakePickup = cake.AddComponent<PickupItem>();

            cakePickup.canPickUp = true;  //Allow manual pickup.

            //Ensure it has Rigidbody and Collider.
            Rigidbody rb = cake.GetComponent<Rigidbody>();
            if (rb == null) rb = cake.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;

            if (cake.GetComponent<Collider>() == null)
                cake.AddComponent<BoxCollider>();
        }

        //Destroy the mixing bowl after baking.
        Destroy(bowl.gameObject);

        isBaking = false;
    }
}
