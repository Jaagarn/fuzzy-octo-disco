using UnityEngine;

public class ParentSlideController : MonoBehaviour
{
    public enum SlideType {Default, Booster}
    [HideInInspector]
    public float speedModifier = 1.0f;
    [SerializeField]
    private SlideType slideType = SlideType.Default;
    [SerializeField]
    private bool displayRails = true;
    [SerializeField]
    private bool isDisappearing = false;
    [SerializeField]
    private float visibleTimeSeconds = 10.0f;
    [SerializeField]
    private float invisibleTimeSeconds = 4.0f;
    [SerializeField]
    private bool startAsInvisible = false;
    private Material transparentMaterialOverwrite;
    private Material opaqueMaterialOverwrite;
    private float timer = 0.0f;
    private bool isVisible = false;
    private float timeToWait;

    // Broadcast to all children to become visible
    private void Start()
    {
        isVisible = startAsInvisible;

        switch (slideType)
        {
            case SlideType.Booster:
                speedModifier = 2.0f;
                transparentMaterialOverwrite = (Material)Resources.Load("Materials/TransparentBoosterMaterial");
                opaqueMaterialOverwrite = (Material)Resources.Load("Materials/OpaqueBoosterMaterial");
                break;
            default:
                speedModifier = 1.0f;
                break;
        }
        
        BroadcastMaterialSelection(transparentMaterialOverwrite, opaqueMaterialOverwrite);
        transform.BroadcastMessage("DisplayRails", displayRails);

        if(isDisappearing)
        {
            transform.BroadcastMessage(isVisible ? "BecomeInvisible" : "BecomeVisible");
            timeToWait = isVisible ? invisibleTimeSeconds : visibleTimeSeconds;            
        }
        else
        {
            transform.BroadcastMessage("BecomeVisible");
        }
    }

    private void Update()
    {
        if(!isDisappearing)
            return;

        timer += Time.deltaTime;

        if(timer > timeToWait)
        {
           if(isVisible)
           {
                transform.BroadcastMessage("BecomeInvisible");
                isVisible = false;
                timer -= timeToWait;
                timeToWait = invisibleTimeSeconds;
           }
           else
           {
                transform.BroadcastMessage("BecomeVisible");
                isVisible = true;
                timer -= timeToWait;
                timeToWait = visibleTimeSeconds;
           }
        }
    }

    private void BroadcastMaterialSelection(Material transparentMaterial, Material opaqueMaterial)
    {
        transform.BroadcastMessage("SetMaterial", new Material[2] {transparentMaterial, opaqueMaterial});
    }

    // Parent needs to implement methods called by broadcast
    private void BecomeVisible(){}
    private void BecomeInvisible(){}
    private void SetMaterial(Material[] materials){}
    private void DisplayRails(bool displayRails){}
}
