using UnityEngine;

public class Oven : MonoBehaviour
{
    [Header("Baking")]
    public GameObject bakedCakePrefab;
    public Transform spawnPoint;


    //When a mixed mixture enters the oven
    private void OnTriggerEnter(Collider other)
    {
        PickupItem mixture = other.GetComponent<PickupItem>();
        MixingBowl bowl = other.GetComponent<MixingBowl>();

        if (mixture == null || bowl == null) return;
        if (!bowl.IsMixed()) return;

        //Drop mixture if held
        PlayerInteractable player = FindObjectOfType<PlayerInteractable>();
        if (player != null && player.heldItem == mixture)
            player.DropItem();

        //Spawn baked cake
        if (bakedCakePrefab != null && spawnPoint != null)
        {
            GameObject cake = Instantiate(bakedCakePrefab, spawnPoint.position, spawnPoint.rotation);

            //Ensure PickupItem
            PickupItem cakePickup = cake.GetComponent<PickupItem>();
            if (cakePickup == null)
                cakePickup = cake.AddComponent<PickupItem>();

            //Ensure Rigidbody exists
            Rigidbody rb = cake.GetComponent<Rigidbody>();
            if (rb == null)
                rb = cake.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;

            //Ensure collider exists
            if (cake.GetComponent<Collider>() == null)
                cake.AddComponent<BoxCollider>();

            //Reset scale so it’s correct
            cake.transform.localScale = bakedCakePrefab.transform.localScale;

            //Auto-pickup
            if (player != null && player.hand != null)
            {
                if (player.heldItem != null)
                    player.DropItem();

                cakePickup.PickUp(player.hand); // now correctly scales with handScaleFactor
                player.heldItem = cakePickup;
            }
        }

        //Remove mixture
        Destroy(mixture.gameObject);

        Debug.Log("Mixture baked into cake!");
    }
}
