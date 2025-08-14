using UnityEngine;

/*public class Door : Interactable
{
    public bool isOpen = false;
    public float openAngle = 90f;
    public float speed = 2f;  //Rotations per second

    private Quaternion closedRotation; 
    private Quaternion openRotation;
    private Coroutine rotateCoroutine;

    private void Start()
    {
        //Store the initial rotation as the closed state
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    public override void Interact()
    {
        //Toggle door state
        isOpen = !isOpen;

        //Stop ongoing rotation coroutine if any
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        //Start rotating towards target rotation
        rotateCoroutine = StartCoroutine(RotateDoor());
    }

    private System.Collections.IEnumerator RotateDoor()
    {
        //Calculate the target rotation based on the door state
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        float timeElapsed = 0f;
        float duration = 1f / speed; //Duration based on speed

        //Smoothly rotate the door using Slerp
        while (timeElapsed < duration)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //Ensure the final rotation is set to the target rotation
        transform.localRotation = targetRotation;
        rotateCoroutine = null;
    }
}*/
