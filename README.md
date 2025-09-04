# Custom ShootEmUp

Rogue shooter looks to implement the base logic of a top down shoot em up with rogue like progression. Using dialogue trees for character interactions on death & Utility UI for enemy ships. Features are designed first as coding challenges. 

I'm developing this project alongside the [Graph Tools Package](https://github.com/Alasdair-PB/AlGraphTools). This package adds support for dialogue interactions and a custom extension to the Unity Graph tools package making graph tools simple to add for Scriptable object workflows. The current dialogue system in this project is in legacy mode and will be replaced with this implementation in full in the near future. 

## Key features
* Modular Character controller with support for different gameplay systems incl: Side scroller and top down scenarios.
* Advanced Enemy and projectile pattern customization. Links actions and projectiles to create entirely unqiue enemies without any additional code. 
From a burst pattern that spawns a prefab in intervals, the pattern can quickly be made more complex by using schedule, loop, layer or reverse operations to make this pattern part of a larger pattern. Similarly, schedule enemies attacks or switch between complicated schedules to make enemies dash around, shoot projectiles and perform actions in unqiue ways. 
* Dialogue, Save and Inventory Systems: Save dialogue interactions and keep track of progression. (Please note the dialogue system will be refactored in the near future)

## Exploring the code
Despite the simple gameplay, the project contains over 70 scripts in total (not including scriptable objects) making diving into this project difficult. Key folders to look at are:
* Assets> Scripts> Player: The different components that can be enabled on the player to add mechanics like jumping, interacting, projectiles and more. 
* Assets> Scripts> Enemies: Explore the state machines of the enemies and data driven approach allowing for unique enemies to be made in the fields of scriptable objects. E_StateMachine and E_Controller provide the base for understanding this design. 
* Assets> ScriptableObjects> Formations> Layered: Showcases object data and references to other patterns that make up the enemy projects. In the asset window->right click->create to see all the custom data objects that can be created to define your game. 

## Exploring the Game

### Controls: 
Move: 'wsad', Dash: 'shift' (can be held) <br>

#### Sample Game specific
Switch Projectile; 'e'<br>

#### Hub specific
Interact: 'e'<br>

### Enemies, player movement and projectile pooling
See 'SampleScene'. Assets> Scenes> SampleScene. Avoid projecitles and defeat enemies. New enemies spawn, when the current enemies leave the battlefield (either by schedule and when defeated). Enemies are represented by white cube, black cubes represent 'tower blocks'. When the player is above a 'tower block' they are able to launch more projectiles, incentivizing players to move often. The scene reloads automatically when the player has cleared all enemies or when defeated. 

### Dialogue and Save
When the project opens the default scene is 'Hub'. Assets> Scenes> Hub. While visually lacking, this scene exists to demo the dialogue and save system of the game. Modular UI systems are being reworked with the dialogue package so dialogue is currently debugged to the console along with save data. The 4 small black cubes represent NPCs, the Green square represents a save point and the white square is the player. The long black squares are walls- blocking progression. NPCs can each be interacted with by pressing 'e'. Talking to the NPC in the center of the room loads the next stage, however he will only let you progress if you have already spoken to every other NPC. If you save after talking to any of these NPCS and restart the scene, you won't need to speak to them again. If you talk to the center NPC before any others beware the player has two choices where one is not selectible unless all NPCs have been spoken to. Use ws to travel to the second option and 'e' to select it to exit this dialogue. This is one area where visual clarity is needed as you cannot see the option selected without UI. 
