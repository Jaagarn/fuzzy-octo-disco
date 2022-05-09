using UnityEngine;

public class RandomCube : MonoBehaviour
{
    public MeshRenderer Renderer;
    private static bool cooldown = true;
    float randomXAngle;
    float randomYAngle;
    float randomZAngle;

    void Start()
    {
        newCube();
        Invoke("ResetCooldown", 3f);
    }

    void Update()
    {

        transform.Rotate(randomXAngle * Time.deltaTime, randomYAngle * Time.deltaTime, randomZAngle * Time.deltaTime, Space.Self);

        if (cooldown == false)
        {
            newCube();
            Invoke("ResetCooldown", 3f);
            cooldown = true;
        }
    }

    void newCube()
    {
        int randomX = Random.Range(20, 30);
        int randomY = Random.Range(0, 5);
        int randomZ = Random.Range(10, 20);

        transform.position = new Vector3(randomX, randomY, randomZ);

        float randomScaleMultiplier = Random.Range(1f, 4f);

        transform.localScale = Vector3.one * randomScaleMultiplier;

        float randomR = Random.Range(0f, 1f);
        float randomG = Random.Range(0f, 1f);
        float randomB = Random.Range(0f, 1f);
        float randomOpacity = Random.Range(0f, 1f);

        Material material = Renderer.material;

        material.color = new Color(randomR, randomG, randomB, randomOpacity);

        randomXAngle = Random.Range(0f, 90f);
        randomYAngle = Random.Range(0f, 90f);
        randomZAngle = Random.Range(0f, 90f);

    }

    void ResetCooldown()
    {
        cooldown = false;
    }
}
