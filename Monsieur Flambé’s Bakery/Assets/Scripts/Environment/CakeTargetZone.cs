using UnityEngine;

public class CakeTargetZone : MonoBehaviour
{
    [Header("References")]
    public GameTimer gameTimer;        
    public Transform cakeSpawnPoint; 

    [HideInInspector] public bool cakePlaced = false;

    private void OnTriggerEnter(Collider other)
    {
        //Check if the object is a cake
        PickupItem pickup = other.GetComponent<PickupItem>();
        if (pickup != null && other.gameObject.name.Contains("Cake"))
        {
            //Stop timer
            gameTimer?.ObjectiveComplete();

            //Place the cake at a specific position and prevent pickup
            if (cakeSpawnPoint != null)
            {
                other.transform.position = cakeSpawnPoint.position;
                other.transform.rotation = cakeSpawnPoint.rotation;
            }
            pickup.enabled = false; //Prevents player from picking it up again

            //Mark cake as placed
            cakePlaced = true;

            Debug.Log("[CakeTargetZone] Cake has been placed!");
        }
    }
}
