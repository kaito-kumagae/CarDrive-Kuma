using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    private CarAgent carAgent;
    private Evaluator evaluator = Evaluator.getInstance();
    private CarInformation carInformation;

    // Initialize member variables
    public void Initialize(CarAgent carAgent)
    {
        this.carAgent = carAgent;
        this.carInformation = carAgent.carInformation;
    }

    public void CrashProcess(Collision other)
    {
        // If car have an accident
        if (other.gameObject.CompareTag("wall") || other.gameObject.CompareTag("car"))
        {
            var carCenter = carAgent.transform.position + Vector3.up;

            carAgent.rewardCalculation.setCrashReward(carAgent.crashReward);
            carAgent.EndEpisode();
            if (carAgent.countPassing == true)
            {
                carAgent.detectedFrontCarIdList.Clear();
                GetComponentInParent<UpdateCarParameters>().RemoveMyIdFromAllcarAgents(carAgent.id);
            }

            // If the collision was a car
            if (other.gameObject.CompareTag("car"))
            {
                var otherAgent = (CarAgent)other.gameObject.GetComponent(typeof(CarAgent));
                if ((carAgent.id < otherAgent.id) && (IsNotErasedId(otherAgent.id)))
                {
                    if (Physics.Raycast(carCenter, Vector3.down, out var hit, 2f))
                    {
                        var newHit = hit.transform;
                        if (newHit.GetComponent<Collider>().tag == "startTile")
                        {
                            if (carAgent.generateNew)
                            {
                                Destroy(other.gameObject);
                                carAgent.carInformation.currentCarNum--;
                            }
                        }
                        else
                        {
                            evaluator.addCrashCars(Time.realtimeSinceStartup,carAgent.speed);
                            if (carAgent.generateNew)
                            {
                                evaluator.addCrashCars(Time.realtimeSinceStartup,otherAgent.speed);
                                Destroy(other.gameObject);
                                carInformation.currentCarNum--;
                            }
                        }
                    }
                }
            }
            else
            {
                evaluator.addCrashCars(Time.realtimeSinceStartup, carAgent.speed);
            }
        }
    }

    // Prevent both cars from disappearing when cars with id0 and id1 collide
    private bool IsNotErasedId(int otherAgentId)
    {
        List<int> isNotErasedIdList = new List<int>();
        for(int i = 0; i < carAgent.startCarNum; i++)
        {
            isNotErasedIdList.Add(i);
        }
        if ((isNotErasedIdList.Contains(carAgent.id)) && (isNotErasedIdList.Contains(otherAgentId)))
        {
            return false;
        }
        return true;
    }
}