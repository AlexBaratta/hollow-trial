using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Vector2 size = new Vector2(1f, 0.15f);
    public float padding = 0.1f;
    public float border = 0.06f;
    public Color borderColor = Color.black;
    public Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    public Color fillColor = Color.red;

    private Transform fillTransform;
    private float maxHealth;

    void Awake()
    {
        SpriteRenderer parentSprite = GetComponentInParent<SpriteRenderer>();
        float yOffset = parentSprite != null
            ? -parentSprite.bounds.extents.y - padding
            : -padding;
        transform.localPosition = new Vector3(0f, yOffset, 0f);

        SpriteRenderer borderSr = CreateBar("HealthBar_Border", borderColor, 0);
        borderSr.transform.localScale = new Vector3(size.x + border * 2f, size.y + border * 2f, 1f);

        SpriteRenderer bg = CreateBar("HealthBar_BG", backgroundColor, 1);
        bg.transform.localScale = new Vector3(size.x, size.y, 1f);

        GameObject fillObj = CreateBar("HealthBar_Fill", fillColor, 2).gameObject;
        fillTransform = fillObj.transform;
        fillTransform.localScale = new Vector3(size.x, size.y, 1f);
    }

    public void Bind(PlayerHealth health)
    {
        maxHealth = health.maxHealth;
        health.OnHealthChanged += UpdateBar;
    }

    public void Bind(MobHealth health)
    {
        maxHealth = health.maxHealth;
        health.OnHealthChanged += UpdateBar;
    }

    private void UpdateBar(int current, int max)
    {
        float ratio = Mathf.Clamp01((float)current / max);

        fillTransform.localScale = new Vector3(size.x * ratio, size.y, 1f);
        fillTransform.localPosition = new Vector3((size.x * ratio - size.x) * 0.5f, 0f, 0f);
    }

    SpriteRenderer CreateBar(string name, Color color, int order)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(transform, false);
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = MakeWhitePixelSprite();
        sr.color = color;
        sr.sortingOrder = order;
        return sr;
    }

    void OnDrawGizmosSelected()
    {
        SpriteRenderer parentSprite = GetComponentInParent<SpriteRenderer>();
        float yOffset = parentSprite != null
            ? -parentSprite.bounds.extents.y - padding
            : -padding;
        Vector3 center = transform.parent != null
            ? transform.parent.position + new Vector3(0f, yOffset, 0f)
            : transform.position;

        Gizmos.color = borderColor;
        Gizmos.DrawCube(center, new Vector3(size.x + border * 2f, size.y + border * 2f, 0f));

        Gizmos.color = backgroundColor;
        Gizmos.DrawCube(center, new Vector3(size.x, size.y, 0f));

        Gizmos.color = fillColor;
        Gizmos.DrawCube(center, new Vector3(size.x, size.y, 0f));
    }

    static Sprite cachedSprite;

    static Sprite MakeWhitePixelSprite()
    {
        if (cachedSprite != null) return cachedSprite;

        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        cachedSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        return cachedSprite;
    }
}
