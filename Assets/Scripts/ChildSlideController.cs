using UnityEngine;

public class ChildSlideController : MonoBehaviour
{
    private Color originalColor;
    private Color transparentColor;
    private Material transparentMaterial;
    private Material opaqueMaterial;

    // Called by broadcasting from parent
    private void SetMaterial(Material[] materials)
    {
        transparentMaterial = materials[0] == null ? (Material)Resources.Load("Materials/TransparentSlide") : materials[0];
        opaqueMaterial = materials[1] == null ? (Material)Resources.Load("Materials/OpaqueSlide") : materials[1];
        originalColor = opaqueMaterial.color;
        transparentColor = transparentMaterial.color;
        transparentColor.a = 0f;
    }

    // Called by broadcasting from parent
    private void BecomeVisible()
    {
        GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<Renderer>().material = opaqueMaterial;
        gameObject.GetComponent<Renderer>().material.color = originalColor;
    }

    // Called by broadcasting from parent
    private void BecomeInvisible()
    {
        GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Renderer>().material = transparentMaterial;
        gameObject.GetComponent<Renderer>().material.color = transparentColor;
    }
}
