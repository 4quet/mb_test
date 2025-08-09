# mb_test

#### How you approached this test: what were your different game making phases?  
1. Player input and basic movement with a running animation. (~1 hour)
2. Weapons logic with UI buttons to switch weapons. (~1 hour)
3. Improved player controller logic using states: idle, moving, attacking. (~1 hour)
4. Enemy detection using `Physics.OverlapSphere()` around the player character. (~1 hour)
5. Added logic to trigger the death animation of an enemy when attacked. (~30 min)
6. Enemy spawner component, spawning an enemy every 3 seconds randomly around the player. (~1 hour)
7. Added logic to change the player's attack speed based on which weapon is currently equipped. (~20 min)
8. Added small improvement of the joystick's UI to show the current direction of movement. (~10 min)
___
#### The features that were difficult for you and why.
Nothing was particularly difficult because these are pretty common features.
___
#### The features you think you could do better and how. 
- Weapons are all loaded at the start of the game to keep things simple - if the game has more than 3 weapons they should be loaded dynamically using Addressables, for example.
- Weapons stats could use a much more elaborate and flexible system if needed.
- Better way of switching weapons without using buttons, something more integrated with the game's design.
- Better management of animations based on input - properly canceling an attack animation if the player is moving, for example.
___
#### What you would do if you could go a step further on this game.
- Adding AI behavior to enemies, making them more interesting.
- Adding different types of enemies, elite enemies, bosses, etc.
- Adding items, drops, treasure chests.
- Adding character progression, increasing stats, better weapons and gear.
- Creating multiple interesting levels with a proper environment and things to explore.