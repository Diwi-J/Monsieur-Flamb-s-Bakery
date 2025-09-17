using UnityEngine;

public class Oven : MonoBehaviour
{
    [Header("Baking")]
    public GameObject bakedCakePrefab;
    public Transform spawnPoint; //BakedCake spawn point.

    private void OnTriggerEnter(Collider other)
    {
        //Checks if the collided object is a PickupItem.
        PickupItem mixture = other.GetComponent<PickupItem>();
        if (mixture == null) return;

        //Only allows "Mixture" items.
        if (!mixture.gameObject.name.Contains("Mixture")) return;

        //Removes mixture from player if held.
        PlayerInteractable player = FindObjectOfType<PlayerInteractable>();
        if (player != null && player.heldItem == mixture)
        {
            player.DropItem();
        }


        Destroy(mixture.gameObject);

        //Spawn baked cake.
        if (bakedCakePrefab != null && spawnPoint != null)
        {
            Instantiate(bakedCakePrefab, spawnPoint.position, spawnPoint.rotation);
        }

        Debug.Log("[Oven] Mixture baked!");
    }
}

