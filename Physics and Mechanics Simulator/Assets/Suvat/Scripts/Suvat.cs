using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suvat : MonoBehaviour {

    static float Gravity = 9.8f;

    public static void OnCalculateClicked()
    {
        Particle values = new Particle();
        int dimentions = Suvat_UiController.instance.DropBox_Dimentions.value + 1;
        getMisc(ref values);
        getSuvat(ref values, dimentions);
        ValidateCheck(ref values);
        Suvat_UiController.instance.UpdateUI(values);
        try
        {
            Particle.Instances[dimentions] = values;
        }
        catch(ArgumentOutOfRangeException e)
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

    private static void getMisc(ref Particle values)
    {
        Suvat_UiController controller = Suvat_UiController.instance;
        if (controller.Radius.text != "")
        {
            values.Radius = float.Parse(controller.Radius.text);
        }else
        {
            values.Radius = 1;
        }
        if (controller.Gravity.isOn == true)
        {
            values.Acceleration -= new Vector3(0, Gravity, 0);
        }
    }

    private static void getSuvat(ref Particle values, int dimentions)
    {
        if (dimentions >= 1)
        {
            GetInput_Suvat_x(ref values, dimentions);
            if (dimentions >= 2)
            {
                GetInput_Suvat_y(ref values, dimentions);
                if (dimentions >= 3)
                {
                    GetInput_Suvat_z(ref values, dimentions);
                }
                else
                {
                    values.inValidInput[2] = true;
                }
            }
            else
            {
                values.inValidInput[1] = true;
                values.inValidInput[2] = true;
            }

        }
        else
        {
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
