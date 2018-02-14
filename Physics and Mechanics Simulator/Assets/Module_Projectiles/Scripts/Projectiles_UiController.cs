using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectiles_UiController : MonoBehaviour {

    #region Variables

    public static Projectiles_UiController instance;

    #region Main Panel Variabels

    public GameObject ParticleInfomationPanel;
    public GameObject ParticleGraphPanel;

    #endregion

    #region Simulation Settings Variables

    public Slider sliderSimulationSpeed;
    public Text labelSimulationSpeed;

    public Text LabelSimulationTime;

    public Dropdown dropboxCameraTarget;

    //Particle selection dropbox for graphs
    public Dropdown ParticleGraphDropBox;
    //Particle dimention dropbox for graphs
    public Dropdown ParticleGraphDimention;



    #endregion

    #region Particle Settings Variables

    public InputField InputField_Velocity;
    public InputField InputField_Distance;
    public InputField InputField_ProjectionAngle;
    public InputField InputField_SlopeAngle;
    public InputField InputField_Duration;

    public Slider Slider_Restitution;
    public Text Label_Restitution;

    #endregion

    #endregion

    public void Start()
    {
        instance = this;
        CameraController.DropBoxTarget = dropboxCameraTarget;
        //Select Particle infomation
        ParticleInfomationPanel.SetActive(true);
        ParticleGraphPanel.SetActive(false);

    }


    //Ran when the SimulationSpeed slider changes value due to user input
    //Must update the Label_Speed text to match current slider value
    public void OnSlider_SimulationSpeedChanged()
    {
        //Rounding to 2 Decimal places
        string value2DP = sliderSimulationSpeed.value.ToString("n2");
        //Updates the label's text
        labelSimulationSpeed.text = "Speed = " + value2DP + "x";
    }

    //Ran when pause button is clicked
    //Pauses simulation
    public void OnPauseClicked()
    {
        SimulateController.isSimulating = false;
    }

    //Ran when play button is clicked
    //Plays simulation
    public void OnPlayClicked()
    {
        SimulateController.isSimulating = true;
    }


    //Selects the particle infomation panel
    public void OnParticleInfomationClicked()
    {
        ParticleInfomationPanel.SetActive(true);
        ParticleGraphPanel.SetActive(false);
    }
    //Selects the graphing panel
    public void OnParticleGraphClicked()
    {
        ParticleInfomationPanel.SetActive(false);
        ParticleGraphPanel.SetActive(true);
    }

    //Ran when the SimulationSpeed slider changes value due to user input
    //Must update the Label_Speed text to match current slider value
    public void OnSlider_RestitutionChanged()
    {
        //Rounding to 2 Decimal places
        string value2DP = Slider_Restitution.value.ToString("n2");
        //Updates the label's text
        Label_Restitution.text = value2DP;
    }

    //Ran when reset button is clicked
    public void OnResetClicked()
    {
        ResetUI();
    }
    //Ran when the Simulation button is clicked
    public void OnSimulateClicked()
    {
        //Calculates values required for simulation
        Projectiles.OnCalculateClicked();
        //Creates reference in SImulateController to the Speed Slider in UI
        SimulateController.speedInput = sliderSimulationSpeed;
        //Reference between time updating label for the simulation
        SimulateController.LabelTime = LabelSimulationTime;

        SimulateController.GraphDropBoxParticles = ParticleGraphDropBox;
        SimulateController.GraphDropBoxDimention = ParticleGraphDimention;
        //Begins the simulation process in the SimulateController class
        SimulateController.OnSimulateClicked();
    }


    private void ResetUI()
    {
        InputField_Distance.text = "";
        InputField_Duration.text = "";
        InputField_ProjectionAngle.text = "";
        InputField_SlopeAngle.text = "";
        InputField_Velocity.text = "";
        Slider_Restitution.value = 1;
    }

    public void UpdateUi(Particle values)
    {
        float Distance = values.Displacement.magnitude;
        InputField_Distance.text = Distance.ToString();
        InputField_Duration.text = values.Time.ToString();
    }

    public void OnDropBox_CameraTargetChanged()
    {
        int value = dropboxCameraTarget.value;
        if (value == 0)
        {
            CameraController.isFreeRoam = true;
        }
        else
        {
            CameraController.isFreeRoam = false;
        }
    }



    //Stops non numbers or "-" and "+" symbols from being inputted
    public void OnTextChanged(InputField box)
    {
        try
        {
            string fieldInput = box.text;
            int size = fieldInput.Length;
            char lastChar = fieldInput[size - 1];
            //Checks if the character is a number (0-9) or + or - and return true or false
            if (System.Char.IsNumber(lastChar) || lastChar == '.' || lastChar == '-' || lastChar == '+')
            {
                return;
            }
            else
            {
                //Remove character
                box.text = fieldInput.Substring(0, size - 1);
            }
        }
        //Catch occurs when backspace occurs
        catch (System.IndexOutOfRangeException)
        {

        }
    }

}
