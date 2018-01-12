using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid m_instance;

    private int m_width;
    private int m_height;

    private Color m_startColour;

    public GameObject m_tilePref;
    private Tile[,] m_tiles;

    [SerializeField]
    private List<Tile> m_debugTilesList = new List<Tile>();

    public void CreateGrid(int _width, int _height, Color _startColour)
    {
        CreateInstance();
        m_width = _width;
        m_height = _height;
        m_tiles = new Tile[m_width, m_height];
        m_startColour = _startColour;
    }

    private void CreateInstance()
    {
        if (!m_instance)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public int GetWidth()
    {
        return m_width;
    }

    public int GetHeight()
    {
        return m_height;
    }

    public void CreateTiles()
    {
        GameObject t_parent = new GameObject("Tiles");
        t_parent.transform.position = Vector3.zero;
        t_parent.transform.parent = transform;
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                Vector3 t_pos = new Vector3(x, y, 0f);
                Vector3 t_rot = new Vector3(90f, 0f, 0f);
                GameObject t_tile = (GameObject)Instantiate(m_tilePref, t_pos, Quaternion.identity /*Quaternion.Euler(t_rot)*/);
                //t_tile.transform.parent = t_parent.transform;
                t_tile.AddComponent<BoxCollider2D>();
                Tile t_tileScript = t_tile.GetComponent<Tile>();
                m_tiles[x, y] = t_tileScript;
                m_debugTilesList.Add(t_tileScript);
                t_tileScript.CreateTile(m_startColour);
            }
        }
    }

    public Tile[,] GetTiles()
    {
        return m_tiles;
    }
}