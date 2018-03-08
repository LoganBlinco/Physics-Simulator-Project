using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Suvat_UiController : MonoBehaviour {

    #region Variables

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

    //Drop boxes for particle selected and dimention selected when graphing
    public Dropdown GraphDropBoxParticles;
    public Dropdown GraphDropBoxDimention;

    //Database entry field for the address
    public InputField DatabaseInputField;


    #endregion

    //unity method ran when the object is first instatiated
    //This occurs when the scene is first loaded in the case of the UIController
    public void Start()
    {
        //Creates reference for all methods to access.
        instance = this;

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
        newSimulateController.isSimulating = false;
    }

    //Ran when play button is clicked
    //Plays simulation
    public void OnPlayClicked()
    {
        newSimulateController.isSimulating = true;
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

    #region OnClicked

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
        //Begins the simulation process in the SimulateController class
        newSimulateController.isSimulating = true;
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

    #endregion

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
        Time.text = Time.text;
    }
    private void changeFieldPosition(float x)
    {
        var temp_Time = Time.gameObject.transform.localPosition;
        Time.gameObject.transform.localPosition = new Vector3(x, temp_Time.y, temp_Time.z);
    }
    #endregion

    //Ran when the value of DropBox_Particle is changed
    //Must check if add particle has been selected and if so add it
    public void OnDropBox_ParticleChanged()
    {
        //Number of options in dropbox
        int maximum = DropBox_Particle.options.Count;
        //value dropbo has selected
        int value = DropBox_Particle.value;
        //"Add Particle" is the last option
        //therefore maximum -1
        //cannot add a particle while simulation is occuring
        if (value == maximum - 1 && newSimulateController.isSimulating == false)
        {
            //Adds the additional particle to the dropbox's
            AddOptionToDropBox(maximum);
        }
        //Updates values depending on the particle selected.
        UpdateValues();
    }

    //Adds an additional particle to the dropbox
    //option must be added to camera target and particle selections
    private void AddOptionToDropBox(int size)
    {
        //Gets the current option in the particle dropbox.
        Dropdown.OptionData[] oldOptions = new Dropdown.OptionData[size + 1];
        for (int i = 0; i < size; i++)
        {
            oldOptions[i] = DropBox_Particle.options[i];
        }
        //Clears the dropbox's options
        DropBox_Particle.options.Clear();
        DropBox_CameraTarget.options.Clear();
        GraphDropBoxParticles.options.Clear();
        string _text = "Free Roam ";
        //Adds the Free roam option to dropbox particles
        DropBox_CameraTarget.options.Add(new Dropdown.OptionData() { text = _text });
        //Adds previous options to CameraTarget and Particle dropboxes
        for (int i = 0; i < size - 1; i++)
        {
            DropBox_Particle.options.Add(oldOptions[i]);
            DropBox_CameraTarget.options.Add(oldOptions[i]);
            GraphDropBoxParticles.options.Add(oldOptions[i]);
        }
        //Adds additioanl particle to options
        _text = "Particle " + size.ToString();
        DropBox_Particle.options.Add(new Dropdown.OptionData() { text = _text });
        DropBox_CameraTarget.options.Add(new Dropdown.OptionData() { text = _text });
        GraphDropBoxParticles.options.Add(new Dropdown.OptionData() { text = _text });
        //Adds the add particle option to the Particle dropbox 
        _text = "Add Particle";
        DropBox_Particle.options.Add(new Dropdown.OptionData() { text = _text });
        //Selects the added particle
        //A glitch means that the first selection does not occur therefore a dummy selection occurs to fix this 
        DropBox_Particle.value = size - 2;
        DropBox_Particle.value = size - 1;

        DropBox_Particle.RefreshShownValue();
        GraphDropBoxParticles.RefreshShownValue();
    }

    //Updates values in the User interface depending on the particle selected by the user
    private void UpdateValues()
    {
        //Gets the particle index value fromt the dropbox
        int current = DropBox_Particle.value;
        //particle may not be created yet therefore try ,catch exception prevents error.
        try
        {
            //Updates UI using a particles values
            UpdateUI(newParticle.ParticleInstances[current]);
        }
        catch (ArgumentOutOfRangeException)
        {
            //If no particle is found the default values should be used
            ResetUI();
        }
    }

    public void OnDropBox_CameraTargetChanged()
    {
        int value = DropBox_CameraTarget.value;
        if (value == 0)
        {
            newCameraController.isFreeRoam = true;
        }
        else
        {
            newCameraController.isFreeRoam = false;
        }
    }


    //Updates values of the UI with the values inside of the Particle parameter given in each dimention
    public void UpdateUI(newParticle values)
    {
        S_x.text = values.displacement[0].ToString();
        S_y.text = values.displacement[1].ToString();
        S_z.text = values.displacement[2].ToString();

        U_x.text = values.initialVelocity[0].ToString();
        U_y.text = values.initialVelocity[1].ToString();
        U_z.text = values.initialVelocity[2].ToString();

        V_x.text = values.currentVelocity[0].ToString();
        V_y.text = values.currentVelocity[1].ToString();
        V_z.text = values.currentVelocity[2].ToString();

        A_x.text = values.acceleration[0].ToString();
        A_y.text = values.acceleration[1].ToString();
        A_z.text = values.acceleration[2].ToString();

        Time.text = values.motionTime.ToString();

        R_x.text = values.initialPosition[0].ToString();
        R_y.text = values.initialPosition[1].ToString();
        R_z.text = values.initialPosition[2].ToString();

        Radius.text = values.diameter.ToString();
    }

    //Resets particle input fields contents to empty string or default values
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
        //Unchecks the toggle
        Gravity.isOn = false;
    }

    public void OnTextChanged(InputField box)
    {
        try
        {
            string fieldInput = box.text;
            int size = fieldInput.Length;
            char lastChar = fieldInput[size - 1];
            //Checks if the character is a number (0-9) and return true or false
            if (Char.IsNumber(lastChar) || lastChar == '.' || lastChar == '-' || lastChar == '+')
            {
                return;
            }
            else
            {
                //Remove character
                box.text = fieldInput.Substring(0, size - 1);
            }
        }
        catch (IndexOutOfRangeException) { }
    }


}
