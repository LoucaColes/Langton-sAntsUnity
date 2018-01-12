using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    [SerializeField]
    private Color m_currentColour;
    private int m_mask;

    // Use this for initialization
    private void Start()
    {
        m_currentColour = Color.black;
        m_mask = LayerMask.GetMask("Tile");
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
        SwitchColours();
        Draw();
    }

    public string GetCurrentColour()
    {
        if (m_currentColour == Color.white)
        {
            return "white";
        }
        else
        {
            return "black";
        }
    }

    private void Draw()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward, 100);
            Debug.DrawRay(transform.position, Vector3.forward * 25, Color.red, 999);
            if (hit.collider != null)
            {
                print(hit.collider.gameObject);
                if (hit.collider.tag == "Tile")
                {
                    Tile t_tile = hit.collider.gameObject.GetComponent<Tile>();
                    t_tile.PaintTile(m_currentColour);
                }
            }
        }
    }

    private void SwitchColours()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (m_currentColour == Color.black)
            {
                m_currentColour = Color.white;
            }
            else
            {
                m_currentColour = Color.black;
            }
        }
    }
}