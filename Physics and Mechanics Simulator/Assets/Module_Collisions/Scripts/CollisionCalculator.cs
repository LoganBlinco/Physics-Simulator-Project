using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCalculator : MonoBehaviour {

    //Stores index of the particle in the CollisionParticles.ParticleInstances list
	public int particleIndex;


    public List<GameObject> collidedObjects = new List<GameObject>();

    #region OnTriggers
    //Called when a trigger collider interacts with a rigid body with a collider
    private void OnTriggerEnter(Collider other)
	{
        if (CollisionsSimulationController.isSimulating == true)
        {
            if (other.gameObject.tag == "Particle")
            {
                //Performs calculations for particle to particle collision
                ParticleCollisionCalculation(other);
            }
            else if (other.gameObject.tag == "Border")
            {
                //Performs calculations for particle to border collision
                BorderCollisionCalculation(other);
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
            other.gameObject.GetComponent<CollisionCalculator>().collidedObjects.Remove(gameObject);
            collidedObjects.Remove(other.gameObject);
        }
	}
    #endregion

    #region Border Collision
    //Calculations when a border collision occurs
    private void BorderCollisionCalculation(Collider other)
	{
        //Gets restitution calculaues from the slider and from particle
        float BorderRestitution = Collisions_InputController.Instance.Slider_BorderRestitution.value;
        float Restitution = CollisionsParticle.ParticleInstances[particleIndex].restitution;

        Vector3 previousVelocity = CollisionsParticle.ParticleInstances[particleIndex].currentVelocity;
        //Horizontal border (y velocity changes)
        if (other.transform.localScale.x > other.transform.localScale.y)
        {
            //If collided with a horizontal border then only the Y is afected by reversing the direction and multiplying by the restitutions
            CollisionsParticle.ParticleInstances[particleIndex].currentVelocity = new Vector3(
                previousVelocity.x,
                -1 * (BorderRestitution * Restitution) * previousVelocity.y,
                previousVelocity.z);
        }
        //Vertical border ( x velocity changes)
        else if (other.transform.localScale.x < other.transform.localScale.y)
        {
            //If collided with a vertical border then only the X is afected by reversing the direction and multiplying by the restitutions
            CollisionsParticle.ParticleInstances[particleIndex].currentVelocity = new Vector3(
                -1 * (BorderRestitution * Restitution) * previousVelocity.x,
                previousVelocity.y,
                previousVelocity.z);
        }
	}
    #endregion

    #region Particle Collision
    //Controls the calculation process for particle to particle collisions
    private void ParticleCollisionCalculation(Collider other)
	{
		int otherIndex = other.gameObject.GetComponent<CollisionCalculator> ().particleIndex;
        //Preventing double calculations
		if (collidedObjects.Contains(other.gameObject) == false)
		{
            //other.gameObject.GetComponent<CollisionCalculator> ().hasCollided = true;
            //hasCollided = true;
            other.gameObject.GetComponent<CollisionCalculator>().collidedObjects.Add(gameObject);
            collidedObjects.Add(other.gameObject);
            //Performs the maths
            CalculateCollision (CollisionsParticle.ParticleInstances[particleIndex], CollisionsParticle.ParticleInstances[otherIndex]);
		}
	}
    //Performs and controls the maths of the calculations
	private void CalculateCollision(CollisionsParticle first , CollisionsParticle second)
	{
		//Getting unit direction vector
		Vector3 deltaPosition = first.MyGameObject.transform.position - second.MyGameObject.transform.position;
		Vector3 unitDirection = (deltaPosition) / (MyMaths.Vector_Magnitude(deltaPosition));
		//Getting velocity parrlele and perpendicular before colllisiosn
		Vector3 FirstParrelleVelocity = CalculateParrelelVelocity(first.currentVelocity, unitDirection);
		Vector3 FirstPerpendicularVelocity = first.currentVelocity - FirstParrelleVelocity;

		Vector3 SecondParrelleVelocity = CalculateParrelelVelocity(second.currentVelocity, unitDirection);
		Vector3 SecondPerpendicularVelocity = second.currentVelocity - SecondParrelleVelocity;

		float first_e = first.restitution;
		float second_e = second.restitution;
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
