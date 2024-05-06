# 750-Final-Project
 
Iain Roach
IGME 750 Final Project
TwinStick sandbox

Originally I wanted to create a 2D platformer as that is what I worked on previously in this class, I also wanted to mess around with voxel based game and the quadtree requirement felt like a good oportunity to implement a voxel based approach.  However after making a voxel generater Unity's raycast system which I used in the 2D character controller didn't like hiting the voxels so I pivoted to a spaceship sandbox as I wouldn't need raycasts >:/

Technical Aspects:
    I have a semi optimized quadtree that can have bulk insertion in real time. that I used to generate a singular large mesh from all the voxels.  You can use an image of noise to generate voxel terrain and use a threshold value do change what the image to voxel script considers as a value that would result in a voxel.  All non temp graphics were made in aseprite and there is a shader on the voxels that change their color depending on the data the voxel as.  Currently everything is either 0 being empty space or 1 being a default square.  

    The system debugger is called using the /GD:X argument in the command line and the following are the debugger levels:
    (1) * FPS display
    * Position Data
    * Velocity Data
    * Input Data
    * Gameobject count
    * QuadtreeSize
    * QuadTreeUpdate
    * QuadTreeData
    (9) * Master Debugger Level

    The Unit tests are called using the /UT:X argument in the command line
    (1) Size of Quadtree test
    (2) Realtime destruction and creation test (epilepsy warning)
    (3) Depth Limit of QuadTree (so far a depth of 10 or higher seemed to break the generator i currently have)

    Parallax
    there are backgrounds that utilize parallax

    Advanced Input control:
    You use control to play the game
    left stick moves
    right stick aims
    left bumper build
    right bumper destroys

    esc on keyboard opens "debug" menu
    you can regenerate the quadtree.  This can take a few seconds

Unity

Platform:Windows

Features/Controls:
Left Stick: flies the character around
Right Stick: Aims the gun of the character
Right Bumper: Fires projectile that destroys the environment
Left Bumper: Fire projectile that builds
Esc(keyboard): opens/close debug menu

KnownIssues:
one level
No final boss
the player can just leave as there are no boundaries to the world and the quadtree is finite

Credits:
Art-Me
UX/UI-Me
Audio- does not exist :/

Screenshot:
https://cdn.discordapp.com/attachments/1028849967403638794/1237188989702897774/image.png?ex=663abd89&is=66396c09&hm=88d87ec778a7c0b681cac33e60881e237374f4929f8dbc4b173ff5a9a42514c1&

https://cdn.discordapp.com/attachments/1028849967403638794/1237189041460478002/image.png?ex=663abd95&is=66396c15&hm=d026553a20b8ba08526d64ea6970bf04e43a0cd9425c5e98cd4444acb4c8d747& 



Additional Subsystems:
Advanced Input:
Gamepad is used for controls
Parallax, the backgrounds scroll depending on player movement at varying speeds
"debug" menu heaving quotes there where player can either quit the game or regenerate the voxel terrain

postmortem:
A lot of things I want/need to add to this but didn't budget enough time for this :/

futurework would be to fix the raycast issue with the voxel terrain so I could actually make a platformer
