using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCalculator : MonoBehaviour {

    //Stores index of the particle in the GravityPlanets.PlanetInstances list
    public int particleIndex;

    //List containing gameobjects which have collided with the current object (calculations to perform)
    public List<GameObject> collidedObjects = new List<GameObject>();

    #region OnTriggers
    //Called when a trigger collider interacts with a rigid body with a collider
    private void OnTriggerEnter(Collider other)
    {
        if (GravitySimulationController.isSimulating == true)
        {
            ParticleCollisionCalculation(other);
        }
    }
    //Called when the colliders exit each other
    private void OnTriggerExit(Collider other)
    {
        //If a particle to particle collision has occured then the collision process has ended therefore hasCollided = false (no longer colliding)
        if (other.gameObject.tag == "Particle")
        {
            //Collisions complete therefore gameobject must be removed from collidedObjects list
            other.gameObject.GetComponent<GravityCalculator>().collidedObjects.Remove(gameObject);
            collidedObjects.Remove(other.gameObject);
        }
    }
    #endregion

    #region Particle Collision
    //Controls the calculation process for particle to particle collisions
    private void ParticleCollisionCalculation(Collider other)
    {
        int otherIndex = other.gameObject.GetComponent<GravityCalculator>().particleIndex;
        Debug.Log(particleIndex);
        Debug.Log(otherIndex);
        //Preventing double calculations
        if (collidedObjects.Contains(other.gameObject) == false)
        {
            Debug.Log("Calculation needed");
            //Adds the object which has colllided to the list of collidedobjects
            other.gameObject.GetComponent<GravityCalculator>().collidedObjects.Add(gameObject);
            collidedObjects.Add(other.gameObject);
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
        Debug.Log(deltaPosition);
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

        Debug.Log(first.currentVelocity);
        Debug.Log(second.currentVelocity);
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
