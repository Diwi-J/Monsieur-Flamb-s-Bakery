using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool forwardPressed = Input.GetKey("w");

        // When Player Presses W Key
        if (!isWalking && forwardPressed)
        {
            animator.SetBool("isWalking", true);
        }
        //When Player Doesn't Press W Key
        if (isWalking && !forwardPressed)
        {
            animator.SetBool("isWalking", false);
        }
    }
}
