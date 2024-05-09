using UnityEngine;
using UnityEngine.UI;

public class Paralax : MonoBehaviour
{
    private Image uiImage;
    private Renderer sceneRenderer;

    public float animationSpeed = 1f;

    private void Awake()
    {
        uiImage = GetComponent<Image>();
        sceneRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (uiImage)
        {
            UpdateOffsetUI();
        }
        else if (sceneRenderer)
        {
            UpdateOffsetSceneObject();
        }
    }

    private void UpdateOffsetUI()
    {
        Vector2 currentOffset = uiImage.material.mainTextureOffset;
        float newYOffset = Mathf.Repeat(currentOffset.y + animationSpeed * Time.deltaTime, 1f);
        uiImage.material.mainTextureOffset = new Vector2(0, newYOffset);
    }

    private void UpdateOffsetSceneObject()
    {
        Vector2 currentOffset = sceneRenderer.material.mainTextureOffset;
        float newYOffset = Mathf.Repeat(currentOffset.y + animationSpeed * Time.deltaTime, 1f);
        sceneRenderer.material.mainTextureOffset = new Vector2(0, newYOffset);
    }
}
