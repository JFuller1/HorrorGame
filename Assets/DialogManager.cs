using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public class Dialog
{    
    public string[] text;

    [Range(0.01f, 2)]
    public float textSpeed, PunctuationSpeed;

    public string[] dialogSounds = { };

}

public class DialogManager : MonoBehaviour
{
    public string text = "test text? is being, typed";

    public TextDisplayer textDisplayer;

    private IEnumerator coroutine;

    private string punctuation = "!?.,;:";

    private void Awake()
    {
        //second value used to be 2
        coroutine = PrintDialog(text, 0.1f, 1f);
    }

    private void Start()
    {
        StartCoroutine(coroutine);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine(coroutine);
            textDisplayer.UpdateText(text.ToUpper());
        }



    }

    IEnumerator PrintDialog(string text, float delay, float punctuationDelay)
    {

        string printText = "";

        foreach (char character in text)
        {
            printText += character;

            textDisplayer.UpdateText(printText.ToUpper());

            if (punctuation.Contains(character))
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
