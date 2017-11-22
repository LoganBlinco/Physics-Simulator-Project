using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suvat : MonoBehaviour {

    //Value of gravity which is added when gravity is enabled
    static float Gravity = 9.8f;

    //Ran when the calculate button is clicked on the UI.
    public static void OnCalculateClicked()
    {
        //Creates new particle instance
        Particle values = new Particle();
        //gets number of dimentions used from UI
        int dimentions = Suvat_UiController.instance.DropBox_Dimentions.value + 1;
        //gets particle number from UI dropbox
        int particle = Suvat_UiController.instance.DropBox_Particle.value;
        //Gets the non-suvat values and asseings them to values
        getMisc(ref values);
        //gets the suvat values in all dimentions given 
        getSuvat(ref values, dimentions);
        //Checks that 3 or more inputs have been entered.
        ValidateCheck(ref values);
        //Updates the suvat values displayed to the user
        Suvat_UiController.instance.UpdateUI(values);
        //Stores the instance into the Instances list in Particle.
        //Try must be used because the index may not be deffined yet if a particle has not been created with such index yet
        //if no particle with index has been created then it must be the final particle.
        try
        {
            Particle.Instances[particle] = values;
        }
        catch
        {
            Particle.Instances.Add(values);
        }
    }

    private static void ValidateCheck(ref Particle values)
    {
        if (values.GetNumberOfInputs()[0] >= 3 || values.GetNumberOfInputs()[1] >= 3 || values.GetNumberOfInputs()[2] >= 3)
        {
            values = SuvatSolvers.FindEquation(values);
        }
        else
        {
            string msg = "You must input atleast 3 quantities in one of the dimentions";
            //Must do
            //Must do
            //Unity.CreateMessagebox(msg);
            Debug.Log("Invalid input");
        }
    }


    #region Getting inputs

    //Runs the methods required to get the Suvat inputs in all dimentions required
    private static void getSuvat(ref Particle values, int dimentions)
    {
        if (dimentions >= 1)
        {
            //Gets inputs in the X dimention
            GetInput_Suvat_x(ref values, dimentions);
            if (dimentions >= 2)
            {
                //Gets inputs in the Y dimention
                GetInput_Suvat_y(ref values, dimentions);
                if (dimentions >= 3)
                {
                    //Gets inputs in the Z dimention
                    GetInput_Suvat_z(ref values, dimentions);
                }
                else
                {
                    //Only two dimentions selected therefore the 2nd element (third dimention) has invalid inputs
                    //Ie: no inputs
                    values.inValidInput[2] = true;
                }
            }
            else
            {
                //Only 1 dimention selcted.Therefore Y and Z are disabled (elements 1 and 2)
                values.inValidInput[1] = true;
                values.inValidInput[2] = true;
            }

        }
        else
        {
            //No dimentions are selected.Therefore all dimentions are disabled
            values.inValidInput[0] = true;
            values.inValidInput[1] = true;
            values.inValidInput[2] = true;
        }
    }

    private static void GetInput_Suvat_x(ref Particle values, int dimentions)
    {
        Suvat_UiController controller = Suvat_UiController.instance;
        if (controller.S_x.text != "")
        {
            values.Displacement[0] = float.Parse(controller.S_x.text);
            values.Key[0] = ReplaceAtIndex(0, '1', values.Key[0]);
        }
        if (controller.U_x.text != "")
        {
            values.InitialVelocity[0] = float.Parse(controller.U_x.text);
            values.Key[0] = ReplaceAtIndex(1, '1', values.Key[0]);
        }
        if (controller.V_x.text != "")
        {
            values.FinalVelocity[0] = float.Parse(controller.V_x.text);
            values.Key[0] = ReplaceAtIndex(2, '1', values.Key[0]);
        }
        if (controller.A_x.text != "")
        {
            values.Acceleration[0] = float.Parse(controller.A_x.text);
            values.Key[0] = ReplaceAtIndex(3, '1', values.Key[0]);
        }
        if (controller.Time.text != "")
        {
            values.Time = float.Parse(controller.Time.text);
            values.Key[0] = ReplaceAtIndex(4, '1', values.Key[0]);
        }
        if (controller.R_x.text != "")
        {
            values.InitialPosition[0] = float.Parse(controller.R_x.text);
        }
    }
    private static void GetInput_Suvat_y(ref Particle values, int dimentions)
    {
        Suvat_UiController controller = Suvat_UiController.instance;
        if (controller.S_y.text != "")
        {
            values.Displacement[1] = float.Parse(controller.S_y.text);
            values.Key[1] = ReplaceAtIndex(0, '1', values.Key[1]);
        }
        if (controller.U_y.text != "")
        {
            values.InitialVelocity[1] = float.Parse(controller.U_y.text);
            values.Key[1] = ReplaceAtIndex(1, '1', values.Key[1]);
        }
        if (controller.V_y.text != "")
        {
            values.FinalVelocity[1] = float.Parse(controller.V_y.text);
            values.Key[1] = ReplaceAtIndex(2, '1', values.Key[1]);
        }
        if (controller.A_y.text != "")
        {
            //+= because gravity may be added by the toggle
            values.Acceleration[1] += float.Parse(controller.A_y.text);
            values.Key[1] = ReplaceAtIndex(3, '1', values.Key[1]);
        }
        if (controller.R_y.text != "")
        {
            values.InitialPosition[1] = float.Parse(controller.R_y.text);
        }
    }
    private static void GetInput_Suvat_z(ref Particle values, int dimentions)
    {
        Suvat_UiController controller = Suvat_UiController.instance;
        if (controller.S_z.text != "")
        {
            values.Displacement[2] = float.Parse(controller.S_z.text);
            values.Key[2] = ReplaceAtIndex(0, '1', values.Key[2]);
        }
        if (controller.U_z.text != "")
        {
            values.InitialVelocity[2] = float.Parse(controller.U_z.text);
            values.Key[2] = ReplaceAtIndex(1, '1', values.Key[2]);
        }
        if (controller.V_z.text != "")
        {
            values.FinalVelocity[2] = float.Parse(controller.V_z.text);
            values.Key[1] = ReplaceAtIndex(2, '1', values.Key[2]);
        }
        if (controller.A_z.text != "")
        {
            values.Acceleration[2] = float.Parse(controller.A_z.text);
            values.Key[1] = ReplaceAtIndex(3, '1', values.Key[2]);
        }
        if (controller.R_z.text != "")
        {
            values.InitialPosition[2] = float.Parse(controller.R_z.text);
        }
    }


    //Gets non suvat based inputs from the UI and sassigns values to the particle instance
    private static void getMisc(ref Particle values)
    {
        //Reference to the UI instance that being used
        Suvat_UiController controller = Suvat_UiController.instance;
        //Radius cannot be empty text or 0
        if (controller.Radius.text != "" && controller.Radius.text != "0")
        {
            //converts text to float from the input field
            values.Radius = float.Parse(controller.Radius.text);
        }
        else
        {
            //If the radius has not been stated or =0 then a default value of 1 is assigned.
            values.Radius = 1;
        }
        //If the gravity toggle has been enabled
        if (controller.Gravity.isOn == true)
        {
            //gravity must be added.Gravity is negative therefore a vector subtraction occurs of the magnitude of gravity.
            //Y component only
            values.Acceleration -= new Vector3(0, Gravity, 0);
        }
    }

    #endregion

    public static string ReplaceAtIndex(int index, char value, string word)
    {
        try
        {
            char[] letters = word.ToCharArray();
            letters[index] = value;
            return new string(letters);
        }
        catch
        {
            return "";
        }

    }
}
