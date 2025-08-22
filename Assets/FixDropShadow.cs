using UnityEngine;

public class FixDropShadow : MonoBehaviour
{

    SpriteRenderer dropShadow;
    int counter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        dropShadow = GetComponent<DropShadow>().shadowGameObject.GetComponent<SpriteRenderer>();
        if (!dropShadow)
            this.enabled = false;
        if (counter == 0)
        {
            dropShadow.enabled = false;
            dropShadow.gameObject.SetActive(false);
        }
        if (counter == 1)
        {
            dropShadow.enabled = true;
            dropShadow.gameObject.SetActive(true);
            this.enabled = false;
        }
        counter++;
        Debug.Log(counter);
    }
}
