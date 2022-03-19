using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColumn : MonoBehaviour
{
    [SerializeField]
    Tile[] tileColumns;

    // Start is called before the first frame update

    int pointer;


    void Start()
    {
        pointer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tile GetTile(int y)
    {
        return tileColumns[y];
    }

    public int GetSize()
    {
        return tileColumns.Length;
    }

    public void PlaceTile(int playerNumber)
    {
        tileColumns[pointer].SetPlayerImage(playerNumber);
        pointer++;
    }

    // only to be used with helper.
    public void SetTile(int loc, int playerNumber)
    {
        tileColumns[loc].SetPlayerImage(playerNumber);
    }

    public void ClearTile(int loc)
    {
        tileColumns[loc].HideSprite();
    }

    public int GetPointer()
    {
        return pointer;
    }
}
