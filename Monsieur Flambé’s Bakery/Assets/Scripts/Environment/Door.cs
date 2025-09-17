using UnityEngine;

public class Door : Interactable
{
    private Animator animator;
    private bool isOpen = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
        Debug.Log($"Door {(isOpen ? "opened" : "closed")}");
    }
}
