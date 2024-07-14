using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemies : MonoBehaviour
{
    // Start is called before the first frame update
    public Image fillImage;
    public GlassKnight gk;
    public SouledShroom ss;
    public OASIS oa;

    private void Start()
    {
        if (gk == null)
        {
            gk = GetComponentInParent<GlassKnight>();
        }
        if (ss == null)
        {
            ss = GetComponentInParent<SouledShroom>();
        }
        if (oa == null)
        {
            oa = GetComponentInParent<OASIS>();
        }
    }

    private void Update()
    {
        fillImage.fillAmount = ss.currentHealth / ss.maxHealth;
        fillImage.fillAmount = gk.currentHealth / gk.maxHealth;
        fillImage.fillAmount = oa.currentHealth / oa.maxHealth;
    }
}
