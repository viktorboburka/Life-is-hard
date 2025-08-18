using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DropShadow : MonoBehaviour
{
    public Vector2 ShadowOffset;
    public Material ShadowMaterial;

    private SpriteRenderer spriteRenderer;
    private GameObject shadowGameObject;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Create a new GameObject to be used as drop shadow
        shadowGameObject = new GameObject("Shadow");

        //  Make the shadow a child of this GameObject
        shadowGameObject.transform.SetParent(transform);

        // Create a new SpriteRenderer for shadow GameObject
        SpriteRenderer shadowSpriteRenderer = shadowGameObject.AddComponent<SpriteRenderer>();

        // Set the shadow GameObject's sprite to the original sprite
        shadowSpriteRenderer.sprite = spriteRenderer.sprite;

        // Set the shadow GameObject's material to the shadow material we created
        shadowSpriteRenderer.material = ShadowMaterial;

        // Update the sorting layer of the shadow to always lie behind the sprite
        shadowSpriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    void LateUpdate()
    {
        shadowGameObject.transform.localPosition = ShadowOffset;
        shadowGameObject.transform.localRotation = Quaternion.identity;
        shadowGameObject.transform.localScale = Vector3.one;

        // Ensure shadow stays behind the sprite even if sorting order changes dynamically
        var shadowRenderer = shadowGameObject.GetComponent<SpriteRenderer>();
        shadowRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        shadowRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

}
