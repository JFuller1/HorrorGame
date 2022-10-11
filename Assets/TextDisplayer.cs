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

}
