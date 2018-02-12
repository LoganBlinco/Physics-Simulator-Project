using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCalculator : MonoBehaviour {

	public int particleIndex;
	private bool hasCollided = false;

	private void OnTriggerEnter(Collider other)
	{
        if (CollisionsSimulationController.isSimulating == true)
        {
            if (other.gameObject.tag == "Particle")
            {
                Debug.Log("Particle");
                ParticleCollisionCalculation(other);
            }
            else if (other.gameObject.tag == "Border")
            {
                Debug.Log("Border");
                BorderCollisionCalculation(other);
            }
            else
            {
                Debug.Log(other.gameObject.tag);
            }
        }

	}
	private void OnTriggerExit(Collider other)
	{
        if (other.gameObject.tag == "Particle")
        {
            other.gameObject.GetComponent<CollisionCalculator>().hasCollided = false;
            hasCollided = false;
        }

	}
	private void BorderCollisionCalculation(Collider other)
	{
        float BorderRestitution = Collisions_InputController.Instance.Slider_BorderRestitution.value;
        float Restitution = CollisionsParticle.ParticleInstances[particleIndex].restitution;

        Vector3 previousVelocity = CollisionsParticle.ParticleInstances[particleIndex].currentVelocity;
        //Horizontal border (y velocity changes)
        if (other.transform.localScale.x > other.transform.localScale.y)
        {
            CollisionsParticle.ParticleInstances[particleIndex].currentVelocity = new Vector3(
                previousVelocity.x,
                -1 * (BorderRestitution * Restitution) * previousVelocity.y,
                previousVelocity.z);
        }
        //Vertical border ( x velocity changes)
        else if (other.transform.localScale.x < other.transform.localScale.y)
        {
            CollisionsParticle.ParticleInstances[particleIndex].currentVelocity = new Vector3(
                -1 * (BorderRestitution * Restitution) * previousVelocity.x,
                previousVelocity.y,
                previousVelocity.z);
        }
	}


	private void ParticleCollisionCalculation(Collider other)
	{
		int otherIndex = other.gameObject.GetComponent<CollisionCalculator> ().particleIndex;
        Debug.Log(hasCollided);
		if (hasCollided == false)
		{
			Debug.Log ("This has been ran");
			other.gameObject.GetComponent<CollisionCalculator> ().hasCollided = true;
			hasCollided = true;
			CalculateCollision (CollisionsParticle.ParticleInstances[particleIndex], CollisionsParticle.ParticleInstances[otherIndex]);
		}
	}

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
		float e = first_e * second_e;

		float m = first.mass;
		float M = second.mass;

		first.currentVelocity = CalculateAfterVelocityFirst(m, M, FirstParrelleVelocity, SecondParrelleVelocity, e) + FirstPerpendicularVelocity;
		second.currentVelocity = CalculateAfterVelocitySecond(m, M, FirstParrelleVelocity, SecondParrelleVelocity, e) + FirstPerpendicularVelocity;
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

}
