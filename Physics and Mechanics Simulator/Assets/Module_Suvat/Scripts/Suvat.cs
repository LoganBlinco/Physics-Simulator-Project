using Mono.Data.SqliteClient;
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
        newParticle values = newParticle.CreateSuvatParticle();
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
                newParticle.ParticleInstances[particle] = values;
            }
            catch
            {
                newParticle.ParticleInstances.Add(values);
            }
        }
        else { }
    }

    //Checks if atleast 3 inputs in a single dimention have been inputted
    //Checks if dimentions active have atleast 2 dimentiosn entered (min amount for calculation)
    //Returns true or false
    private static bool IsValid(ref newParticle values, int dimentions)
    {
        Debug.Log("Is valid ran");
        //Bool based on if the program has atleast 1 dimention with three inputs
        int[] numberOfInputs = values.numberOfInputs;
        bool minThreeInputs = numberOfInputs[0] >= 3 || numberOfInputs[1] >= 3 || numberOfInputs[2] >= 3;
        Debug.Log("Is valid ran");
        //Bool if all dimentions have 3 inputs
        bool allAboveThree = getAboveThree(values, dimentions);

        //Gets the number of inputs above 2 from all dimentions active
        int numberAboveTwo = GetNumberAboveN(values,2,dimentions);
        Debug.Log("Is valid ran");
        //Must have atleast 3 inputs and other dimentions have more than 2 inputs OR if all dimentions have 3 inputs
        Debug.Log("Isvalid done");
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
    private static bool getAboveThree(newParticle values, int dimentions)
    {
        bool allAbove = true;
        int[] numberOfInputs = values.numberOfInputs;
        for (int i =0;i<dimentions;i++)
        {
            if (numberOfInputs[i] < 3)
            {
                allAbove = false;
            }
        }
        return allAbove;
    }

    //Returns the number of dimentions which have got >= N number of inputs which are not time
    private static int GetNumberAboveN(newParticle values, int N, int dimentions)
    {
        int numberAboveN = 0;
        int[] numberOfinputs = values.numberOfInputs;
        for (int i = 0; i < dimentions; i++)
        {
            if (numberOfinputs[i] >= N)
            {
                //If time is one of the quantities then an additional quantitiy must be entered
                if (values.key[i][4] == '1' && numberOfinputs[i] - 1 >= N)
                {
                    numberAboveN += 1;
                }
                else if (values.key[i][4] == '0')
                {
                    numberAboveN += 1;
                }
            }
        }
        return numberAboveN;
    }


    #region Getting inputs

    //Runs the methods required to get the Suvat inputs in all dimentions required from the UI.
    private static void getSuvat(ref newParticle values, int dimentions)
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
                    values.invalidInputs[2] = true;
                }
            }
            else
            {
                //Only 1 dimention selcted.Therefore Y and Z are disabled (elements 1 and 2)
                values.invalidInputs[1] = true;
                values.invalidInputs[2] = true;
            }

        }
        else
        {
            //No dimentions are selected.Therefore all dimentions are disabled
            values.invalidInputs[0] = true;
            values.invalidInputs[1] = true;
            values.invalidInputs[2] = true;
        }
    }

    //Gets the inputs from the user for each Suvat quantitity in the X dimention
    //If the input is not NULL then Key is updated for the dimention and element
    private static void GetInput_Suvat_x(ref newParticle values, int dimentions)
    {
        //float.parse converts a string to float type

        //Reference to the UI element
        Suvat_UiController controller = Suvat_UiController.instance;
        if (controller.S_x.text != "")
        {
            Vector3 temp = values.displacement;
            temp[0] = float.Parse(controller.S_x.text);
            values.displacement = temp;
            values.key[0] = ReplaceAtIndex(0, '1', values.key[0]);
        }
        if (controller.U_x.text != "")
        {
            Vector3 temp = values.initialVelocity;
            temp[0] = float.Parse(controller.U_x.text);
            values.initialVelocity = temp;
            values.key[0] = ReplaceAtIndex(1, '1', values.key[0]);
        }
        if (controller.V_x.text != "")
        {
            Vector3 temp = values.currentVelocity;
            temp[0] = float.Parse(controller.V_x.text);
            values.currentVelocity = temp;
            values.key[0] = ReplaceAtIndex(2, '1', values.key[0]);
        }
        if (controller.A_x.text != "")
        {
            Vector3 temp = values.acceleration;
            temp[0] = float.Parse(controller.A_x.text);
            values.acceleration = temp;
            values.key[0] = ReplaceAtIndex(3, '1', values.key[0]);
        }
        if (controller.Time.text != "")
        {
            values.motionTime = float.Parse(controller.Time.text);
            //Time shared between all dimentions
            values.key[0] = ReplaceAtIndex(4, '1', values.key[0]);
            values.key[1] = ReplaceAtIndex(4, '1', values.key[1]);
            values.key[2] = ReplaceAtIndex(4, '1', values.key[2]);
        }
        if (controller.R_x.text != "")
        {
            Vector3 temp = values.initialPosition;
            temp[0] = float.Parse(controller.R_x.text);
            values.initialPosition = temp;
        }
    }
    //Gets the inputs from the user for each Suvat quantitity in the Y dimention
    //If the input is not NULL then Key is updated for the dimention and element
    private static void GetInput_Suvat_y(ref newParticle values, int dimentions)
    {
        //float.parse converts a string to float type

        //Reference to the UI element
        Suvat_UiController controller = Suvat_UiController.instance;
        if (controller.S_y.text != "")
        {
            Vector3 temp = values.displacement;
            temp[1] = float.Parse(controller.S_y.text);
            values.displacement = temp;
            values.key[1] = ReplaceAtIndex(0, '1', values.key[1]);
        }
        if (controller.U_y.text != "")
        {
            Vector3 temp = values.initialVelocity;
            temp[1] = float.Parse(controller.U_y.text);
            values.initialVelocity = temp;
            values.key[1] = ReplaceAtIndex(1, '1', values.key[1]);
        }
        if (controller.V_y.text != "")
        {
            Vector3 temp = values.currentVelocity;
            temp[1] = float.Parse(controller.V_y.text);
            values.currentVelocity = temp;
            values.key[1] = ReplaceAtIndex(2, '1', values.key[1]);
        }
        if (controller.A_y.text != "")
        {
            //+= because gravity may be added by the toggle , thus cannot be strickly equal
            Vector3 temp = values.acceleration;
            temp[1] += float.Parse(controller.A_y.text);
            values.acceleration = temp;
            values.key[1] = ReplaceAtIndex(3, '1', values.key[1]);
        }
        if (controller.R_y.text != "")
        {
            Vector3 temp = values.initialPosition;
            temp[1] = float.Parse(controller.R_y.text);
            values.initialPosition = temp;
        }
    }
    //Gets the inputs from the user for each Suvat quantitity in the Z dimention
    //If the input is not NULL then Key is updated for the dimention and element
    private static void GetInput_Suvat_z(ref newParticle values, int dimentions)
    {
        //float.parse converts a string to float type

        //Reference to the UI element
        Suvat_UiController controller = Suvat_UiController.instance;
        if (controller.S_z.text != "")
        {
            Vector3 temp = values.displacement;
            temp[2] = float.Parse(controller.S_z.text);
            values.displacement = temp;
            values.key[2] = ReplaceAtIndex(0, '1', values.key[2]);
        }
        if (controller.U_z.text != "")
        {
            Vector3 temp = values.initialVelocity;
            temp[2] = float.Parse(controller.U_z.text);
            values.initialVelocity = temp;
            values.key[2] = ReplaceAtIndex(1, '1', values.key[2]);
        }
        if (controller.V_z.text != "")
        {
            Vector3 temp = values.currentVelocity;
            temp[2] = float.Parse(controller.V_z.text);
            values.currentVelocity = temp;
            values.key[2] = ReplaceAtIndex(2, '1', values.key[2]);
        }
        if (controller.A_z.text != "")
        {
            Vector3 temp = values.acceleration;
            temp[2] = float.Parse(controller.A_z.text);
            values.acceleration = temp;
            values.key[2] = ReplaceAtIndex(3, '1', values.key[2]);
        }
        if (controller.R_z.text != "")
        {
            Vector3 temp = values.initialPosition;
            temp[2] = float.Parse(controller.R_z.text);
            values.initialPosition = temp;
        }
    }


    //Gets non suvat based inputs from the UI and sassigns values to the particle instance
    private static void getMisc(ref newParticle values)
    {
        //Reference to the UI instance that being used
        Suvat_UiController controller = Suvat_UiController.instance;
        //Radius cannot be empty text or 0
        if (controller.Radius.text != "" && controller.Radius.text != "0")
        {
            //converts text to float from the input field
            values.diameter = float.Parse(controller.Radius.text);
        }
        else
        {
            //If the radius has not been stated or =0 then a default value of 1 is assigned.
            values.diameter = 1;
        }
        //If the gravity toggle has been enabled
        if (controller.Gravity.isOn == true)
        {
            //gravity must be added.Gravity is negative therefore a vector subtraction occurs of the magnitude of gravity.
            //Y component only
            values.acceleration -= new Vector3(0, Gravity, 0);
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
