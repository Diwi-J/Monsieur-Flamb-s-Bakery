using UnityEngine;

public class Oven : MonoBehaviour
{
    [Header("Baking")]
    public GameObject bakedCakePrefab; // prefab to spawn after baking
    public Transform spawnPoint;       // where baked cake appears

    private void OnTriggerEnter(Collider other)
    {
        PickupItem mixture = other.GetComponent<PickupItem>();
        if (mixture == null) return;

        // Only allow "Mixture" items (optional: check name or tag)
        if (!mixture.gameObject.name.Contains("Mixture")) return;

        // Remove mixture from player if held
        PlayerInteractable player = FindObjectOfType<PlayerInteractable>();
        if (player != null && player.heldItem == mixture)
        {
            player.DropItem();
        }

        // Destroy the mixture prefab
        Destroy(mixture.gameObject);

        // Spawn baked cake
        if (bakedCakePrefab != null && spawnPoint != null)
        {
            Instantiate(bakedCakePrefab, spawnPoint.position, spawnPoint.rotation);
        }

        Debug.Log("[Oven] Mixture baked!");
    }
}
