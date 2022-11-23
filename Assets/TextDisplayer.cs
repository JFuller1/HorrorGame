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

    bool effect = false;
    Material mat;
    public Color matColor;

    List<string> dialogSoundsVowels = new List<string>();
    char[] sounds = { 'a', 'e', 'o', 'u' };

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

        mat = defaultMat;

        Setup();
    }

    public void Update()
    {

    }

    public void Setup()
    {
        int i = 1;

        for (int y = textRows; y > 0; y--)
        {
                for (int x = 0; x < textcolumns; x++)
                {
                    GameObject newObject = new GameObject(i.ToString());
                    i++;
                    SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();

                    //renderer.material = defaultMat;

                    float xPos = x - (textcolumns / 2f) + 0.5f;
                    newObject.transform.position = new Vector2(xPos, y - (Camera.main.orthographicSize + 0.5f));
                    newObject.transform.parent = Camera.main.transform;

                renderer.sortingLayerName = "Text";

                    sprites.Add(renderer);
                }
        }
    }

    public void Clear()
    {
        foreach (SpriteRenderer sp in sprites)
        {
            sp.sprite = null;
        }
    }

    public void UpdateText(string text)
    {

        Clear();

        // Play sound if its a letter



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

        //TextEffectsWhileTyping(text);

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

        if (System.Char.IsLetter(text[text.Length - 1]))
        {
            TextSound();
        }

    }

    public void InitializeVoice(string voice, float volume)
    {

        dialogSoundsVowels.Clear();

        if (voice != "none")
        {
            busVolume = volume;
            for (int i = 0; i < sounds.Length; i++)
            {
                dialogSoundsVowels.Add($"event:/Dialogue/{voice}/{sounds[i]}");
            }
        }
    }

    public void TextSound()
    {

        if (dialogSoundsVowels.Count != 0)
        {
            volume = Mathf.Pow(10.0f, busVolume / 20f);
            dialogueBus.setVolume(volume);
            int soundNum = Random.Range(0, dialogSoundsVowels.Count);
            FMODUnity.RuntimeManager.PlayOneShot(dialogSoundsVowels[soundNum]);
        }
    }


    public void TextEffectsWhileTyping(string inputString)
    {

        foreach (TextEffect textEffect in textEffects)
        {

            if (textEffect.character == inputString[inputString.Length - 1])
            {
                effect = !effect;

                if (effect)
                {
                    mat = textEffect.effect;
                }
                else
                {
                    mat = defaultMat;
                }
            }

        }

        sprites[inputString.Length - 1].material = mat;
        mat.SetColor("_Color", matColor);

    }

    public void TextEffectsWhileSkipping(string inputString)
    {

        mat = defaultMat;
        effect = false;

        for (int i = 0; i < inputString.Length; i++)
        {
            foreach (TextEffect textEffect in textEffects)
            {

                if (textEffect.character == inputString[i])
                {
                    effect = !effect;

                    if (effect)
                    {
                        mat = textEffect.effect;
                    }
                    else
                    {
                        mat = defaultMat;
                    }
                }

            }

            sprites[i].material = mat;
            mat.SetColor("_Color", matColor);
        }
    }

    public string ProcessedString(string inputString)
    {
        string outString = "";
        string compoundString = "";
        string tempString = inputString.Trim(); ;

        foreach (TextEffect effect in textEffects)
        {
            tempString = tempString.Replace(effect.character.ToString(), "");
        }

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