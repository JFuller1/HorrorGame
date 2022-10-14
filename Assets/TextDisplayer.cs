using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TextEffect
{
    public char character;
    public Material effect;
}

public class TextDisplayer : MonoBehaviour
{

    public TextEffect[] textEffects;

    public Material defaultMat;
    public CustomFont font;

    public string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ.,!?";

    public Dictionary<char, Sprite> fontTranslator = new Dictionary<char, Sprite>();

    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    public int textRows = 2;
    int textcolumns;

    public string[] dialogSoundsVowels = { };
    // get rid of this for json
    [SerializeField][Range(-80f, 10f)]
    private float busVolume;
    private float volume;
    FMOD.Studio.Bus dialogueBus;

    private void Awake()
    {
        dialogueBus = FMODUnity.RuntimeManager.GetBus("bus:/Dialogue");
        for (int i = 0; i < charSet.Length; i++)
        {
            fontTranslator.Add(charSet[i], font.characters[i]);
        }

        textcolumns = Mathf.FloorToInt(Camera.main.orthographicSize * 2 * (16f / 9f));

        Setup();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FMODUnity.RuntimeManager.PlayOneShot(dialogSoundsVowels[0]);
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

                    //renderer.material = defaultMat;

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

        formatedText = TextEffects(formatedText);

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
        //replace bus volume with json
        volume = Mathf.Pow(10.0f, busVolume / 20f);
        dialogueBus.setVolume(volume);
        int soundNum = Random.Range(0, dialogSoundsVowels.Length);
        FMODUnity.RuntimeManager.PlayOneShot(dialogSoundsVowels[soundNum]);
    }

    public string TextEffects(string inputString)
    {

        bool effect = false;
        Material mat = defaultMat;

        for (int character = 0; character < inputString.Length; character++)
        {

            for (int efct = 0; efct < textEffects.Length; efct++)
            {

                if (inputString[character] == textEffects[efct].character)
                {

                    if (effect == false)
                    {
                        effect = !effect;
                        mat = textEffects[efct].effect;
                    }
                    else if (effect == true)
                    {
                        effect = !effect;
                        mat = defaultMat;
                    }

                    inputString = inputString.Remove(character, 1);
                    break;

                }                
            }

            if (character <= sprites.Count)
            {
                sprites[character].material = mat;
            }


        }

        return inputString;
    }

    public string ProcessedString(string inputString)
    {
        string outString = "";
        string compoundString = "";
        string tempString = inputString.Trim(); ;

        string[] tempArray;

        tempArray = tempString.Split(' ');

        int specialCount = 0;

        for (int i = 0; i < textEffects.Length; i++)
        {
            specialCount += Mathf.CeilToInt((inputString.Split(textEffects[i].character).Length - 1f) / 2f);
        }

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
                    compoundString = compoundString.PadRight(textcolumns + specialCount, ' ');

                    outString += compoundString;
                    compoundString = str + " ";
                }
            }

            compoundString = compoundString.Trim();
            compoundString = compoundString.PadRight(textcolumns + specialCount, ' ');

            outString += compoundString;

        }

        return outString;
    }
}