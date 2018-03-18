using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombinedModule_UiController : MonoBehaviour {

    public Toggle Toggle_Collisions;
    public Toggle Toggle_Gravity;
    public Slider Slider_Restitution;
    public Text Label_Restitution;
    public Slider Slider_Mass;
    public Text Label_Mass;

    #region OnClicked

    //Ran when the Calculate button is clicked
    //Begins the calculation proccess
    public void OnCalculateClicked()
    {
        Suvat.OnCalculateClicked();
        MiscInfomation();
    }

    private void MiscInfomation()
    {
        newParticle particle = newParticle.ParticleInstances[Suvat_UiController.instance.DropBox_Particle.value];
        if (Toggle_Collisions.isOn == true)
        {
            particle.AddParticlePropery(newParticle.Properties.collisions);
            particle.collisions = true;
            particle.AddParticlePropery(newParticle.Properties.restitution);
            particle.restitution = Slider_Restitution.value;
        }
        if (Toggle_Gravity.isOn == true)
        {
            particle.AddParticlePropery(newParticle.Properties.gravity);
            particle.gravity = true;
        }
        if (particle.hasMass)
        {
            particle.mass = Slider_Mass.value;
        }
        else
        {
            particle.AddParticlePropery(newParticle.Properties.mass);
            particle.mass = Slider_Mass.value;
        }

    }

    //Ran when the reset button is clicked
    public void OnResetClicked()
    {
        //Resets the static variables for simulations
        newSimulateController.isSimulating = false;
        newSimulateController.SimulationSpeed = 1;
        newParticle.ParticleInstances.Clear();
        //Loads scene to refresh values
        SceneManager.LoadScene("FinalScene");
    }

    //Ran when the Simulation button is clicked
    public void OnSimulateClicked()
    {
        DestroyObejctsWithTag("Particle");
        //Calculates values required for simulation
        OnCalculateClicked();
        //Begins the simulation process in the SimulateController class
        newSimulateController.isSimulating = true;
    }

    public void OnSliderRestitutionChanged()
    {
        //Rounding to 2 Decimal places
        string value2DP = Slider_Restitution.value.ToString("n2");
        //Updates the label's text
        Label_Restitution.text = value2DP;
    }
    public void OnSliderMassChanged()
    {
        //Rounding to 2 Decimal places
        string value2DP = Slider_Mass.value.ToString("n2");
        //Updates the label's text
        Label_Mass.text = value2DP;
    }

    #endregion

    //Destroys anyobject with the input tag parameter in the scene
    public static void DestroyObejctsWithTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }
}
