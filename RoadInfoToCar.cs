using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoadInfoToCar
{
    private CarAgent carAgent;
    private CarInformation carInformation;
    
    public RoadInfoToCar(CarAgent carAgent)
    {
        this.carAgent = carAgent;
        this.carInformation = carAgent.carInformation;
    }

    public void RecognizeLane(ref List<float> observations) //タイルの判別
    {   
        var carCenter = carAgent.transform.position + Vector3.up;
        if (Physics.Raycast(carCenter, Vector3.down, out var hit, 2f))
        {
            var newHitTile = hit.transform;

            // If on MainLane tile
            if (newHitTile.CompareTag("MainLaneTile"))　
            {
                getCarInfoMergingLane(ref observations);
            }
            // If on MergingLane tile
            else if (newHitTile.CompareTag("MergingLaneTile"))
            {
                getCarInfoMineLane(ref observations);
            }
            else
            {
                observations.Add(0);
                observations.Add(0); 
                observations.Add(0);
            }
        }
        else
        {
            observations.Add(0);
            observations.Add(0); 
            observations.Add(0);
        }
    }
    public void getCarInfoMineLane(ref List<float> observations)//MineLaneTileの上にいる一番近い車の位置の取得
    {
        Vector3 mergingLaneCenter = new Vector3(-10f, 0f, -15f);
        Vector3 mergingLaneSize = new Vector3(10f, 10f, 10f);
        
        Collider[] hitColliders = Physics.OverlapBox(mergingLaneCenter, mergingLaneSize * 0.5f);

        if(hitColliders.Length > 0)
        {
            observations.Add(1);
        }
        else
        {
            observations.Add(0);
        }

        float closestDistance = float.MaxValue;
        Vector3 closestCarPosition = Vector3.zero;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("car"))
            {
                float currentDistance = Vector3.Distance(carAgent.transform.position, hitCollider.transform.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestCarPosition = hitCollider.transform.position;
                    Debug.Log("MergingLaneNearCar;"+closestCarPosition);
                }
            }
        }
        observations.Add(closestCarPosition.x);
        observations.Add(closestCarPosition.z);
    }
    
    public void getCarInfoMergingLane(ref List<float> observations) //MergingLaneTileの上にいる一番近い車の位置の取得
    {
        Vector3 mainLaneCenter = new Vector3(0f, 0f, -25f);
        Vector3 mainLaneSize = new Vector3(10f, 10f, 10f); 

        Collider[] hitColliders = Physics.OverlapBox(mainLaneCenter, mainLaneSize * 0.5f);

        if(hitColliders.Length > 0)
        {
            observations.Add(1);
        }
        else
        {
            observations.Add(0);
        }
        
        float closestDistance = float.MaxValue;
        Vector3 closestCarPosition = Vector3.zero;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("car"))
            {
                float currentDistance = Vector3.Distance(carAgent.transform.position, hitCollider.transform.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestCarPosition = hitCollider.transform.position;
                    Debug.Log("MineLaneNearCar;"+closestCarPosition);
                }
            }
        }
        observations.Add(closestCarPosition.x);
        observations.Add(closestCarPosition.z);
    }

}