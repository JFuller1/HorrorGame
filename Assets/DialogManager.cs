using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public class Dialog
{    
    public string text;
}

[System.Serializable]
public class DialogStrings {
    public Dialog[] dialog;
}

public class DialogManager : MonoBehaviour
{
    public TextAsset jsonFile;

    DialogStrings dialogStrings = new DialogStrings();

    public TextDisplayer textDisplayer;

    private IEnumerator coroutine;

    private string punctuation = "!?.,;:";

    int currentMessage = 0;

    bool typing = false;

    private void Awake()
    {
        dialogStrings = JsonUtility.FromJson<DialogStrings>(jsonFile.text);

        //second value used to be 0.2
        coroutine = PrintDialog(dialogStrings.dialog[currentMessage].text, 0.1f, 1f);
    }

    private void Start()
    {
        StartCoroutine(coroutine);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(typing)
            {
                StopCoroutine(coroutine);
                textDisplayer.UpdateText(dialogStrings.dialog[currentMessage].text.ToUpper());
                typing = false;
            } else
            {
                currentMessage++;
                // loads the correct message as dialogStrings.dialog[currentMessage].text
                // something like textDisplayer.ResetText(); here
                coroutine = PrintDialog(dialogStrings.dialog[currentMessage].text, 0.1f, 1f);

                StartCoroutine(coroutine);
            }

        }



    }

    IEnumerator PrintDialog(string text, float delay, float punctuationDelay)
    {
        typing = true;
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
        typing = false;

    }

}
