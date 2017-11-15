using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Suvat_UiController : MonoBehaviour {

    //stores reference to the UI controller instance
    //Allows it to be accessed anywhere in program
    public static Suvat_UiController instance;

    //Stroes reference to the Particle infomation panel
    public GameObject ParticleInfomationCanvas;
    //Stroes reference to the Particle graphs panel
    public GameObject ParticleGraphCanvas;

    //Suvat input items
    //First character corresponds to Suvat value
    //Second character , after underscore , corresponds to dimention
    public InputField S_x, S_y, S_z;
    public InputField U_x, U_y, U_z;
    public InputField V_x, V_y, V_z;
    public InputField A_x, A_y, A_z;
    public InputField Time;
    public InputField R_x, R_y, R_z;
    public InputField Radius;

    //Time label which contains the current simulation time.
    public Text Label_Time;
    //Reference to the dropbox containing the number of dimentions
    public Dropdown DropBox_Dimentions;
    //Reference to the dropbox containing the particle currently selected
    public Dropdown DropBox_Particle;
    //Reference to the dropbox containing the camera target
    public Dropdown DropBox_CameraTarget;

    //Reference to the slider containing the simulation speed
    public Slider Slider_SimulationSpeed;
    //Reference to the Label containing the simulation speed value on the Slider_SimulationSpeed Slider
    public Text Label_Speed;

    //Reference to Toggle containing whether gravity should be added or not
    //State is true or false
    public Toggle Gravity;


    //unity method ran when the object is first instatiated
    //This occurs when the scene is first loaded in the case of the UIController
    public void Start()
    {
        //Creates reference for all methods to access.
        instance = this;

        //Gives the Camera and Simulate Controllers a reference to the required Dropboxes and Labels,
        CameraController.DropBoxTarget = DropBox_CameraTarget;
        SimulateController.LabelTime = Label_Time;


        //begins program with Particle infomation selected
        //Enabled Particle infomation panel
        //Disables Particle graphs panel
        OnParticleInfomationButtonClicked();
        //Turns all dimention inputs on.
        SetDimention_X(true);
        SetDimention_Y(true);
        SetDimention_Z(true);
    }

    #region Simulation buttons

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
        if (SimulateController.simulationTime == SimulateController.maxTime)
        {
            //increasse time thing
        }
        else
        {
            SimulateController.isSimulating = true;
        }
    }

    //Ran when the SimulationSpeed slider changes value due to user input
    //Must update the Label_Speed text to match current slider value
    public void OnSlider_SimulationSpeedChanged()
    {
        //Rounding to 2 Decimal places
        string value2DP = Slider_SimulationSpeed.value.ToString("n2");
        //Updates the label's text
        Label_Speed.text = "Speed = " + value2DP + "x";
    }
    #endregion

    //Ran when the Calculate button is clicked
    //Begins the calculation proccess
    public void OnCalculateClicked()
    {
        Suvat.OnCalculateClicked();
    }

    //Ran when the reset button is clicked
    public void OnResetClicked()
    {
        ResetUI();
    }

    //Ran when the Simulation button is clicked
    public void OnSimulateClicked()
    {
        //Calculates values required for simulation
        Suvat.OnCalculateClicked();
        //Creates reference in SImulateController to the Speed Slider in UI
        SimulateController.speedInput = Slider_SimulationSpeed;
        //Begins the simulation process in the SimulateController class
        SimulateController.OnSimulateClicked();
    }

    //Ran when Particle infomation is clicked
    //Sets the Particle infomation panel active
    public void OnParticleInfomationButtonClicked()
    {
        ParticleInfomationCanvas.SetActive(true);
        ParticleGraphCanvas.SetActive(false);
    }
    //Ran when Particle graphs is clicked
    //Sets the Particle grapghs panel active
    public void OnParticleGraphButtonClicked()
    {
        ParticleInfomationCanvas.SetActive(false);
        ParticleGraphCanvas.SetActive(true);
    }

    #region Updating dimentions input fields

    //Ran when Dimentions DropBox value is changed
    //Must update the dimentional inputs which are active to the user
    public void OnDropBox_DimentionsChanged()
    {
        //gets current value in Dimentions Dropbox
        int value = DropBox_Dimentions.value;
        switch(value)
        {
            //1 Dimentions selected
            case 0:
                SetDimention_X(true);
                SetDimention_Y(false);
                SetDimention_Z(false);
                //Changes the time input box size and positition to fit the others
                changeFieldSize(0.319f);
                changeFieldPosition(200);
                break;
            //2 Dimentions selected
            case 1:
                SetDimention_X(true);
                SetDimention_Y(true);
                SetDimention_Z(false);
                //Changes the time input box size and positition to fit the others
                changeFieldSize(0.66f);
                changeFieldPosition(261.5f);
                break;
            //3 Dimentions selected
            case 2:
                SetDimention_X(true);
                SetDimention_Y(true);
                SetDimention_Z(true);
                //Changes the time input box size and positition to fit the others
                changeFieldSize(1);
                changeFieldPosition(322);
                break;
        } 
    }
    //Sets input elements in dimention activity to the bool state
    private void SetDimention_X(bool state)
    {
        S_x.gameObject.SetActive(state);
        U_x.gameObject.SetActive(state);
        V_x.gameObject.SetActive(state);
        A_x.gameObject.SetActive(state);
        R_x.gameObject.SetActive(state);
    }
    //Sets input elements in dimention activity to the bool state
    private void SetDimention_Y(bool state)
    {
        S_y.gameObject.SetActive(state);
        U_y.gameObject.SetActive(state);
        V_y.gameObject.SetActive(state);
        A_y.gameObject.SetActive(state);
        R_y.gameObject.SetActive(state);
    }
    //Sets input elements in dimention activity to the bool state
    private void SetDimention_Z(bool state)
    {
        S_z.gameObject.SetActive(state);
        U_z.gameObject.SetActive(state);
        V_z.gameObject.SetActive(state);
        A_z.gameObject.SetActive(state);
        R_z.gameObject.SetActive(state);
    }


    private void changeFieldSize(float size)
    {
        var temp_Time = Time.gameObject.transform.localScale;
        Time.gameObject.transform.localScale = new Vector3(size, temp_Time.y, temp_Time.z);
    }
    private void changeFieldPosition(float x)
    {
        var temp_Time = Time.gameObject.transform.localPosition;
        Time.gameObject.transform.localPosition = new Vector3(x, temp_Time.y, temp_Time.z);
    }
    #endregion


    public void OnDropBox_ParticleChanged()
    {
        int maximum = DropBox_Particle.options.Count;
        int value = DropBox_Particle.value;
        if (value == maximum - 1 && SimulateController.isSimulating == false)
        {
            AddOptionToDropBox(maximum);
        }
        //Updates values depending on the particle selected.
        UpdateValues();
    }

    private void AddOptionToDropBox(int size)
    {
        Dropdown.OptionData[] oldOptions = new Dropdown.OptionData[size + 1];
        for (int i = 0; i < size; i++)
        {
            oldOptions[i] = DropBox_Particle.options[i];
        }
        DropBox_Particle.options.Clear();
        DropBox_CameraTarget.options.Clear();
        string _text = "Free Roam ";
        DropBox_CameraTarget.options.Add(new Dropdown.OptionData() { text = _text });
        for (int i = 0; i < size - 1; i++)
        {
            DropBox_Particle.options.Add(oldOptions[i]);
            DropBox_CameraTarget.options.Add(oldOptions[i]);
        }
        _text = "Particle " + size.ToString();
        DropBox_Particle.options.Add(new Dropdown.OptionData() { text = _text });
        DropBox_CameraTarget.options.Add(new Dropdown.OptionData() { text = _text });

        _text = "Add Particle";
        DropBox_Particle.options.Add(new Dropdown.OptionData() { text = _text });

        DropBox_Particle.value = size - 2;
        DropBox_Particle.value = size - 1;
    }

    private void UpdateValues()
    {
        int current = DropBox_Particle.value;
        try
        {
            UpdateUI(Particle.Instances[current]);
        }
        catch (ArgumentOutOfRangeException e)
        {
            ResetUI();
        }
    }

    public void OnDropBox_CameraTargetChanged()
    {
        int value = DropBox_CameraTarget.value;
        if (value == 0)
        {
            CameraController.isFreeRoam = true;
        }
        else
        {
            CameraController.isFreeRoam = false;
        }
    }

    public void UpdateUI(Particle values)
    {
        S_x.text = values.Displacement[0].ToString();
        S_y.text = values.Displacement[1].ToString();
        S_z.text = values.Displacement[2].ToString();

        U_x.text = values.InitialVelocity[0].ToString();
        U_y.text = values.InitialVelocity[1].ToString();
        U_z.text = values.InitialVelocity[2].ToString();

        V_x.text = values.FinalVelocity[0].ToString();
        V_y.text = values.FinalVelocity[1].ToString();
        V_z.text = values.FinalVelocity[2].ToString();

        A_x.text = values.Acceleration[0].ToString();
        A_y.text = values.Acceleration[1].ToString();
        A_z.text = values.Acceleration[2].ToString();

        Time.text = values.Time.ToString();

        R_x.text = values.InitialPosition[0].ToString();
        R_y.text = values.InitialPosition[1].ToString();
        R_z.text = values.InitialPosition[2].ToString();

        Radius.text = values.Radius.ToString();
    }

    public void ResetUI()
    {
        S_x.text = "";
        S_y.text = "";
        S_z.text = "";

        U_x.text = "";
        U_y.text = "";
        U_z.text = "";

        V_x.text = "";
        V_y.text = "";
        V_z.text = "";

        A_x.text = "";
        A_y.text = "";
        A_z.text = "";

        Time.text = "";

        R_x.text = "";
        R_y.text = "";
        R_z.text = "";

        Radius.text = "";
        Gravity.isOn = false;
    }


}
