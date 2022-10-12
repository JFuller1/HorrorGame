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

    public string[] dialogSounds = { };

    private void Awake()
    {
        for (int i = 0; i < charSet.Length; i++)
        {
            fontTranslator.Add(charSet[i], font.characters[i]);
        }

        transform.localScale = new Vector2(Camera.main.orthographicSize * 2 * (16f / 9f), textRows);

        Debug.Log(Mathf.FloorToInt(transform.localScale.x));

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
            for (int x = 0; x < Mathf.FloorToInt(transform.localScale.x); x++)
            {
                GameObject newObject = new GameObject(i.ToString());
                i++;
                SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();
                
                renderer.material = mat;

                float xPos = x - (Mathf.FloorToInt(transform.localScale.x)/2f) +0.5f;
                newObject.transform.position = new Vector2(xPos, y - (Camera.main.orthographicSize + 0.5f));
                sprites.Add(renderer);
            }
        }
    }

    public void UpdateText(string text)
    {
        // Play sound if its a letter

        if (System.Char.IsLetter(text[text.Length - 1]))
        {
            TextSound();
        }

        for (int i = 0; i < text.Length; i++)
        {
            Sprite temp;

            if (charSet.Contains(text[i]))
            {
                fontTranslator.TryGetValue(text[i], out temp);
                sprites[i].sprite = temp;
            }
            else
            {
                continue;
            }
        }
    }

    public void TextSound()
    {
        int soundNum = Random.Range(0, dialogSounds.Length);
        FMODUnity.RuntimeManager.PlayOneShot(dialogSounds[soundNum]);
    }

}
