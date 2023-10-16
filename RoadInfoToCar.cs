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

    public void RecognizeLane()
    {   
        //bool MainLaneFlag = false;
        //bool MergingLaneFlag = false;

        var carCenter = carAgent.transform.position + Vector3.up;

        if (Physics.Raycast(carCenter, Vector3.down, out var hit, 2f))
        {
            var newHitTile = hit.transform;

            // If on MainLane tile
            if (newHitTile.CompareTag("MainLaneTile"))
            {
                //MainLaneFlag = true;
                Debug.Log("MainLane");
                getCarInfoMergingLane();
            }
            // If on MergingLane tile
            else if (newHitTile.CompareTag("MergingLaneTile"))
            {
                //MergingLaneFlag = true;
                Debug.Log("MergingLane");
                getCarInfoMineLane();
            }
        }
    }

    public void getCarInfoMineLane()
    {
        // MergingLaneのタイル位置とサイズを取得する必要があります。
        // この例では、MergingLaneの中心位置とサイズを事前に知っていると仮定しています。
        Vector3 mergingLaneCenter = new Vector3(0f, 0f, -25f);
        Vector3 mergingLaneSize = new Vector3(10f, 10f, 10f);
        
        Collider[] hitColliders = Physics.OverlapBox(mergingLaneCenter, mergingLaneSize * 0.5f);
        Debug.Log("MergingLaneInfoGet");

        if(hitColliders.Length > 0)
        {
            //存在有無の情報
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
                }
            }
        }

        // ここでclosestCarPositionは、MergingLane上で自分の車に最も近い車の位置です。
        // 必要に応じてこの情報を使用します。
    }
    
    public void getCarInfoMergingLane()
    {
        // MainLaneのタイル位置とサイズを取得する必要があります。
        // この例では、MainLaneの中心位置とサイズを事前に知っていると仮定しています。
        Vector3 mainLaneCenter = new Vector3(-10f, 0f, -15f);
        Vector3 mainLaneSize = new Vector3(10f, 10f, 10f); // TODO: 実際のサイズを設定
        Debug.Log("intomeraginglane");
        // MainLaneの位置でBoxをオーバーラップして、そこにあるすべてのコライダーを取得
        Collider[] hitColliders = Physics.OverlapBox(mainLaneCenter, mainLaneSize * 0.5f);
        Debug.Log("MineLaneInfoGet");

        if(hitColliders.Length > 0)
        {
            //存在有無の情報
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
                }
            }
        }
    // ここでclosestCarPositionは、MainLane上で自分の車に最も近い車の位置です。
    // 必要に応じてこの情報を使用します。
    }
    
}