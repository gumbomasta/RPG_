﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatCommandsToUI : MonoBehaviour {

	public delegate void SendUICommand(GameObject o);

	public static event SendUICommand ClearUI;
	public static event SendUICommand SendCommandToUI;

	AvailableCommands availableCommands;

	GameObject ButtonTemplate;

	CombatCommand[] combatCommandList;

    public void combatCommandsToUI(CombatController cController, Color c)
    {
        ClearUI(null);
		combatCommandList = GetComponentsInChildren<CombatCommand>();
		foreach (CombatCommand combatCommand in combatCommandList)
        {
			
		    GameObject b = Instantiate(Resources.Load("ButtonTemplate") as GameObject); //instantiate button
            Button button = b.GetComponent<Button>();         //inherit button properties from Scriptable Object

			Text t = b.GetComponentInChildren<Text>();
			t.text = combatCommand.commandName;

            ColorBlock cb = button.colors;                          //inherit UI Color
            cb.normalColor = cController.combatantColor;            //inherit UI Color pt 2
            button.colors = cb;   	                                //inherit UI color pt 3
			
			button.onClick.AddListener(combatCommand._ButtonClick); 
            SendCommandToUI(b);  
            Destroy(this);
        }

    }

}
