using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public struct TextEffect
{
    public char character;
    public Material effect;
}

public class TextDisplayer : MonoBehaviour
{

    public TextEffect[] textEffects;
    public Dictionary<char,Material> textEffectsDict = new Dictionary<char, Material>();


    public Material defaultMat;
    public CustomFont font;

    public string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ.,!?";

    public Dictionary<char, Sprite> fontTranslator = new Dictionary<char, Sprite>();

    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    public int textRows = 2;
    int textcolumns;


    private string effectCharacters = "";

    bool effect = false;
    Material mat;
    public Color matColor;

    List<string> dialogSoundsVowels = new List<string>();
    char[] sounds = { 'a', 'e', 'o', 'u' };

    private float busVolume;
    private float volume;
    FMOD.Studio.Bus dialogueBus;

    private string outputText = "";

    private void Awake()
    {
        dialogueBus = FMODUnity.RuntimeManager.GetBus("bus:/Dialogue");
        for (int i = 0; i < charSet.Length; i++)
        {
            fontTranslator.Add(charSet[i], font.characters[i]);
        }

        for (int i = 0; i < textEffects.Length; i++)
        {
            textEffectsDict.Add(textEffects[i].character, textEffects[i].effect);
        }

        foreach (TextEffect effect in textEffects)
        {
            effectCharacters += effect.character;
        }

        mat = defaultMat;

        Setup();
    }

    public void Setup()
    {
        //size of letter sprites
        float ppu = font.characters[0].pixelsPerUnit;

        //grid layout group handels sprites for me
        GetComponent<GridLayoutGroup>().cellSize = Vector2.one * ppu;
        
        //spcaing from layout group
        float spacing = GetComponent<GridLayoutGroup>().spacing.x;


        //columns an rows of text
        textcolumns = Mathf.FloorToInt(GetComponent<RectTransform>().rect.width / (ppu + spacing));
        float rows = Mathf.FloorToInt(GetComponent<RectTransform>().rect.height / (ppu + spacing));

        //total amount of characters
        float total = textcolumns * rows;

        //Initialize array of sprites
        for (int i = 0; i < total; i++)
        {
            GameObject newObject = new GameObject(i.ToString(), typeof(RectTransform));

            SpriteRenderer image = newObject.AddComponent<SpriteRenderer>();

            newObject.layer = 5;

            image.sortingLayerName = "Text";
            image.transform.SetParent(transform);

            newObject.transform.localScale = Vector3.one * ppu;

            sprites.Add(image);
        }

    }

    public void Clear()
    {
        //go through each sprite and remove the visible element
        foreach (SpriteRenderer sp in sprites)
        {
            sp.sprite = null;
        }
    }

    public void UpdateText(string text)
    {
        //remove previous text
        Clear();

        //process the string to make them align properly
        string formatedText = TextEffects(ProcessedString(text));

        if (formatedText.Length <= sprites.Count)
        {

            outputText = formatedText;

        }


        for (int i = 0; i < outputText.Length; i++)
        {

                Sprite temp;

                if (charSet.Contains(outputText[i]))
                {
                    fontTranslator.TryGetValue(outputText[i], out temp);
                    sprites[i].sprite = temp;
                }
                else
                {
                    sprites[i].sprite = null;
                    continue;
                }            

        }

        //play sound
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

    /*
    public void TextEffectsWhileTyping(string inputString)
    {
        Debug.Log("typing effects");
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
        */
    public string ProcessedString(string inputString)
    {

        int effectAmount = 0;

        string outString = "";
        string compoundString = "";
        string tempString = inputString.Trim(); ;

        //counts total amount of effects in text
        effectAmount = EffectsInString(tempString);

        if (tempString.Length - effectAmount <= textcolumns)
        {
            outString = tempString;
        }
        else
        {
            //split string into individual words
            string[] tempArray = tempString.Split(' ');

            //go through each word
            foreach (string str in tempArray)
            {
                //gets all effects in current line
                int effectsInWord = EffectsInString(str) + EffectsInString(compoundString);

                //checks if another word could fit
                if (compoundString.Length + (str.Length - effectsInWord) <= textcolumns)
                {
                    effectAmount = 0;

                    compoundString += str + " ";
                }
                else
                {
                    effectsInWord = 0;

                    compoundString = compoundString.Trim();

                    effectAmount = EffectsInString(compoundString);

                    compoundString = compoundString.PadRight(textcolumns + effectAmount, ' ');



                    outString += compoundString;
                    compoundString = str + " ";
                }
            }

            compoundString = compoundString.Trim();

            effectAmount = EffectsInString(compoundString);

            compoundString = compoundString.PadRight(textcolumns + effectAmount, ' ');

            outString += compoundString;

        }

        return outString;
    }

    public string TextEffects(string processedString)
    {

        mat = defaultMat;
        effect = false;
        for (int i = 0; i < processedString.Length; i++)
        {
            if (effectCharacters.Contains(processedString[i]))
            {
                if(effect == false)
                {
                    mat = textEffectsDict[processedString[i]];
                }
                else
                {
                    mat = defaultMat;
                }

                effect = !effect;

                processedString = processedString.Remove(i,1);
            }

            if (i < sprites.Count)
                sprites[i].material = mat;

            mat.SetColor("_Color", matColor);
        }

        return processedString;

    }

    public int EffectsInString(string str)
    {

        int outInt = 0;

        foreach (char character in str)
        {
            if (effectCharacters.Contains(character))
            {
                outInt += 1;
            }
        }

        return outInt;

    }

}