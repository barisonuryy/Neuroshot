using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithCamera : MonoBehaviour
{
    public Transform camHolder;

    private void Update()
    {
        // Align the rotation of this object with the camHolder's rotation in the Y-axis only
        Vector3 targetRotation = new Vector3(0, camHolder.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(targetRotation);
    }
}