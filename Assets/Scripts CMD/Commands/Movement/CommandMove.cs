﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavWaypointManager))]
[RequireComponent(typeof(DistanceCalc))]
[RequireComponent(typeof(DistanceCompare))]

public class CommandMove : CombatCommand  // the master controller for the 'Move' combat command.
{
    public delegate void SetPointDelegate(Vector3 point);

    public event SetPointDelegate Clicked;
    public event SetPointDelegate RemoveWaypoint;

    public bool canPlaceWaypoint;
    public bool hoveringOverSomething;

    public DistanceCalc distanceCalc;
    public DistanceCompare distanceCompare;

    void OnEnable()
    {
        NavWaypoint.DeSelectAllEvent += _DeSelectEvent;   
        Selection.Enter += HoveringOverSomething;     
        Selection.Exit += NotHovering;
    }

    public override void Start()
    {
        base.Start();

        combatController.SelectEvent += Ready;

        distanceCalc = GetComponent<DistanceCalc>();
        distanceCompare = GetComponent<DistanceCompare>();
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void Update()
    {

        while(!combatController.selected)
        {
            return;
        }

        distanceCompare.DistanceTest(distanceCalc);

        if (Input.GetMouseButtonDown(0) && distanceCompare.InRange == true && canPlaceWaypoint && !hoveringOverSomething && !EventSystem.current.IsPointerOverGameObject())  // Left Click
        {
            Clicked(GetThePoint.PickVector3());  //send point out to all relevant scripts
            return;
        }

        if (Input.GetMouseButtonDown(1) && !hoveringOverSomething && !EventSystem.current.IsPointerOverGameObject())  // Right Click
        {
            RemoveWaypoint(GetThePoint.PickVector3());
            if (canPlaceWaypoint)
            {   
                distanceCalc.currentDistance = 0;
                canPlaceWaypoint = false;
                return;
            }
            if (!canPlaceWaypoint)
            {
                distanceCalc.currentDistance = 0;
                canPlaceWaypoint = true;
                return;
            }
            return;
        }

    }

    public override void RemoveCommand(){

            RemoveWaypoint(GetThePoint.PickVector3());
            distanceCalc.currentDistance = 0;
            canPlaceWaypoint = false;
            return;

    }

    public override void _DeSelectEvent()
    {
        base._DeSelectEvent();
        hoveringOverSomething = false;
        canPlaceWaypoint = false;
    }

    public void HoveringOverSomething()
    {
        if (canPlaceWaypoint)
        hoveringOverSomething = true;
    }

    public void NotHovering()
    {
        if (combatController.selected)
        hoveringOverSomething = false;
    }

    public void Ready()
    {
        Invoke("DelayedReady", .1f);
    }

    void DelayedReady()
    {
        canPlaceWaypoint = true;
        this.enabled = true;
    }

    public override void OnDisable()
    {
        Selection.Enter -= HoveringOverSomething;    
        Selection.Exit -= NotHovering;

        base.OnDisable();
    }

    void OnDestroy()
    {   
        combatController.selected = false;
        NavWaypoint.DeSelectAllEvent -= _DeSelectEvent;        
        combatController.SelectEvent -= Ready;
    }

	public override void _ButtonClick()
    {
        print("Move button was clicked");
    }

}
