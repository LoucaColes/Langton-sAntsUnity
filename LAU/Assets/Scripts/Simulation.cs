using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SimulationState
{
    Start,
    Setup,
    Draw,
    Simulate,
    Pause
}

public class Simulation : MonoBehaviour
{
    public static Simulation m_instance;

    public GameObject m_gridPref;
    public GameObject m_antPref;
    public Pen m_pen;

    [Header("Input")]
    public int m_antCount;
    public bool m_wrap;
    private int m_gridWidth;
    private int m_gridHeight;
    private Color m_startColour;
    private float m_delay;

    private Grid m_grid;
    private List<Ant> m_ants = new List<Ant>();

    private int m_turnCount;

    private SimulationState m_state;
    private bool m_drew;

    // Use this for initialization
    private void Start()
    {
        CreateInstance();
        SetTurnCount(0);
        m_state = SimulationState.Start;
        m_drew = false;
        m_pen.enabled = false;
        m_pen.gameObject.SetActive(false);
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

    private void SetTurnCount(int _value)
    {
        m_turnCount = _value;
    }

    public void IncreaseTurnCount()
    {
        m_turnCount++;
    }

    public int GetTurnCount()
    {
        return m_turnCount;
    }

    public void UpdateSimState(SimulationState _state)
    {
        m_state = _state;
        switch (m_state)
        {
            case SimulationState.Setup:
                CreateGrid();
                CreateAnts();
                break;

            case SimulationState.Draw:
                if (!m_pen.enabled)
                {
                    Vector3 t_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    m_pen.transform.position = new Vector3(t_mousePos.x, t_mousePos.y, -1);
                    m_pen.enabled = true;
                    m_pen.gameObject.SetActive(true);
                    //Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;
                    m_drew = true;
                }
                break;

            case SimulationState.Simulate:
                InvokeRepeating("Simulate", 1f, m_delay);
                if (Cursor.lockState == CursorLockMode.Confined)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                if (m_drew)
                {
                    m_pen.enabled = false;
                    m_pen.gameObject.SetActive(false);
                    m_drew = false;
                }
                break;

            case SimulationState.Pause:
                CancelInvoke();
                break;
        }
    }

    public SimulationState GetState()
    {
        return m_state;
    }

    public void SetInputData(int _width, int _height, Color _startColour, float _delay, bool _wrap)
    {
        m_gridWidth = _width;
        m_gridHeight = _height;
        if (!m_drew)
        {
            m_startColour = _startColour;
        }
        m_delay = _delay;
        m_wrap = _wrap;
        CameraSetUp();
    }

    private void CameraSetUp()
    {
        Camera.main.transform.position = new Vector3(m_gridWidth / 2, m_gridHeight / 2, -((m_gridWidth / 2) + (m_gridHeight / 2)));
    }

    private void CreateGrid()
    {
        GameObject t_grid = (GameObject)Instantiate(m_gridPref, Vector3.zero, Quaternion.identity);
        m_grid = t_grid.GetComponent<Grid>();
        m_grid.CreateGrid(m_gridWidth, m_gridHeight, m_startColour);
        m_grid.CreateTiles();
    }

    private void CreateAnts()
    {
        for (int i = 0; i < m_antCount; i++)
        {
            Vector3 t_pos = new Vector3(m_gridWidth / 2, m_gridHeight / 2, -1);
            GameObject t_ant = (GameObject)Instantiate(m_antPref, Vector3.zero, Quaternion.identity);
            Ant t_antScript = t_ant.GetComponent<Ant>();
            t_antScript.CreateAnt(t_pos, Direction.North, Color.red);
            m_ants.Add(t_antScript);
        }
    }

    private void Simulate()
    {
        IncreaseTurnCount();
        for (int i = 0; i < m_ants.Count; i++)
        {
            int t_x = (int)m_ants[i].transform.position.x;
            int t_y = (int)m_ants[i].transform.position.y;
            Tile t_tile = Grid.m_instance.GetTiles()[t_x, t_y];
            Grid.m_instance.GetTiles()[t_x, t_y].UpdateTile();
            m_ants[i].UpdateDirection(t_tile);
            m_ants[i].UpdatePosition();
        }
    }
}