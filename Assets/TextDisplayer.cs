using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplayer : MonoBehaviour
{
    public Material mat;
    public CustomFont font;

    public string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ.,!?";

    public Dictionary<char, Sprite> fontTranslator = new Dictionary<char, Sprite>();

    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    int textRows = 2;
    int textcolumns;

    public string[] dialogSounds = { };

    private void Awake()
    {
        for (int i = 0; i < charSet.Length; i++)
        {
            fontTranslator.Add(charSet[i], font.characters[i]);
        }

        textcolumns = Mathf.FloorToInt(Camera.main.orthographicSize * 2 * (16f / 9f));

        //Debug.Log(Mathf.FloorToInt(transform.localScale.x));

        Setup();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FMODUnity.RuntimeManager.PlayOneShot(dialogSounds[0]);
        }
    }

    public void Setup()
    {
        int i = 1;

        for (int y = textRows; y > 0; y--)
        {

            //for (int x = 0; x < Mathf.FloorToInt(transform.localScale.x); x++)
            //{
                for (int x = 0; x < textcolumns; x++)
                {
                    GameObject newObject = new GameObject(i.ToString());
                    i++;
                    SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();

                    renderer.material = mat;

                    float xPos = x - (textcolumns / 2f) + 0.5f;
                    newObject.transform.position = new Vector2(xPos, y - (Camera.main.orthographicSize + 0.5f));
                    sprites.Add(renderer);
                }
        }
    }

    public void UpdateText(string text)
    {

        foreach (SpriteRenderer sp in sprites)
        {
            sp.sprite = null;
        }

        // Play sound if its a letter

        if (System.Char.IsLetter(text[text.Length - 1]))
        {
            TextSound();
        }
        /*
              ______     ____  _             
             |___  /    |  _ \| |            
                / / __ _| |_) | | ___   ___  
               / / / _` |  _ <| |/ _ \ / _ \ 
              / /_| (_| | |_) | | (_) | (_) |
             /_____\__,_|____/|_|\___/ \___/                                  
             put the sound trigger for the text sounds here
        */

        string formatedText = ProcessedString(text);

        //Debug.Log(formatedText);

        for (int i = 0; i < formatedText.Length; i++)
        {
            Sprite temp;

            if (charSet.Contains(formatedText[i]))
            {
                fontTranslator.TryGetValue(formatedText[i], out temp);
                sprites[i].sprite = temp;
            }
            else
            {
                sprites[i].sprite = null;
                continue;
            }
        }
    }

    public void TextSound()
    {
        int soundNum = Random.Range(0, dialogSounds.Length);
        FMODUnity.RuntimeManager.PlayOneShot(dialogSounds[soundNum]);
    }

    public string ProcessedString(string inputString)
    {
        string outString = "";
        string compoundString = "";
        string tempString = inputString.Trim(); ;

        string[] tempArray;

        tempArray = tempString.Split(' ');

        if (tempString.Length <= textcolumns)
        {
            outString = tempString;
        }
        else
        {

            foreach (string str in tempArray)
            {
                if (compoundString.Length + str.Length <= textcolumns)
                {
                    compoundString += str + " ";
                }
                else
                {
                    compoundString = compoundString.Trim();
                    compoundString = compoundString.PadRight(textcolumns, ' ');

                    outString += compoundString;
                    compoundString = str + " ";
                }
            }

            compoundString = compoundString.Trim();
            compoundString = compoundString.PadRight(textcolumns, ' ');

            outString += compoundString;

        }

        return outString;
    }
}