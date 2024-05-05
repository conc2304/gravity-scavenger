# Gravity Scavenger
By Jose Conchello

Harvard Extension School | GD50 Game Development Final project

Spring 2024

Developed on Unity v.2022.3.22f1

Final Project Video: [https://youtu.be/Cs_P3tOuU4w](https://youtu.be/Cs_P3tOuU4w) 


<!-- Intro -->
## Intro
Gravity Scavenger is a 2.5D top down shooter and procedural open world explorer. While learning trigonometry and calculus, I was inspired to create a game that incorporated some of the concepts I had been learning. The first thing that came to mind was using calculus to plot the trajectory path of spaceships as they encounter gravitational fields. Gravity Scavenger is inspired by 3 games in particular: the classic Asteroid game, Spore, and Gravitura. I liked the game play of Spore where you are a creature trying to evolve, and I liked the game mechanics of using the mouse to direct the player while keeping the player in the center in an infinitely scrolling and generating space. Gravitura inspired a lot of the game mechanics of using gravitational fields on planets and having pickups to maneuver to, as well as their implementation of a trajectory path, but I did not want to design and create individual hard coded levels and would rather it the levels procedurally generate an infinite scrolling game like Spore.

In Gravity Scavenger you are the pilot of your ship that is out in deep space. You are a space scavenger trying to survive on limited fuel. You will need to scavenge for fuel, health, and parts to upgrade your ship as you traverse space. But it is not that easy. As you traverse the galaxy you will find that you have to learn to harness the gravitational pull of planets in order to get the most of your fuel and survive. Lucky for you your ship comes with a built-in flight path trajectory that will forecast your flight path given the gravity of planets acting on your ship. Use this to predict where planets will be and where your ship is headed with their gravity in mind.

Piloting your ship through the galaxy, you can scavenge for and collect parts you find in asteroid or around planets. You can take these parts to any space station and use to upgrade your ship. Upgrade things like your ship's thrust, armor, firing rate, and firing range among other things. The more you upgrade, the better chance you have of surviving. And while you are here at the station enjoy a free fuel up on us. But watch out, there are other scavengers out there trying to steal from you. Look out for raiders, they will try to shoot you down and board you to take your hard-earned parts. Use the gravitational pull of the planets to your advantage as you engage in dog fights with other raiders. Take them down and you can scavenge their parts. As you gain XP in the game, more complex planetary systems emerge to challenge you. Let's see how long you can last in deep space as a Gravity Scavenger.

## Game mechanics/controls
- The game's starting scene is Home
- Use your mouse to steer your ship in the direction of your mouse.
- Press the left mouse button to add thrust to your ship to move forward. Using thrust depletes your fuel, and if you run out of fuel, you die.
- You start with 3 lives.
    - TODO: Add ability to purchase lives or extra life slots in upgrade center.
- Press the right mouse button or space to fire your lasers.
    - You can shoot asteroid to see if they have any items for you.
    - You can shoot down enemies and scavenge their parts.
- Crashing into asteroid does damage to your ship. But can also be a lifeline if the alternative is crashing into a planet.
- Crashing into a planet is instant death, and you lose a life.
- Crashing into an enemy ship, believe it or not, also instant death, you lose a life, and you lose some parts to being scavenged.
- Once you lose all 3 lives it is game over.

- **Tip**s: Go slow, let yourself drift, don't fight gravity and learn to use it to your advantage. This may sound obvious, but don't fly straight into planets because you probably won't survive it. It is also best to go in a horizontal direction versus a vertical one so that you have a little more time to react to incoming planets. Orbiting planets is a good strategy to use for having a dogfight with an enemy ship.


<!-- Directory Overview -->
## Assets Directory Overview
**Scripts associated with prefabs will be described in full in the Script's section**
- `/Audio`: Contains all the game's audio sources.
    - Most game audio came from a free sound library at Pixabay, and background music from Soundcloud with some modification to make game audio infinitely loop.
    - https://pixabay.com/sound-effects/search/game/
- `/Fonts`: Contains fonts used in the UI found on https://www.1001fonts.com/
- `/Materials`: Simple materials made in unity that are used to color various models in the game.
- `/Prefabs`: 
    - `/Pickups`: prefabs for spawning pick up items
        -  Fuel, Health, & Parts Pickups were made using Unity Pro Builder
            - Add fuel/health/parts to player stats as they pick them up
            - Tag for fuel/health/parts is used for determining pickup type
            - Comprised of Mesh Collider for collision detection
            - Scripts:
                - Power Up 
                - Orbit Object
        - PowerUp Effect \<color> are parent wrappers around particle effects from Unities Particle Pack (in `/Vendor`).  The use of the wrapper is so that the particle effects can be scaled to the appropriate size.
            - Contains Audio Source to play pickup effect sound
    - `/Planets`: prefabs for spawning planets, asteroidsm and moons
        - Low Poly Planet models are from the Unity Asset Store via [One Potato Kingdom](https://assetstore.unity.com/packages/3d/environments/stylized-planet-pack-full-148233)
            - Comprised of Rigid Body for physics and Mesh collider for collision
            - Scripts:
                - Gravity Field : uses the mass of the rigid body and the player's rigid body mass to apply gravitation attraction around each planet.
                - Planet Collision
                - Rotate
        - Asteroid models are from [CGTrader](https://www.cgtrader.com/) with ground material from the Ground Texture from [TextureCan](https://www.texturecan.com/details/136/)
            - Comprised of Rigid Body for physics and Mesh collider for collision
            - Scripts:
                - Asteroid
                - Orbit Object
                - Despawn On Distance
        - Moon models are basic spheres with texture materials via Unity Asset Store from [101 Artistic Noise Textures by Alvios](https://assetstore.unity.com/publishers/17296)
            - Same as planets
        - Cylon Raider spaceship prefab from [CGTrader](https://www.cgtrader.com/free-3d-models/space/spaceship/cylon-raider) is the spawned enemy
            - Comprised of the following components
                - Rigid Body for physics 
                - Mesh Collider for collision detection
                - Audio Source for laser gun
                - Audio Source for on death explosion sound
            - Comprised of the following scripts (see scripts for description)
                - Despawn on Distance
                - Enemy Controller
                - Enemy Stats
            - Contains a Firing Point game object that is used to indicate where to shoot lasers from.

        - Laser prefab: styled capsule model with attached scripts
    - **misc**
        - Laser: Capsule model made in Unity; Fired by player and enemy ship to do damage to each other
            - Comprised of Rigid Body for physics and capsule collider for collision
            - Scripts:
                - Laser
        - Station: Where players go to in order to make upgrades to their ship
            - Comprised of a space station model from [CGTrader](https://www.cgtrader.com/free-3d-models/space/spaceship/babbage-station)
            - Station Barrier Component 
                - uses the Force Field material shader from [Ultimate 10 plus Shaders from Unity Asset Store](https://assetstore.unity.com/packages/vfx/shaders/ultimate-10-shaders-168611)
                - Capsule Collider to trigger on enter and load the Player Upgrade scene
            - Point lights to illuminate the space station

- `/Scenes` : Contains the Unity game scenes
    - Home: Home Screen
        - Buttons to go to Help or Play scenes or Quit
    - Help: Short help / how-to section
    - Play: Main game play 
    - Player Upgrade: Where players can use points to upgrade their stats
    - Game Over: Shows total points
        - Buttons to go to Help or Play scenes or Quit
- `/Scripts`: Primary Scripts driving gameplay
    - `/Audio`: Scripts related to scene audio
        - `AudioManager.cs`
            - Singleton class to access the audio sources to control them from anywhere so that audio can persist between scenes.
            - Takes in Audio Sources as input for the following class   properties: mainMenuAudio, gameOverAudio, gamePlayAudio, playerUpgradeAudio.
            - Public method Play that takes in an AudioSource and sets it to play. Takes in a boolean, stopOthers, to stop all other audio sources in the Audio Manager.
        - `<GameOver/Home/Play/PlayerUpgrade>Audio.cs`:
            - Starts the appropriate audio for each scene on Start(). Uses AudioManager instance's Play method to stop any other music that might overlap or interfere;
    -`/LevelSpawning`: Scripts associated with the spawning of the level/world
        **In order of Importance**
        - `WorldSpawner.cs`:
            - Responsible for infinite spawning of sections, or "chunks", of the game as the player moves around the game space allowing for infinite scrolling in the X and Y direction.
            - Takes in lists of Game Objects from the Unity Inspector for the categories of things that it spawns: Planets, Enemies, Pickups, Asteroids, and Space Stations.
            - **Process**:
                - Calculate the size of the game view in world spaces
                - We are defining chunk to be the size of the game screen times the given multiplier;
                - Create a grid of "Chunks" around the player stores in a HashSet by their chunk coordinate.
                - Initialize the chunks that are offscreen that a player can go into
                - On Update() lifecycle call, get the current chunk.  If the current chunk coordinate is not the previously set coordinate then update the hash set with new chunks.
                - The current Chunk coordinates correspond to the player's position in world space and the sice of the chunk.
                - When updating chunks, the code loops over the chunks that surround the current chunk that the player is in.  If the chunk is not already loaded then load it.  If the chunk is no longer 1 coordinate position away in the x and y, then unload it. (More in Chunk.cs)
            - Applied to an empty game object in Play Scene
        - `Chunk.cs`:
            - Responsible for spawning and despawning entities inside of a given "Chunk" of space via instancing of a Chunk via  `WorldSpawner.cs`.
            - **Process**:
                - Chunk class takes in a set of coordinates and size used to spawn items within that given boundary. 
                - Takes in lists of Game Objects as instancing argumetns for the categories of things that it spawns: Planets, Enemies, Pickups, Asteroids, and Space Stations.
                - Load() method uses randomness to determine whether the Chunk is empty or not. Only empty chunks will have Space Stations instantiated.  Randomness is also used to determine the chance of enemies, pickup items, planets, and asteroids.
                - There are methods responsible for the logic for spawning each type of game object.
                - Planet spawning has the more complex logic of the spawners. A small random number of planets are spawned using a variety of random ranges; the mass, maximum gravitation distance, and size of the planet are dynamically set to create a variety of planetary configurations.  The mass of the planet's Rigid Body is directly relates to the force of the gravitational pull that the planet enacts on the player.  The maximum distance relates to how far the gravitational field goes.
                - To incentivise players to navigate through the collision prone gravitational areas, pickups are dispersed in a circle around the planet.
                - Additionally, when a player gains a given number of XP (currently set to 200) there is a chance that these pickup will orbit around the center of the planet. And at 275xp the radius of the orbit will fluctuate bringing the pickups closer and farther from the planet making it more difficult for the player.
        - `GravityField.cs`:
            - Responsible for adding the gravitational pull force of a game object, given their rigid body mass, onto another game object. Currently just the player, but it is built so that it can attract other items.
            - It uses Newtons law of universal gravitation to calculate the gravitational force of the gravitational body on the object that it is attracting, the attractee.
            - If the attractee is within gravitational range, then that force is then added to the Rigid Body via AddForce(Vector3 force) causing the gravitational attraction of the player towards the planets.
        - `Pickup.cs`:
            - Responsible for the behavior, animation, and sounds associated with in game pickups.
            - Pickups in the game include Fuel, Health, and Parts, that a player must run over to refuel, heal, and collect parts for upgrades.
            - Pickups have a pickupValue which is how much Fuel/Health/Parts to add to player stats and an xpValue which is added to a player's experience points.
            - It uses the OnTriggerEnter to detect collision since physics based forces are not being applied here.
            - On collision, it uses the tag of the prefab to distinguis between Fuel/Health/Parts and appriopriately applies them to player stats.
            - Only player's trigger the powerUp animation which is a combition of instancing a pickupEffect prefab from Unitis Particle Pack, and a pickup sound from pixabay.com.
        - `Asteroid.cs`:
            - The addition of asteroids is inspired by the classic game Asteroids.
            - Asteroids are instantiated in the Chunks.cs script and derive their stats from entity stats. They are instantiated with random health and damage.
            - Colliding with an asteroid causes minor damage to the player, and then stops causing damage for short time to not repeatedly cause damage each frame using a collision timer.
            - Deriving from EntityStats gives the asteroid the ability to take and give damage.
            - Asteroids die and explode when a player has shot them enough with lasers.
            - Asteroids have a random chance of leaving a pick up item for the player to scavenge.
            - Exploding asteroids shoot out a small debris field on explosion that can cause damage to the player.
        - `PlanetCollision.cs`:
            Resonsible for detecting the collision of gameObjects with the "Player" tag and calling their public Die() method.
        - `OrbitObject.cs`:
            - Responsible for making objects move in a kinematic fashion by applying the formula for a circle and updating the angle of position over time.
            - Has a dynamicRadius flag that allows for the radius of the orbit to change over time by fluctuating from near to far for an increased challenge at higher xp.
        - `DespawnOnDistance.cs`:
            - Responsible for despawning items when they are too far from the player.
            - Because some game objects are spawned outside of the the WorldSpawner and Chunk process, ie via ships or asteroids leaving behind pickups after they die, this destroys items when they are too far to prevent keeping too many things in memory.
    - `/Stats`: Responsible for player, enemy, and entity stats, and persistent player stat storage
        - `EntityStats.cs`:
            - Is the base class that defines basic entity attributes.
            - It has overridable virtual methods that allows methods to be overridden in derived classes.  This allows us to have different methods for how to handle TakeDamage, and Die() for different instances.
            - It also takes in an Audio Source that is used by derived classes for their Die() methods.
            - Sets up basic entity stats such as health, damage, armor.
            - The `isDead` boolean property is to flag the entity as having died. There are a few instances where it is necessary to delay the destruction of the game object until an animation or a sound has stopped playing, but the object is still active, just not being rendered.
        - `UpgradableStats.cs`:
            - Class responsible for allowing player stats to be upgraded via the Player Upgrade scene. 
            - Contains properties that include the its value, max value, and number of upgrades, as well as the cost, and cost scale.
            - Using these properties, a player can upgrade a stat up to the given number of upgrades, and as they upgrade/downgrade the cost goes up/down.
            - Upgrade and Downgrade methods update the current value of a stat, the number of upgrades available, and the cost of the next upgrade/downgrade.
        - `PlayerStats.cs`:
            - Derived class from Entity stats. Responsible for updating player stats through via taking damage and adding or removing points/health/fuel.
            - TakeDamage() invokes the base, EntityStats, TakeDamage() and then additionally updates the player stats state and the corresponding UI that renders the health and fuel levels, parts collected, lives, and XP.
            - Die() overrides the vitual method to implement its own bespoke death handler which takes a life away, and plays its death animation.
            - The Die() method plays an explosion animation and sound which are Inspector arguments. Destroying the main player game object causes errors with rigid body physics, so instead the rendered space ship is wrapped in a gameObject parent and then is disable the "Ship Wrapper" while the death animation happens.
            - If the player is out of lives then transition to the Game Over scene, other wise
        - `PlayerStatsManager.cs`:
            - A persistent singleton class used as a state store to persist player stats across scenes.
            - Contains the state of our player at any given time and in any given scene.
            - Uses the UpgradableStat class to instantiate upgradable stats.
            - Implements DontDestoryOnLoad() to persist the game stats across scenes.
            - Implemented as a singleton so that there are never conflicting or multiple state stores.
            - This is where all of a players stats are initialized.
        - `EnemyStats.cs`:
            - Derived class of the EntityStats class, responsible for maintaining the stats for enemy entities.
            - It holds values for the enemy's health, firing range, firing rate, damage.
            - Uses it's parent TakeDamage() method to take damage, but implements its own Die() override method.
            - Die() plays an explosion sound and animation, leave pickups/parts for player to scavenge as a bonus and assigns a random value to that pickup. It destroys the game object after the animation has ended, and disables rendering to hide the ship during the explosion.
            - In order to skew the probbility of dropping certain pickups versus others. EnemyStats has a probabilty map that maps the prefabs index to a probability range.
    - **Uncategorized Scripts**
        - `ShipController.cs`: 
            - Responsible for the movement and control of the player's spaceship via mouse and keyboard.
            - The ship rotates to face the user's mouse on screen, by translating the angle of the mouse to the player in camera view.
            - On left mouse button hold, a thrust force is added to the rigid body of the player's ship
            - On left mouse button down/up
                - Play/Pause thrust audio source
                - Set jet engine particle system emission rate to high/low
            - On right mouse button fire the laser prefab and set the range of the fired laser to the damage and range settings based on the player.
            - Uses PlayerStats to determine thrust amount, firing rate, firing range, and laser damage.
            - A fire rate timer is used to limit the rate at which a player can fire; this stat can be upgraded in the player upgrade scene, and the range is for how far the laser can travel.
        - `EnemyController.cs`: 
            - Responsible for the AI movement logic of the enemy spaceship.
            - If enemy collides with the player then they kill the player and scavenge some of their parts inventory.
            - Sets a random value of aggression and anxiety level for the enemy.
            - Aggression Level dictates how far a target can be before it will start pursuing.
            - Anxiety Level dictates how close an enemy will get to its target.
            - This is to create a small variety in the behavior of the enemy.
            - Controls the direction the enemy is facing by rotating towards its target.  If the target is in front add thrust, up to a certain magnitude so that they don't go zooming through the scene.
            - If the target is the player, and they are ahead of the enemy then shoot at the player.
            - Uses EnemyStats to determine thrust amount, firing rate, firing range, and laser damage.
            - A fire rate timer is used to limit the rate at which a player can fire; this stat can be upgraded in the player upgrade scene, and the range is for how far the laser can travel.
            - TODO: Make a random variation that pursues pickups instead of the player to compete for resources.
        - `TrajectoryLine.cs`: 
            - This component was the entire foundation for the idea of this game. I have been learning trigonometry and calculus the past year and I wanted to incorporate that into this game. So the idea of plotting the trajectory of a spaceship in regard to the gravitational pull of multiple bodies inspired and informed the rest of the game's mechanics.
            - Responsible for plotting the trajectory of the ship given its mass, via Rigid Body physics, and the mass/gravitational force of all the gravity fields acting on it in the game. The Trajectory Line will only render when there are gravitational forces being applied to the ship. Otherwise, it will turn itself off.
            - For this component, I built a simple custom shader material that acts as the animation for the trajectory line. I used Photoshop to create the white to gray faded line, which is then offset over the course of the Line Renderer to show movement in that direction.
            - TrajectoryLine uses the LineRenderer component attached to the game object to render a line with a given number of points on the along the line and a time offset which represents the time between each point on the line.
            - For each point on the line, iterate over all the gravity fields in play and add their forces together in order to get the position of the next point in the line renderer.
            - Due to the fact that the line renderer gets completely culled when any segment of it is outside the camera view, validation is added to make sure that each new segment of the line is valid. If it is not, then fill the rest with the segment's positions to the last valid position.
            - TODO: The original idea was to use differential equations by implementing the Runge-Kutta method to calculate the force of all the gravitational fields.  This however was taking too long to implement, so instead the more rudimentary Euler method is used.
        - `Laser.cs`:
            - Responsible for the movement and behavior of lasers shot by the player and enemies.
            - The Range property dictates the lifetime of the laser in seconds before it gets destroyed if it has not collided with anything.
            - It uses rigid body physics to add a force vector in the forward direction multiplied by the Speed constant to move/fire the laser.
            - Uses a collisionBuffer/Timer to ignore collisions for a brief moment to not collide the shooter's collider.
            - On collision with an entity, it calls the entity's TakeDamage() function with the given damage amount.
            - It then plays the explosion sound and animation before destroying itself
        - `TrackPlayer.cs`: 
            - Responsible for the camera tracking that keeps the player in the center of the screen.
            - The idea to use SmoothDamp is from a tutorial on smooth camera tracking. However, some adjustment will need to be made in the future to properly use the damping amount.
        - `FollowUV.cs`: 
            - Responsible for the bi-directional parallax scrolling effect of the backgrounds that make it appear that the player is moving through space.
            - It is implemented on 3 different star fields/nebula backgrounds, each with a different level of parallax to simulate things closer to the camera moving faster than those further away in the background by updating x and y offset of the material.
        - `LoadPlayerUpgradeScene.cs`: 
            - Responsible for taking the user to the Player Upgrade scene when they fly into the space station.
            - Attached the blue barrier game object with a capsule collider that detects when the player has entered the space station area and takes them to the player upgrade scene.
        - `Rotate.cs`: 
            - Responsible for adding constant rotation to objects on the X/Y/Z directions.
            - This is used on components like planets, and the rings around the pickups to give them a rotation animation.
- `/Shaders`: 
    - `TrajectoryLine.shadergraph`:
        - The rendered material for the TrajectoryLine game object.
        - Made in Unity's shader graph editor, it takes in a texture, the number of dashes for how many times to tile the texture, and speed at which to translate the dash texture.
        - Main texture for the line animation was made via photoshop.
        - Uses the Y offset if the texture to scroll the texture over the line.
- `/Textures`:
    - Contains the textures/images used in the game.
    - The `/Backgrounds` and `/Ground` assets are from free asset repositories online.
    - The icons were made in photoshop.
    - The main game logo was made with generative AI: Dall-E/
- `/UI`
    - Contains files associated with interactive user interfaces seen in the game.
    - These UI components were made using Unity's UI toolkit. Making UI in this fashion is very similar to making UI for the web, and it gives the developer more granular control over look and behavior as compared to the basic Canvas components. It uses `uxml` as the document to structure the elements, similar to the web's HTML; `uss` is Unity's version of the CSS, and then a c# script acts as the JavaScript to add interactivity.  Coming from a web UI background, this work flow was very intuitive for me, plus the added GUI to edit the document and see the changes as I make them in the UI Editor is a nice perk.
    - UIs include: Main Menu, Game Over, Player Upgrade, Game Play Stats Bar, Help Modal
    - `/PlayerUpgrade`:
        - Responsible for the UI in the Player Upgrade scene.
        - Makes use of UI Toolkit's template for building more complex components out of smaller ones in the `/Components` directory: UpgradeSlot, UpgradeStatRow.
        - Takes in a UI Document, a stat row template, and an upgrade slot template.
        - The `PlayerUpgrade.cs` script starts by getting all  the upgradable stats from the persistent PlayerStatsManager class and for each of those stats it creates a UI row that manages upgrading and downgrading that given stat. Populating the data into the visual elements works similarly to vanilla JavaScript where you query for an element and then set its properties such as text, styles, classes, etc... Each upgradable stat row contains the name of the stat, upgrade slots, and buttons and prices associated with upgrading and downgrading stats.
        - The upgradeSlot visual element is used to show the state of whether an upgrade slot is empty or not to indicate how many of the total available upgrades have been used. When it is used, the "slotUsed" class is added to the element and vice versa.
        - Each row has a button to upgrade and downgrade each of the stats.  If an upgrade or a downgrade is not available then the buttons takes on the styles of the "disabled" class and further button clicks are ignored.
        - When an upgrade or a downgrade is made, all the user stat rows update their state. This is so that upgrades that are too expensive given the player's current available parts can be disabled.
        - There are Audio Sources associated with the buttons for upgrading, downgrading, and for when it is disabled, as well on to confirm upgrade.
        - When the user clicks on the "Upgrade" button, the confirm-audio is played then they are returned to the Play scene.
    - `/GamePlayStatsBar`:
        - Responsible for updating the real time in game stats which include the player's lives, current health, current fuel, parts collects, and XP gained.
        - Using Unity query selectors, the various visual elements and their properties are made accessible via the c# script.
        - Updating the UI reads from the state store that is the PlayerStatsManager instance. It uses those values to set text elements for XP and parts, progress bar values for fuel and health, and reuses the upgradeSlots component to visualize how many lives the play has and has used.
        - Rather than unnecessarily updating on every Update() call in the game, a public UpdateUI() method is exposed so that other components/objects can manually update the UI.
    - `/MainMenu`:
        - Responsible for the UI of the Home Scene whose interactions include moving the player to one of the following scenes: Help, Play, as well as a button to quit.
        - The `MainMenu.cs` is reused on the GameOver UI since the button behavior is the same.
        - The script takes in an audio source for the audio to play when a button has been clicked.
        - On clicking start, it resets all the player's stats so that they are fresh as the player starts a new round.
    - `/GameOver`:
        - Responsible for the UI that is in the Game Over scene which shows the player's score as well as buttons to play again, go home, or quit.
        - This component reuses the USS styles from the MainMenu as well and its buttons for Home/Help/Quit use the click event listeners in the MainMenu.cs.
    - `HelpModal.uxml`: 
        - Responsible for the UI that is in the Help scene that gives players simple instructions on how to play.
        - This component reuses the USS styles from the MainMenu as well and its buttons for Home/Help/Quit use the click event listeners in the MainMenu.cs.