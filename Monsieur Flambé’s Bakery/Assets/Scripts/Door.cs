using UnityEngine;

public class Door : Interactable
{
    public bool isOpen = false;
    public float openAngle = 90f;
    public float speed = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isMoving = false;

    private void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor());
    }

    private System.Collections.IEnumerator RotateDoor()
    {
        isMoving = true;
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * speed);
            yield return null;
        }
        transform.localRotation = targetRotation;
        isMoving = false;
    }
}
