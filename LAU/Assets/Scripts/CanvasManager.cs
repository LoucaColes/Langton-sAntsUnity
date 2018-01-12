using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Input")]
    public Canvas m_inputCanvas;
    public Slider m_widthSlider;
    public Text m_widthSliderText;
    public Slider m_heightSlider;
    public Text m_heightSliderText;
    public Slider m_delaySlider;
    public Text m_delaySliderText;
    public Toggle m_wrapToggle;

    [Header("Set Up")]
    public Canvas m_setUpCanvas;

    [Header("Draw")]
    public Canvas m_drawCanvas;
    public Text m_currentColourText;

    [Header("Simulate")]
    public Canvas m_simulateCanvas;
    public Text m_turnText;
    public Text m_pauseButtonText;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        KeyboardInput();
        CanvasesUpdate();
    }

    private void KeyboardInput()
    {
        switch (Simulation.m_instance.GetState())
        {
            case SimulationState.Start:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CompleteSetUp();
                }
                break;

            case SimulationState.Setup:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartSim();
                }
                break;
        }
    }

    private void CanvasesUpdate()
    {
        switch (Simulation.m_instance.GetState())
        {
            case SimulationState.Start:
                m_inputCanvas.gameObject.SetActive(true);

                m_widthSliderText.text = m_widthSlider.value.ToString();
                m_heightSliderText.text = m_heightSlider.value.ToString();
                m_delaySliderText.text = m_delaySlider.value.ToString("F2");

                m_setUpCanvas.gameObject.SetActive(false);
                m_drawCanvas.gameObject.SetActive(false);
                m_simulateCanvas.gameObject.SetActive(false);
                break;

            case SimulationState.Setup:
                m_inputCanvas.gameObject.SetActive(false);
                m_setUpCanvas.gameObject.SetActive(true);
                m_drawCanvas.gameObject.SetActive(false);
                m_simulateCanvas.gameObject.SetActive(false);
                break;

            case SimulationState.Draw:
                m_inputCanvas.gameObject.SetActive(false);
                m_setUpCanvas.gameObject.SetActive(false);
                m_drawCanvas.gameObject.SetActive(true);
                m_simulateCanvas.gameObject.SetActive(false);

                m_currentColourText.text = "Current Colour: " + Simulation.m_instance.m_pen.GetCurrentColour().ToString();
                break;

            case SimulationState.Simulate:
                m_inputCanvas.gameObject.SetActive(false);
                m_setUpCanvas.gameObject.SetActive(false);
                m_drawCanvas.gameObject.SetActive(false);
                m_simulateCanvas.gameObject.SetActive(true);

                m_turnText.text = "Turn: " + Simulation.m_instance.GetTurnCount();
                m_pauseButtonText.text = "Pause";
                break;

            case SimulationState.Pause:
                m_inputCanvas.gameObject.SetActive(false);
                m_setUpCanvas.gameObject.SetActive(false);
                m_drawCanvas.gameObject.SetActive(false);
                m_simulateCanvas.gameObject.SetActive(true);

                m_turnText.text = "Turn: " + Simulation.m_instance.GetTurnCount();
                m_pauseButtonText.text = "Play";
                break;
        }
    }

    public void CompleteSetUp()
    {
        Simulation.m_instance.SetInputData((int)m_widthSlider.value, (int)m_heightSlider.value, Color.white, m_delaySlider.value, m_wrapToggle.isOn);
        Simulation.m_instance.UpdateSimState(SimulationState.Setup);
    }

    public void StartDrawing()
    {
        Simulation.m_instance.UpdateSimState(SimulationState.Draw);
    }

    public void StartSim()
    {
        Simulation.m_instance.UpdateSimState(SimulationState.Simulate);
    }

    public void PauseUnpause()
    {
        if (Simulation.m_instance.GetState() == SimulationState.Simulate)
        {
            Simulation.m_instance.UpdateSimState(SimulationState.Pause);
        }
        else if (Simulation.m_instance.GetState() == SimulationState.Pause)
        {
            Simulation.m_instance.UpdateSimState(SimulationState.Simulate);
        }
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}