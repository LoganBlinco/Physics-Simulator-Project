using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCollisions : MonoBehaviour {

    //Stores index of the particle in the CollisionParticles.ParticleInstances list
    public int particleIndex;
    //Bool preventing double calculations when two particles collide
    //This would be due to each one calling the OnTriggerEnter method
    private bool hasCollided = false;

    #region OnTriggers
    //Called when a trigger collider interacts with a rigid body with a collider
    private void OnTriggerEnter(Collider other)
    {
        if (GravitySimulationController.isSimulating == true)
        {
            if (other.gameObject.tag == "Particle")
            {
                //Performs calculations for particle to particle collision
                ParticleCollisionCalculation(other);
            }
            //Allowing me to find any objects which have not been tagged
            //A useful testing procedure
            else
            {
                Debug.Log(other.gameObject.tag);
            }
        }
    }
    //Called when the colliders exit each other
    private void OnTriggerExit(Collider other)
    {
        //If a particle to particle collision has occured then the collision process has ended therefore hasCollided = false (no longer colliding)
        if (other.gameObject.tag == "Particle")
        {
            other.gameObject.GetComponent<GravityCollisions>().hasCollided = false;
            hasCollided = false;
        }
    }
    #endregion

    #region Particle Collision
    //Controls the calculation process for particle to particle collisions
    private void ParticleCollisionCalculation(Collider other)
    {
        int otherIndex = other.gameObject.GetComponent<GravityCollisions>().particleIndex;
        //Preventing double calculations
        if (hasCollided == false)
        {
            other.gameObject.GetComponent<GravityCollisions>().hasCollided = true;
            hasCollided = true;
            //Performs the maths
            CalculateCollision(GravityPlanets.PlanetInstances[particleIndex], GravityPlanets.PlanetInstances[otherIndex]);
        }
    }
    //Performs and controls the maths of the calculations
    private void CalculateCollision(GravityPlanets first, GravityPlanets second)
    {
        //Getting unit direction vector
        Vector3 deltaPosition = first.MyGameObject.transform.position - second.MyGameObject.transform.position;
        Vector3 unitDirection = (deltaPosition) / (MyMaths.Vector_Magnitude(deltaPosition));
        //Getting velocity parrlele and perpendicular before colllisiosn
        Vector3 FirstParrelleVelocity = CalculateParrelelVelocity(first.currentVelocity, unitDirection);
        Vector3 FirstPerpendicularVelocity = first.currentVelocity - FirstParrelleVelocity;

        Vector3 SecondParrelleVelocity = CalculateParrelelVelocity(second.currentVelocity, unitDirection);
        Vector3 SecondPerpendicularVelocity = second.currentVelocity - SecondParrelleVelocity;

        float first_e = 1;
        float second_e = 1;
        //Relative restitution
        float e = first_e * second_e;

        float m = first.mass;
        float M = second.mass;
        //maths 
        first.currentVelocity = CalculateAfterVelocityFirst(m, M, FirstParrelleVelocity, SecondParrelleVelocity, e) + FirstPerpendicularVelocity;
        second.currentVelocity = CalculateAfterVelocitySecond(m, M, FirstParrelleVelocity, SecondParrelleVelocity, e) + SecondPerpendicularVelocity;
    }

    public static Vector3 CalculateParrelelVelocity(Vector3 velocity, Vector3 unitDirection)
    {
        //returns a vector of the parrelel velocity
        return (MyMaths.DotProduct_Value(velocity, unitDirection) * unitDirection);
    }

    public static Vector3 CalculateAfterVelocityFirst(float m, float M, Vector3 v, Vector3 u, float e)
    {
        Vector3 topQuotient = (m * v) + (M * u) + (M * e * u) - (M * e * v);
        float bottomQuotient = m + M;
        return (1 / bottomQuotient) * topQuotient;
    }

    public static Vector3 CalculateAfterVelocitySecond(float m, float M, Vector3 v, Vector3 u, float e)
    {
        Vector3 topQuotient = (m * v) + (M * u) + (m * e * v) - (m * e * u);
        float bottomQuotient = m + M;
        return (1 / bottomQuotient) * topQuotient;
    }
    #endregion
}
