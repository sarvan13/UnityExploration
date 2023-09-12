using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private UnityEngine.UI.Image startingHealth;
    [SerializeField] private UnityEngine.UI.Image currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        startingHealth.fillAmount = playerHealth.currentHealth / 10;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth.fillAmount = playerHealth.currentHealth / 10;
    }
}
