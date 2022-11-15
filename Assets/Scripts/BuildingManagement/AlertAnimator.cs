using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertAnimator : MonoBehaviour
{
    public float verticalSpeed = 1;
    public float verticalAmp = 1;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * verticalAmp, 0);
    }
}
