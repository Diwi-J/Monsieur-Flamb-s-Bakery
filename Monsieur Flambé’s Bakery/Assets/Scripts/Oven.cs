using System.Collections;
using UnityEngine;

public class Oven : MonoBehaviour
{
    [SerializeField] private GameObject cakePrefab;
    [SerializeField] private Transform cakeSpawnPoint;
    [SerializeField] private float bakingDuration = 5f;

    private bool isBaking = false;

    private void OnTriggerEnter(Collider other)
    {
        var bowl = other.GetComponent<MixingBowl>();
        if (bowl != null && bowl.IsMixed() && !isBaking)
        {
            StartCoroutine(BakeCoroutine(bowl));
        }
    }

    private IEnumerator BakeCoroutine(MixingBowl bowl)
    {
        isBaking = true;
        Debug.Log("Baking started...");

        yield return new WaitForSeconds(bakingDuration);

        Instantiate(cakePrefab, cakeSpawnPoint.position, Quaternion.identity);

        Debug.Log("Cake is ready!");

        bowl.ResetBowl();

        GameManager.Instance.AdvanceStage(CakeStage.CakeReady);
        isBaking = false;
    }
}
