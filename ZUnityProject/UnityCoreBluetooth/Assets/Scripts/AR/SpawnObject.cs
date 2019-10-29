using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSessionOrigin))]
public class SpawnObject : MonoBehaviour
{

    private ARSessionOrigin origin;
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    [SerializeField]
    private Transform controllerTransform;

    [SerializeField] GameObject prefab;

    private void Awake()
    {
        origin = GetComponent<ARSessionOrigin>();
    }

    private GameObject targetGameObject = null;
    private Renderer targetRenderer = null;
    private Rigidbody targetRigidbody = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (origin.Raycast(Input.GetTouch(0).position, hitResults))
            {
                Instantiate(prefab, hitResults[0].pose.position, Quaternion.identity);
            }
        }

        this.searchSpawnObject();
    }

    private void searchSpawnObject()
    {
        if (targetRenderer != null)
            targetRenderer.material.color = Color.white;

        Vector3 controllerPosition = controllerTransform.position;
        Vector3 controllerDirection = controllerTransform.forward;
        Ray ray = new Ray(controllerPosition, controllerDirection);
        string rayStr = "ray: " + ray.origin + ", " + ray.direction + ", ";
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(ray, out raycastHit);
        if (!hit)
        {
            Debug.Log(rayStr + "no hit");
            this.resetTarget();
            return;
        }

        if (raycastHit.collider.tag != "SpawnObject")
        {
            Debug.Log(rayStr + "not spawn object");
            this.resetTarget();
            return;
        }

        targetGameObject = raycastHit.collider.gameObject;
        if (targetGameObject == null)
        {
            Debug.Log(rayStr + "no game object");
            this.resetTarget();
            return;
        }

        targetRenderer = targetGameObject.GetComponent<Renderer>();
        if (targetRenderer == null)
        {
            Debug.Log(rayStr + "no renderer");
            this.resetTarget();
            return;
        }

        targetRigidbody = targetGameObject.GetComponent<Rigidbody>();
        if (targetRigidbody == null)
        {
            Debug.Log(rayStr + "no rigidbody");
            this.resetTarget();
            return;
        }

        targetRenderer.material.color = new Color(0.1f, 0f, 0f, 0.5f);
    }

    private void resetTarget()
    {
        targetGameObject = null;
        targetRenderer = null;
        targetRigidbody = null;
    }
}
