using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    Sprite[] playerSprites;

    SpriteRenderer spriteDisplay;

    public const int PLAYER1 = 1;
    public const int PLAYER2 = 2;

    private int currentSprite;
    private bool spriteStatus = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteDisplay = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteStatus)
        {
            spriteDisplay.sprite = playerSprites[currentSprite];
            spriteStatus = false;
        }
    }

    public void SetPlayerImage(int i)
    {
        //Debug.Log(i);
        currentSprite = i - 1;
        spriteStatus = true;
    }

    public void HideSprite()
    {
        spriteDisplay.sprite = null;
    }
}
