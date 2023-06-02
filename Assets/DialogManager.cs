using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public class Dialog
{    
    public string text;
    public string voice;
    public float delay;
    public float volume;
    public string color;
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
    static bool engaged = false;
    int num = 76;

    public void TriggerDialogue(TextAsset dialogueFile)
    {
        if (!engaged)
        {

            currentMessage = 0;

            engaged = true;

            dialogStrings = JsonUtility.FromJson<DialogStrings>(dialogueFile.text);

            textDisplayer.InitializeVoice(dialogStrings.dialog[currentMessage].voice, dialogStrings.dialog[currentMessage].volume);

            Color textColor;
            ColorUtility.TryParseHtmlString(dialogStrings.dialog[currentMessage].color, out textColor);
            textDisplayer.matColor = textColor;

            coroutine = PrintDialog(dialogStrings.dialog[currentMessage].text, dialogStrings.dialog[currentMessage].delay, 1f);

            StartCoroutine(coroutine);



        }

    }

    private void Update()
    {

        if (engaged)
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (typing)
                {

                    string text = dialogStrings.dialog[currentMessage].text.ToUpper();

                    string printText = "";

                    StopCoroutine(coroutine);

                    foreach (char character in text)
                    {

                        printText += character;

                        textDisplayer.UpdateText(printText.ToUpper());
                    }

                    typing = false;

                }
                else
                {
                    currentMessage++;
                    

                    if (currentMessage < dialogStrings.dialog.Length)
                    {
                        textDisplayer.InitializeVoice(dialogStrings.dialog[currentMessage].voice, dialogStrings.dialog[currentMessage].volume);

                        Color textColor;
                        ColorUtility.TryParseHtmlString(dialogStrings.dialog[currentMessage].color, out textColor);
                        textDisplayer.matColor = textColor;

                        coroutine = PrintDialog(dialogStrings.dialog[currentMessage].text, dialogStrings.dialog[currentMessage].delay, 1f);

                        StartCoroutine(coroutine);
                    }
                    else
                    {
                        

                        textDisplayer.Clear();

                        Invoke("changeBool", 1);

                    }

                }

            }
        }

    }

    private void changeBool()
    {
        engaged = !engaged;
    }

    IEnumerator PrintDialog(string text, float delay, float punctuationDelay)
    {
        typing = true;
        string printText = $"";

        foreach (char character in text)
        {
            // hi, sincerely Jaeden Fuller
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
