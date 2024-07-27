using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    [ReadOnly] public float damage;
    [ReadOnly] public Damageable damagee;
    [ReadOnly] public Damageable.DamageType damageType;
    public TextMeshPro text;
    public Vector3 startPos;

    private readonly Dictionary<Damageable.DamageType, Color> damageTypeToColor = new()
    {
        { Damageable.DamageType.DEFAULT, new Color(49/255.0f, 44/255.0f, 58/255.0f) },
        { Damageable.DamageType.CRIT, new Color(218/255.0f, 65/255.0f, 103/255.0f) },
        { Damageable.DamageType.FIRE, new Color(255/255.0f, 65/255.0f, 103/255.0f) },
        { Damageable.DamageType.POISON, new Color(97/255.0f, 231/255.0f, 134/255.0f) },
        { Damageable.DamageType.ICE, new Color(32/255.0f, 164/255.0f, 243/255.0f) },
    };

    private float startHeight = 1.0f;
    private float endHeight = 0.5f;
    private float angle = 5f;
    private float variance = 0.75f;
    private float duration = 0.75f;
    private float dotFontSize = 10;

    public void SetDamage(float damage, Damageable damagee, Damageable.DamageType damageType)
    {
        this.damage = damage;
        this.damagee = damagee;
        this.damageType = damageType;

        startPos = transform.position + Vector3.up * startHeight;

        RefreshText();
    }

    public void UpdateDamage(float additionalDamage, Vector3 position)
    {
        damage += additionalDamage;

        startPos = position + Vector3.up * startHeight;

        RefreshText();
    }

    public void RefreshText(bool roundNumber = true)
    {
        // Show tenths decimal place if < 10
        if (roundNumber)
        {
            string damageText;
            if (damage < 10)
            {
                damageText = (Mathf.Round(damage * 10f) / 10f).ToString();
            }
            else
            {
                damageText = Mathf.RoundToInt(damage).ToString();
            }
            
            text.text = damageText;
        }


        if (damageTypeToColor.TryGetValue(damageType, out var value))
        {
            text.color = value;
        }

        // DOT damage should have a smaller font size
        if (damageType != Damageable.DamageType.DEFAULT)
        {
            text.fontSize = dotFontSize;
        }

        Vector3 endPos = startPos + new Vector3(0, endHeight + (variance * Random.Range(-0.5f, 0.5f)), 0);

        transform.DOKill();

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-angle, angle) * variance);
        transform.DORotate(new Vector3(0, 0, Random.Range(-angle, angle) * variance), duration).SetEase(Ease.OutQuad);

        transform.localScale = Vector3.one * 0.75f;
        transform.DOScale(Vector3.one, duration).SetEase(Ease.OutQuad);

        transform.position = startPos;
        transform.DOMove(endPos, duration).SetEase(Ease.OutQuad).onComplete += () =>
        {
            Destroy(gameObject);
        };
    }
}
