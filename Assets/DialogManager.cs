using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Dialog
{
    public string[,] lines;
}

public class DialogManager : MonoBehaviour
{

    public TextMeshProUGUI textBox;

    private IEnumerator coroutine;

    private void Start()
    {
        coroutine = PrintDialog("Hello. my name is olivier!", 0.1f, 0.2f);
        StartCoroutine(coroutine);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine(coroutine);
            textBox.text = "Hello. my name is olivier!";
        }



    }

    IEnumerator PrintDialog(string text, float delay, float punctuationDelay)
    {

        string printText = "";

        foreach (char character in text)
        {
            printText += character;

            textBox.text = printText;

            if (character == '.' || character == '?' || character == '!' || character == ',')
            {
                yield return new WaitForSeconds(punctuationDelay);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }

        }

    }

}
