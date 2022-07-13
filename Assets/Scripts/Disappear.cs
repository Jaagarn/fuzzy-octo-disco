using System.Collections;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    [SerializeField]
    private float visableTime = 8.0f;
    [SerializeField]
    private float invisibleTime = 2.0f;

    private float visableTimeLongPart;
    private float visableTimeShortPart;
    private float visableTimeShortPartPart;
    private bool mainLoopCompleted = false;
    private Color originalColor;
    private Color halfTransparentColor;
    private Color transparentColor;

    private void OnEnable()
    {
        StateAndLocatizationEventManager.OnReset += ResetEventHandler;
    }

    private void OnDisable()
    {
        StateAndLocatizationEventManager.OnReset -= ResetEventHandler;
    }

    private void Start()
    {
        originalColor = gameObject.GetComponent<Renderer>().material.color;
        transparentColor = halfTransparentColor = originalColor;
        transparentColor.a = 0f;
        halfTransparentColor.a = 0.5f;

        if (visableTime < 4.0f)
            visableTime = 4.0f;

        if (invisibleTime < 2.0f)
            invisibleTime = 2.0f;

        visableTimeLongPart = visableTime * 0.6f;
        visableTimeShortPart = visableTime - visableTimeLongPart;
        visableTimeShortPartPart = visableTimeShortPart / 6;

        StartCoroutine(DoDisapper());
    }

    private void OriginalForm()
    {
        GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(DoBlinkColor(transparentColor, originalColor, 0.1f));
    }

    private void TransparentForm()
    {
        GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Renderer>().material.color = transparentColor;
    }

    private bool IsColorSameAsObjectColor(Color compare)
    {
        return gameObject.GetComponent<Renderer>().material.color.Equals(compare);
    }

    private IEnumerator DoDisapper()
    {
        while (isActiveAndEnabled)
        {
            StartCoroutine(DoMainLoop());
            yield return new WaitUntil(() => mainLoopCompleted);
            mainLoopCompleted = false;
        }
    }

    private IEnumerator DoMainLoop()
    {
        yield return new WaitForSeconds(visableTimeLongPart);

        StartCoroutine(DoBlinkColor(originalColor, halfTransparentColor, visableTimeShortPartPart));
        yield return new WaitUntil(() => IsColorSameAsObjectColor(halfTransparentColor));

        int twoLoops = 0;
        while(twoLoops != 2)
        {
            StartCoroutine(DoBlinkColor(halfTransparentColor, originalColor, visableTimeShortPartPart));
            yield return new WaitUntil(() => IsColorSameAsObjectColor(originalColor));
            StartCoroutine(DoBlinkColor(originalColor, halfTransparentColor, visableTimeShortPartPart));
            yield return new WaitUntil(() => IsColorSameAsObjectColor(halfTransparentColor));
            twoLoops++;
        }

        StartCoroutine(DoBlinkColor(halfTransparentColor, transparentColor, visableTimeShortPartPart));
        yield return new WaitUntil(() => IsColorSameAsObjectColor(transparentColor));

        TransparentForm();
        yield return new WaitForSeconds(invisibleTime);

        OriginalForm();
        yield return new WaitUntil(() => IsColorSameAsObjectColor(originalColor));

        mainLoopCompleted = true;
    }

    private IEnumerator DoBlinkColor(Color fromColor, Color toColor, float blinkSpeed)
    {
        for (float t = 0f; t < blinkSpeed; t += Time.deltaTime)
        {
            float normalizedTime = t / blinkSpeed;

            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(fromColor, toColor, normalizedTime);
            yield return null;
        }
        gameObject.GetComponent<Renderer>().material.color = toColor;
    }

    private void ResetEventHandler()
    {
        StopAllCoroutines();
        OriginalForm();
        mainLoopCompleted = false;
        StartCoroutine(DoDisapper());
    }
}
