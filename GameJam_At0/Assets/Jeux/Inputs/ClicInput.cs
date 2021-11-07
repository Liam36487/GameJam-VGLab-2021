using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClicInput : CustomInput
{
    [SerializeField] private Button MyButton = null; // assign in the editor
    
    public JeuTir JeuTir;

    void Start()
    {
        MyButton.onClick.AddListener(() => { OnClick(); });
    }

    // Update is called once per frame
    void Update()
    {
        Expiration -= Time.deltaTime;
        if (Expiration <= 0)
        {
            JeuTir.RemoveHitBox(this.gameObject);
            Destroy(gameObject);
        }

    }

    void OnClick()
    {
        JeuTir.CibleCasse(gameObject);
    }
}
