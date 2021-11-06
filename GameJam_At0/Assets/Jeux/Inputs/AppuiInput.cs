using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppuiInput : CustomInput
{
    public string KeyCode;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetKeyCode(string keyCode)
    {
        KeyCode = keyCode;
        Text textInput = gameObject.GetComponentInChildren<Text>();
        textInput.text = KeyCode.ToUpper();
    }
}
