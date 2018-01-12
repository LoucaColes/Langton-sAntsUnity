using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState
{
    L,
    R
}

public class Tile : MonoBehaviour
{
    public TileState m_tileState;
    private Renderer m_renderer;
    public Color m_colour;

    public void CreateTile(TileState _state)
    {
        m_tileState = _state;
        m_renderer = GetComponent<Renderer>();
        switch (_state)
        {
            case TileState.L:
                m_colour = Color.black;
                break;

            case TileState.R:
                m_colour = Color.white;
                break;

            default:
                Debug.LogError("Invalid State");
                break;
        }
        m_renderer.material.color = m_colour;
    }

    public void CreateTile(Color _color)
    {
        m_colour = _color;
        m_renderer = GetComponent<Renderer>();
        m_renderer.material.color = m_colour;

        if (m_colour == Color.black)
        {
            m_tileState = TileState.L;
        }
        else if (m_colour == Color.white)
        {
            m_tileState = TileState.R;
        }
        else
        {
            Debug.LogError("Invalid Colour");
        }
    }

    public void UpdateTile()
    {
        switch (m_tileState)
        {
            case TileState.L:
                m_tileState = TileState.R;
                m_colour = Color.white;
                m_renderer.material.color = m_colour;
                break;

            case TileState.R:
                m_tileState = TileState.L;
                m_colour = Color.black;
                m_renderer.material.color = m_colour;
                break;
        }
    }

    public void PaintTile(Color _colour)
    {
        if (_colour == Color.white)
        {
            m_tileState = TileState.R;
            m_colour = Color.white;
            m_renderer.material.color = m_colour;
        }
        else if (_colour == Color.black)
        {
            m_tileState = TileState.L;
            m_colour = Color.black;
            m_renderer.material.color = m_colour;
        }
    }
}