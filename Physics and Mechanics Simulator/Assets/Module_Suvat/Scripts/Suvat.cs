﻿using Mono.Data.SqliteClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        if (IsValid(ref values, dimentions))
        {
            //Caclulates the values of quantities in all dimentions
            values = SuvatSolvers.FindEquation(values);
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
        else
        {
            string title = "Invalid input";
            string message = "You must enter at least 3 quantities in a dimention and two quantities in all other dimentions active";
            //Creates message box with title , message and button with text "Ok"
            //EditorUtility.DisplayDialog(title, message, "Ok");
        }

    }

    //Checks if atleast 3 inputs in a single dimention have been inputted
    //Checks if dimentions active have atleast 2 dimentiosn entered (min amount for calculation)
    //Returns true or false
    private static bool IsValid(ref Particle values, int dimentions)
    {
        //Bool based on if the program has atleast 1 dimention with three inputs
        bool minThreeInputs = values.GetNumberOfInputs()[0] >= 3 || values.GetNumberOfInputs()[1] >= 3 || values.GetNumberOfInputs()[2] >= 3;
        //Bool if all dimentions have 3 inputs
        bool allAboveThree = getAboveThree(values, dimentions);

        //Gets the number of inputs above 2 from all dimentions active
        int numberAboveTwo = GetNumberAboveN(values,2,dimentions);
        //Must have atleast 3 inputs and other dimentions have more than 2 inputs OR if all dimentions have 3 inputs
        if ((minThreeInputs == true && numberAboveTwo == dimentions) || allAboveThree == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //Returns if all dmentions active have 3 or more inputs
    private static bool getAboveThree(Particle values, int dimentions)
    {
        bool allAbove = true;
        for (int i =0;i<dimentions;i++)
        {
            if (values.GetNumberOfInputs()[i] < 3)
            {
                allAbove = false;
            }
        }
        return allAbove;
    }

    //Returns the number of dimentions which have got >= N number of inputs which are not time
    private static int GetNumberAboveN(Particle values, int N, int dimentions)
    {
        int numberAboveN = 0;
        for (int i = 0; i < dimentions; i++)
        {
            if (values.GetNumberOfInputs()[i] >= N)
            {
                //If time is one of the quantities then an additional quantitiy must be entered
                if (values.Key[i][4] == '1' && values.GetNumberOfInputs()[i] - 1 >= N)
                {
                    numberAboveN += 1;
                }
                else if (values.Key[i][4] == '0')
                {
                    numberAboveN += 1;
                }
            }
        }
        return numberAboveN;
    }


    #region Getting inputs

    //Runs the methods required to get the Suvat inputs in all dimentions required from the UI.
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

    //Gets the inputs from the user for each Suvat quantitity in the X dimention
    //If the input is not NULL then Key is updated for the dimention and element
    private static void GetInput_Suvat_x(ref Particle values, int dimentions)
    {
        //float.parse converts a string to float type

        //Reference to the UI element
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
            //Time shared between all dimentions
            values.Key[0] = ReplaceAtIndex(4, '1', values.Key[0]);
            values.Key[1] = ReplaceAtIndex(4, '1', values.Key[1]);
            values.Key[2] = ReplaceAtIndex(4, '1', values.Key[2]);
        }
        if (controller.R_x.text != "")
        {
            values.InitialPosition[0] = float.Parse(controller.R_x.text);
        }
    }
    //Gets the inputs from the user for each Suvat quantitity in the Y dimention
    //If the input is not NULL then Key is updated for the dimention and element
    private static void GetInput_Suvat_y(ref Particle values, int dimentions)
    {
        //float.parse converts a string to float type

        //Reference to the UI element
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
            //+= because gravity may be added by the toggle , thus cannot be strickly equal
            values.Acceleration[1] += float.Parse(controller.A_y.text);
            values.Key[1] = ReplaceAtIndex(3, '1', values.Key[1]);
        }
        if (controller.R_y.text != "")
        {
            values.InitialPosition[1] = float.Parse(controller.R_y.text);
        }
    }
    //Gets the inputs from the user for each Suvat quantitity in the Z dimention
    //If the input is not NULL then Key is updated for the dimention and element
    private static void GetInput_Suvat_z(ref Particle values, int dimentions)
    {
        //float.parse converts a string to float type

        //Reference to the UI element
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
            values.Key[2] = ReplaceAtIndex(2, '1', values.Key[2]);
        }
        if (controller.A_z.text != "")
        {
            values.Acceleration[2] = float.Parse(controller.A_z.text);
            values.Key[2] = ReplaceAtIndex(3, '1', values.Key[2]);
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

    //Repalces a character in a string and returns the new string
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