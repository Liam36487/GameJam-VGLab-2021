using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppuiInput : CustomInput
{
    public string KeyCode;

    public Image Image;


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
        print(KeyCode + "\t" + textInput.text);
    }

    public void SetImageKeyCode(KeyImagePair keyImagePair)
    {
        SetKeyCode(keyImagePair.Key);
        Image.sprite = keyImagePair.KeyTexture;
    }

    public void Recolor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
