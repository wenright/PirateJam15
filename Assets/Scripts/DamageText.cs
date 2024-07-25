using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    public float damage;
    public uint damagerNetId;
    public Damageable damagee;
    public Damageable.DamageType damageType;

    private Dictionary<Damageable.DamageType, Color> damageTypeToColor = new()
    {
        { Damageable.DamageType.DEFAULT, new Color(1, 1, 1) },
        { Damageable.DamageType.CRIT, new Color(218.0f/255, 65.0f/255, 103.0f/255) },
        { Damageable.DamageType.POISON, new Color(97.0f/255, 231.0f/255, 134.0f/255) },
        { Damageable.DamageType.ICE, new Color(32.0f/255, 164.0f/255, 243.0f/255) },
    };

    private float startHeight = 0.5f;
    private float endHeight = 0.5f;
    private float angle = 5f;
    private float variance = 0.75f;
    private float duration = 0.75f;
    // private float minNeighborDistance = 1.0f;
    private Vector3 startPos;

    public void SetDamage(float damage, uint damagerNetId, Damageable damagee, Damageable.DamageType damageType)
    {
        this.damage = damage;
        this.damagerNetId = damagerNetId;
        this.damagee = damagee;
        this.damageType = damageType;

        startPos = transform.position + Vector3.up * startHeight;

        UpdateText();
    }

    public void UpdateDamage(float additionalDamage)
    {
        damage += additionalDamage;

        UpdateText();
    }

    private void UpdateText()
    {
        TextMeshPro textMesh = GetComponent<TextMeshPro>();
        TextMeshPro shadowTextMesh = transform.Find("Shadow").GetComponent<TextMeshPro>();

        string damageText;
        if (damage < 10)
        {
            damageText = (Mathf.Round(damage * 10f) / 10f).ToString();
        }
        else
        {
            damageText = Mathf.RoundToInt(damage).ToString();
        }

        textMesh.text = damageText;
        shadowTextMesh.text = damageText;

        if (damageTypeToColor.TryGetValue(damageType, out var value))
        {
            textMesh.color = value;
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
