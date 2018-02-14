using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour {

    private static Projectiles_UiController Controller;
    private static float Velocity = 0;
    private static float Distance = 0;
    private static float ProjectionAngle = 0;
    private static float SlopeAngle = 0;
    private static float TimeDuration = 0;

    private static float gravity = 9.8f;

    private static int numberOfInputs = 0;
    private static bool boolSlopeAngle = false;
    private static bool boolProjectionAngle = false;



    public static void OnCalculateClicked()
    {
        Controller = Projectiles_UiController.instance;
        Particle values = new Particle();
        //gets the inputs from user
        getInputs(ref values);
        if (IsValid(ref values))
        {
            AssignInputs(ref values);
            values = SuvatSolvers.FindEquation(values);
            Controller.UpdateUi(values);
            Particle.Instances = new List<Particle>();
            Particle.Instances.Add(values);
        }
        else
        {
            string title = "Invalid input";
            string message = "You must enter atleast 3 quantities and the time duration!";
            //Creates message box with title , message and button with text "Ok"
            //EditorUtility.DisplayDialog(title, message, "Ok");
        }
    }



    //Assigns values from the users inputs
    private static void getInputs(ref Particle values)
    {
        #region If Statements for inputs
        if (Controller.InputField_Velocity.text != "")
        {
            Velocity = float.Parse(Controller.InputField_Velocity.text);
            numberOfInputs += 1;
        }
        if (Controller.InputField_Distance.text != "")
        {
            Distance = float.Parse(Controller.InputField_Distance.text);
            numberOfInputs += 1;
        }
        if (Controller.InputField_ProjectionAngle.text != "")
        {
            ProjectionAngle = float.Parse(Controller.InputField_ProjectionAngle.text);
            boolProjectionAngle = true;
        }
        if (Controller.InputField_SlopeAngle.text != "")
        {
            SlopeAngle = float.Parse(Controller.InputField_SlopeAngle.text);
            boolSlopeAngle = true;
        }
        if (Controller.InputField_Duration.text != "")
        {
            TimeDuration = float.Parse(Controller.InputField_Duration.text);
            numberOfInputs += 1;
        }
        #endregion
	}

    //Checks if input is valid and returns true or false
    private static bool IsValid(ref Particle values)
    {
        //SlopeAngle and ProjectionAngle must be input aswell as two other quanties
        if (boolSlopeAngle == true && boolProjectionAngle == true && numberOfInputs >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private static void AssignInputs(ref Particle values)
    {
        if (Controller.InputField_Velocity.text != "")
        {
            values.InitialVelocity = new Vector3(
                Velocity * Mathf.Cos(ProjectionAngle * Mathf.Deg2Rad),
                Velocity * Mathf.Sin(ProjectionAngle * Mathf.Deg2Rad),
                0.0f);
            values.Key[0] = Suvat.ReplaceAtIndex(1, '1', values.Key[0]);
            values.Key[1] = Suvat.ReplaceAtIndex(1, '1', values.Key[1]);
        }
        if (Controller.InputField_Distance.text != "")
        {
            values.Displacement = new Vector3(
                Distance * Mathf.Cos(SlopeAngle * Mathf.Deg2Rad),
                Distance * Mathf.Sin(SlopeAngle * Mathf.Deg2Rad),
                0.0f);
            values.Key[0] = Suvat.ReplaceAtIndex(0, '1', values.Key[0]);
            values.Key[1] = Suvat.ReplaceAtIndex(0, '1', values.Key[1]);
        }
        if (Controller.InputField_Duration.text != "")
        {
            values.Time = TimeDuration;
            values.Key[0] = Suvat.ReplaceAtIndex(4, '1', values.Key[0]);
            values.Key[1] = Suvat.ReplaceAtIndex(4, '1', values.Key[1]);
            values.Key[2] = Suvat.ReplaceAtIndex(4, '1', values.Key[2]);
        }
        values.Acceleration = new Vector3(
            0.0f,
            -gravity,
            0.0f);
        values.Key[0] = Suvat.ReplaceAtIndex(3, '1', values.Key[0]);
        values.Key[1] = Suvat.ReplaceAtIndex(3, '1', values.Key[1]);

        //Default radius
        values.Radius = 1;
    }
}
