﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrawPathway : MonoBehaviour {  //DRAWS WAYPOINTS

	public NavMeshAgent agentSource;
	NavMeshPath path;
	LineRenderer line;
    CombatCommandMove host;
	public Vector3[] pathway;
    public float lineOffset = 1;
	public float totalDistance;

	void Start () {

	    host = this.gameObject.transform.parent.GetComponentInChildren<CombatCommandMove>();
        host.Clicked += Deactivate;

	    line = GetComponent<LineRenderer>(); //get the line renderer
		agentSource.isStopped = true;
	}
	
    public void GetAgentSource (GameObject o)
    {
        agentSource = o.GetComponent<NavMeshAgent>();
    }

	void Update(){

		Vector3 targetPoint = CastRay();
        if (targetPoint != Vector3.zero)
        {
            agentSource.SetDestination(targetPoint); //create the path pointing to target
		}
        pathway = agentSource.path.corners;
        line.positionCount = pathway.Length;

        for (int i = 0; i < pathway.Length; i++)
        {
            line.SetPosition(i, new Vector3(pathway[i].x, ((pathway[i].y) + (lineOffset)), pathway[i].z));
        }

    }

	public Vector3 CastRay()

    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;

        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
            {
                return interactionInfo.point;
            }
		else return Vector3.zero;
    }


    public void Deactivate(Vector3 point){

        print("Draw Pathway Deactivated");
        host.Clicked -= Deactivate;
        this.enabled = false;
        return;

    }

}

