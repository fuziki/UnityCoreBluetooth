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

    [SerializeField]
    private Hakumuchu.HakumuchuController.Hakumuchu3DoFController controller;

    [SerializeField] GameObject prefab;

    private void Awake()
    {
        origin = GetComponent<ARSessionOrigin>();
    }

    [SerializeField]
    private LineRenderer line;
    
    private GameObject targetGameObject = null;
    private Renderer targetRenderer = null;
    private Rigidbody targetRigidbody = null;
    private float targetDistance = -1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (origin.Raycast(Input.GetTouch(0).position, hitResults))
            {
                Instantiate(prefab, hitResults[0].pose.position, Quaternion.identity);
            }
        }

        Vector3 controllerPosition = controllerTransform.position;
        Vector3 controllerDirection = controllerTransform.rotation * Vector3.forward;
        Ray ray = new Ray(controllerPosition, controllerDirection);

        if (controller.touchPad.click)
        {
            this.moveTargetObject(ray);
        }
        else
        {
            this.searchSpawnObject(ray);
        }
    }

    private void moveTargetObject(Ray ray)
    {
        if (targetGameObject == null) return;

        targetRenderer.material.color = new Color(1.0f, 0.0f, 0.0f);

        Vector3 end = ray.GetPoint(targetDistance);
        targetRigidbody.isKinematic = true;
        targetRigidbody.position = end;

        line.SetPosition(0, ray.origin);
        line.SetPosition(1, end);
    }

    private void searchSpawnObject(Ray ray)
    {
        if (targetRenderer != null)
        {
            targetRenderer.material.color = Color.white;
            targetRigidbody.isKinematic = false;
        }

        line.SetPosition(0, ray.origin);
        line.SetPosition(1, ray.GetPoint(1f));
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
        line.SetPosition(1, ray.GetPoint(raycastHit.distance));
        targetDistance = raycastHit.distance;

        targetRenderer = targetGameObject.GetComponent<Renderer>();
        if (targetRenderer == null)
        {
            Debug.Log(rayStr + "no renderer");
            this.resetTarget();
            return;
        }

        targetRigidbody = raycastHit.rigidbody;
        if (targetRigidbody == null)
        {
            Debug.Log(rayStr + "no rigidbody");
            this.resetTarget();
            return;
        }
//        targetRigidbody.AddForce(new Vector3(0f, 20f, 0f), ForceMode.Acceleration);

        targetRenderer.material.color = new Color(1.0f, 0.5f, 0.5f);
    }

    private void resetTarget()
    {
        targetGameObject = null;
        targetRenderer = null;
        targetRigidbody = null;
        targetDistance = -1f;
    }
}
