using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplayer : MonoBehaviour
{
    public CustomFont font;

    public string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ.,!?";

    public Dictionary<char, Sprite> fontTranslator = new Dictionary<char, Sprite>();

    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    int textRows = 2;


    private void Start()
    {
        for (int i = 0; i < charSet.Length; i++)
        {
            fontTranslator.Add(charSet[i], font.characters[i]);
        }

        transform.localScale = new Vector2(Camera.main.orthographicSize * 2 * (16f / 9f), textRows);

        Debug.Log(Mathf.FloorToInt(transform.localScale.x));

        Setup();

    }

    public void Setup()
    {
        for (int y = textRows; y > 0; y--)
        {
            for (int x = 0; x < Mathf.FloorToInt(transform.localScale.x); x++)
            {
                int i = 1;
                GameObject newObject = new GameObject(i.ToString());
                i++;
                SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();

                float xPos = x - (Mathf.FloorToInt(transform.localScale.x)/2f) +0.5f;
                newObject.transform.position = new Vector2(xPos, y - (Camera.main.orthographicSize + 0.5f));
                sprites.Add(renderer);
            }
        }
    }

    public void UpdateText(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            

            if (text[i] == ' ')
            {
                continue;
            }
            else
            {
                sprites[i].sprite = fontTranslator[text[i]];
            }
        }
    }

}
