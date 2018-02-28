using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Gravity_InputController : MonoBehaviour {

    //Stores refernce to the UI controller allowing external classes to access the UI
    public static Gravity_InputController Instance;

    //Stores current particle selected for particle infomation
    public int ParticleIndexSelected =0;

    #region Simulation References
    //Reference to the dropbox storing the Camera target
    public Dropdown CameraTarget;
    //Label displaying simulation time
    public Text Label_SimulationTime;
    //Slider storing the simulation speed multiplier
    public Slider Slider_SimulationSpeed;
    //Label showing the simulatio speed
    public Text Label_SimulationSpeed;

    #endregion

    #region Particle Infomation references
    //References to Input field for velocity in X and Y dimention
    public InputField Inputfield_VelocityX;
    public InputField Inputfield_VelocityY;
    //Slider for mass input
    public Slider Slider_Mass;
    //Label for displaying Slider value
    public Text Label_Mass;
    //Slider for diameter input
    public Slider Slider_Diameter;
    //Label for displaying diameter value
    public Text Label_Diameter;

    //Dropbox containing selectable planets or to create a new planet
    public Dropdown DropBoxPlanet;
    #endregion

    #region Panels

    public GameObject PanelParticleInfomation;
    public GameObject PanelParticleGraph;
    public GameObject PanelAboutMenu;

    public void OnPanelParticleInfomationClicked()
    {
        PanelParticleInfomation.SetActive(true);
        PanelParticleGraph.SetActive(false);
        PanelAboutMenu.SetActive(false);
    }
    public void OnPanelParticleGraphClicked()
    {
        PanelParticleInfomation.SetActive(false);
        PanelParticleGraph.SetActive(true);
        PanelAboutMenu.SetActive(false);
    }
    //When about is clicked the about panel should be loaded
    public void OnAboutClicked()
    {
        PanelAboutMenu.SetActive(true);
        PanelParticleInfomation.SetActive(false);
        PanelParticleGraph.SetActive(false);
    }
    //When ok is clicked the Particle infomation panel should be shown and others hidden
    public void OnAbout_OkClicked()
    {
        OnPanelParticleInfomationClicked();
    }


    #endregion

    #region Graph references

    public GameObject GraphAcceleration;
    public GameObject GraphSpeed;

    public Dropdown DropBoxGraphTarget;
    #endregion

    #region Simulation Controls
    //Resets the scene
    public void OnResetClicked()
    {
        //Resets the static variables for simulations
        GravitySimulationController.isSimulating = false;
        GravitySimulationController.SimulationSpeed = 1;
        GravityPlanets.PlanetInstances.Clear();
        //Loads scene to refresh values
        SceneManager.LoadScene("GravityScene");
    }


    //Called when the camera dropbox's value is changed
    public void OnDropBoxCameraTargetChanged()
    {
        //Current index of item selected
        int value = CameraTarget.value;
        //Index 0 is the free roam option
        if (value == 0)
        {
            GravityCameraController.isFreeRoam = true;
        }
        else
        {
            GravityCameraController.isFreeRoam = false;
        }
    }

    //Ran when play button clicked
    public void OnPlayClicked()
    {
        GravitySimulationController.isSimulating = true;
    }
    //Ran when pause button clicked
    public void OnPauseClicked()
    {
        GravitySimulationController.isSimulating = false;
    }
    //Ran when simulation speed slider is changed
    //Must update simulation speed 
    public void OnSliderSimulationSpeedChanged()
    {
        //Sets value
        GravitySimulationController.SimulationSpeed = Slider_SimulationSpeed.value;
        //Updates Label value to 2 DP
        string value2DP = Slider_SimulationSpeed.value.ToString("n2");
        Label_SimulationSpeed.text = "Speed = " + value2DP + "x";
    }
    #endregion

    #region DropBox updates
    //Ran when the value of DropBox_Particle is changed
    //Must check if add particle has been selected and if so add it
    public void OnDropBoxParticleChanged()
    {
        //Max number of options in dropbox
        int maximum = DropBoxPlanet.options.Count;
        int currentValue = DropBoxPlanet.value;
        //If the last option is selected , which is "Add Particle" then a particle must be created
        if (currentValue == maximum - 1)
        {
            //Adds additional particle to dropbox
            AddOptionToDropBox(maximum);
            //Creates new particle
            GravityPlanets.PlanetInstances.Add(new GravityPlanets());
        }
        ParticleIndexSelected = DropBoxPlanet.value;
        //Updates the UI 
        UpdateValues();
    }
    //Adds another particle option to the dropbox and upates the "Add particle" option
    private void AddOptionToDropBox(int size)
    {
        string _text = "Particle " + (size).ToString();
        //Adds additonal particle to the camera target and planet selection dropbox
        DropBoxPlanet.options[size - 1] = new Dropdown.OptionData() { text = _text };
        CameraTarget.options.Add(new Dropdown.OptionData() { text = _text });
        DropBoxGraphTarget.options.Add(new Dropdown.OptionData() { text = _text });
        _text = "Add Particle";
        //Option to add particle added at end of list
        DropBoxPlanet.options.Add(new Dropdown.OptionData() { text = _text });
        //Updates option selected
        DropBoxPlanet.value = size - 1;
        DropBoxPlanet.RefreshShownValue();
    }

    //Updates values in the User interface depending on the particle selected by the user
    private void UpdateValues()
    {
        //Gets the particle index value fromt the dropbox
        int current = DropBoxPlanet.value;
        //particle may not be created yet therefore try ,catch exception prevents error.
        try
        {
            //Updates UI using a particles values
            UpdateUI(GravityPlanets.PlanetInstances[current]);
        }
        catch (ArgumentOutOfRangeException)
        {
            //If no particle is found the default values should be used
            ResetUI();
        }
    }
    #endregion

    #region Particle Slider Updates
    //When slider is changed must update label and current particles value
    public void OnMassSliderChanged()
    {
        //Updates Label
        OnSliderChanged(Slider_Mass, Label_Mass);
        GravityPlanets.PlanetInstances [ParticleIndexSelected].mass = Slider_Mass.value;
    }
    //When slider is changed must update label and current particles value
    public void OnRadiusChanged()
    {
        //Updates Label
        OnSliderChanged(Slider_Diameter, Label_Diameter);
        GravityPlanets.PlanetInstances[ParticleIndexSelected].diameter = Slider_Diameter.value;
    }
    //Updates the labels text to store the value of slider rounded to 2 D.P
    public void OnSliderChanged(Slider sliderChanged, Text LabelToUpdate)
    {
        //Rounding to 2 Decimal places
        string value2DP = sliderChanged.value.ToString("n2");
        //Updates the label's text
        LabelToUpdate.text = value2DP;
    }

    #endregion

    #region Start methods

    //When the scene is first loaded the first particle should be in the scene ready for manipulation
    public void Start()
    {
        Debug.Log("Scnee2);");
        Instance = this;
        //Generates the first planet in the scene
        CreateFirstObject();
        OnPanelParticleInfomationClicked();
    }
    //Creates a planet to be centered in the screen when scene loads
    private void CreateFirstObject()
    {
        if (GravityPlanets.PlanetInstances.Count == 0)
        {
            //Assigns default values to the particle
            GravityPlanets newParticle = new GravityPlanets();
            newParticle.initialVelocity = Vector3.zero;
            newParticle.mass = 1.0f;
            newParticle.diameter = 0.25f;
            //Adds particle to the list which causes the prefab to be instatiated
            GravityPlanets.PlanetInstances.Add(newParticle);
            //Update values for UI
            OnRadiusChanged();
            OnMassSliderChanged();
            Debug.Log("Object made");
        }
    }
    #endregion

    #region Updating UI and resetting

    //Updates UI with values from a planet
    public void UpdateUI(GravityPlanets values)
    {
        Inputfield_VelocityX.text = values.currentVelocity.x.ToString();
        Inputfield_VelocityY.text = values.currentVelocity.y.ToString();
        Slider_Mass.value = values.mass;
        Slider_Diameter.value = values.diameter;
        //Updates UI values
        OnMassSliderChanged();
        OnRadiusChanged();
    }
    //Resets UI elements to default values
    public void ResetUI()
    {
        Inputfield_VelocityX.text = "";
        Inputfield_VelocityY.text = "";
        Slider_Mass.value = 1;
        Slider_Diameter.value = 0.25f;
    }


    #endregion

    #region Velocity Update
    //When update velocity is clicked the input fields must be checked
    //if they are not empty then the dimention they are not empty in will be the new initial velocity of the particle
    public void OnUpdateVelocityClicked()
    {
        //gets current initial velocity
        Vector2 Velocity = GravityPlanets.PlanetInstances[ParticleIndexSelected].initialVelocity;
        //If not empty
        if (Inputfield_VelocityX.text != "")
        {
            //float.parse is a cast from string to float
            Velocity.x = float.Parse(Inputfield_VelocityX.text);
        }
        //If not empty
        if (Inputfield_VelocityY.text != "")
        {
            //float.parse is a cast from string to float
            Velocity.y = float.Parse(Inputfield_VelocityY.text);
        }
        //Updates velocity
        GravityPlanets.PlanetInstances[ParticleIndexSelected].initialVelocity = Velocity;
    }
    #endregion

    #region Inputfield Validation
    //Prevents invalid data being entered into inputfields
    public void OnTextChanged(InputField box)
    {
        try
        {
            //Gets current input
            string fieldInput = box.text;
            int size = fieldInput.Length;
            //Only needs to check last character
            char lastChar = fieldInput[size - 1];
            //Checks if the character is a number (0-9) and return true or false
            if (Char.IsNumber(lastChar) || lastChar == '.' || lastChar == '-' || lastChar == '+')
            {
                return;
            }
            else
            {
                //Remove character
                //Removes the last character
                box.text = fieldInput.Substring(0, size - 1);
            }
        }
        //Catch occurs if backspace was used
        catch (IndexOutOfRangeException) { }
    }


    #endregion

    #region Premade system Controller

    public void OnDropBox_PresetsChanged()
    {
        Gravity_PremadeSystems temp = new Gravity_PremadeSystems();
        temp.EarthMoonSystem();
    }
    #endregion

}