using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EyeTrackingRay : MonoBehaviour
{
    [SerializeField]
    private float rayDistance = 1.0f;

    [SerializeField]
    private float rayWidth = 0.01f;

    [SerializeField]
    private LayerMask layersToInclude;

    [SerializeField]
    private Color rayColorDefaultState = Color.yellow;

    [SerializeField]
    private Color rayColorHoverState = Color.red;

    private LineRenderer circleRenderer;

    public List<EyeInteractable> eyeInteractables = new List<EyeInteractable>();

    // Start is called before the first frame update
    void Start()
    {
        circleRenderer = GetComponent<LineRenderer>();
        DrawCircle(100, 0.1f);
    }

    void DrawCircle(int steps, float radius)
    {
        circleRenderer.useWorldSpace = false;
        circleRenderer.positionCount = steps;
        circleRenderer.startColor = rayColorDefaultState;
        circleRenderer.endColor = rayColorDefaultState;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, rayDistance) + transform.position;

            circleRenderer.SetPosition(currentStep, currentPosition);
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 rayCastDirection = transform.TransformDirection(Vector3.forward) * rayDistance;
        if (Physics.Raycast(transform.position, rayCastDirection, out hit, Mathf.Infinity, layersToInclude))
        {
            UnSelect();
            circleRenderer.startColor = rayColorHoverState;
            circleRenderer.endColor = rayColorHoverState;
            var eyeInteractable = hit.transform.GetComponent<EyeInteractable>();
            eyeInteractables.Add(eyeInteractable);
            eyeInteractable.IsHovered = true;
        }
        else
        {
            circleRenderer.startColor = rayColorDefaultState;
            circleRenderer.endColor = rayColorDefaultState;
            UnSelect(true);
        }
    }

    void UnSelect(bool clear = false)
    {
        foreach (var interactable in eyeInteractables)
        {
            interactable.IsHovered = false;
        }
        if (clear)
        {
            eyeInteractables.Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
