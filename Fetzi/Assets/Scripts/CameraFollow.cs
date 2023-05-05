using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    private Vector3 initialRotation;

    private AudioSource audioSource;

    private float BGVolumenStandart;

    private void Start()
    {
        initialRotation = transform.eulerAngles;
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        BGVolumenStandart = audioSource.volume;

        GameObject.Find("Canvas").GetComponent<CanvasScaler>().matchWidthOrHeight = PlayerPrefs.GetFloat("GUIScale", 0.5f);

    }

    private void LateUpdate()
    {


        audioSource.volume = BGVolumenStandart * PlayerPrefs.GetFloat("volume", 0.5f) * PlayerPrefs.GetFloat("volumeMusik", 0.5f);

        if (target != null)
        {
            desiredPosition = target.position + offset;
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }


        // Set the rotation to the initial rotation
        transform.eulerAngles = initialRotation;
    }
}