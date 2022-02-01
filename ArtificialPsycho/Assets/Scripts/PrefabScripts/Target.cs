using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] private Transform indicator;
    [SerializeField] private GameObject damagePopup;

    public void TakeDamage(float damage)
    {
        GameObject currentText = Instantiate(damagePopup, indicator.position, indicator.rotation);
        TextMesh textMesh = currentText.GetComponent<TextMesh>();

        textMesh.text = damage.ToString();
        Destroy(currentText, 0.3f);
    }
}
