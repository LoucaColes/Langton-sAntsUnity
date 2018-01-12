using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North,
    East,
    South,
    West,
    Count
}

public class Ant : MonoBehaviour
{
    private Transform m_transform;
    private Direction m_direction;
    private Renderer m_renderer;

    public void CreateAnt(Vector3 _position, Direction _direction, Color _color)
    {
        m_transform = GetComponent<Transform>();
        m_transform.position = _position;
        m_direction = _direction;
        m_renderer = GetComponent<Renderer>();
        m_renderer.material.color = _color;
    }

    public void UpdateDirection(Tile _tile)
    {
        TileState t_state = _tile.m_tileState;
        switch (t_state)
        {
            case TileState.L:
                m_direction--;
                if (m_direction < 0)
                {
                    m_direction = Direction.Count - 1;
                }
                break;

            case TileState.R:
                m_direction++;
                if (m_direction >= Direction.Count)
                {
                    m_direction = 0;
                }
                break;

            default:
                Debug.LogError("Invalid Tile State");
                break;
        }
    }

    public void UpdatePosition()
    {
        Vector3 t_pos;
        if (m_direction == Direction.North)
        {
            t_pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (transform.position.y < Grid.m_instance.GetHeight() - 1)
            {
                transform.position = t_pos;
            }
            if (Simulation.m_instance.m_wrap)
            {
                if (transform.position.y >= Grid.m_instance.GetHeight() - 1)
                {
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                }
            }
        }
        else if (m_direction == Direction.East)
        {
            t_pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            if (transform.position.x < Grid.m_instance.GetWidth() - 1)
            {
                transform.position = t_pos;
            }
            if (Simulation.m_instance.m_wrap)
            {
                if (transform.position.x >= Grid.m_instance.GetWidth() - 1)
                {
                    transform.position = new Vector3(0, transform.position.y, transform.position.z);
                }
            }
        }
        else if (m_direction == Direction.South)
        {
            t_pos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            if (transform.position.y > 0)
            {
                transform.position = t_pos;
            }
            if (Simulation.m_instance.m_wrap)
            {
                if (transform.position.y <= 0)
                {
                    transform.position = new Vector3(transform.position.x, Grid.m_instance.GetHeight() - 1, transform.position.z);
                }
            }
        }
        else if (m_direction == Direction.West)
        {
            t_pos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            if (transform.position.x > 0)
            {
                transform.position = t_pos;
            }
            if (Simulation.m_instance.m_wrap)
            {
                if (transform.position.x <= 0)
                {
                    transform.position = new Vector3(Grid.m_instance.GetWidth() - 1, transform.position.y, transform.position.z);
                }
            }
        }
        else
        {
            Debug.LogError("Invalid Direction");
        }
    }

    public Vector3 GetPostion()
    {
        return m_transform.position;
    }

    public int GetPosX()
    {
        return Mathf.RoundToInt(m_transform.position.x);
    }

    public int GetPosZ()
    {
        return Mathf.RoundToInt(m_transform.position.y);
    }
}