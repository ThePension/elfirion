## General improvements

* [ ] Refactor entity system : Player should also be an entity
    * [ ] Base class Entity, then StaticEntity (for trees, rocks, ...) and DynamicEntity (for ennemies, player f.e.)
    * [ ] Then, add feedback force to ennemy when attacked

* [x] Use global loaded class for signals handling (Bus Signals)
    * [ ] Adapt what needs to be adapted to use SignalsBus
* [ ] Improve terrain generation using noise (see : https://www.youtube.com/watch?v=cqyD2EEVD3g&ab_channel=KobeDev)
* [ ] Separate entities and interactions ? -> Global game manager (InteractionsManager or smtg) ?
* [ ] Use `await` for animations instead of callback
* [ ] Improve interaction system with entities
* [ ] Centralize UI ? (Inventory, Player related stuff)
* [ ] Refactor Item system : Weapon should have an inherated implementation for handle their own stuff (animation, generate arrow for bow, ...)
    * [ ] Perhaps would be nice that item (like Sword, Bow) have their own scene ?

## Bugfixes

* [ ] Properly fix the z Index sorting
* [ ] Sometimes the player can not move up anymore
* [ ] Player can cancel interaction's animations by moving a bit
* [ ] Refactor where entities are stored -> Extract them from chunk, because there are being deleted as their parent chunk is unloaded

## Features to add

* [ ] Proper pathfinding algorithm for the companion

* Improve Inventory :
    * [ ] Fast inventory (always display at the bottom of the screen, change selected Item using wheel),
    * [ ] Drag and drop to rearrange locations of items
    * [ ] Sort, filter ?
    * [ ] Use / Consume items from inventory :
        * [x] Apple to increase food and energy
        * 
    
* Static entities :
    * [ ] When dead, replaced by a placeholder (tree seed f.e.), that grows slowly back to the original entity

* Combat :
    * [X] Health system for the player
    * [X] Weapon Item that can be used ?
    * [ ] Shield to block attacks ?

* Ennemy :
    * [ ] Randomly spawn (during the night)
    * [X] Following the player (pathfinding ?) when close enough
    * [X] Attacking the player ?
    * [X] Die when health = 0

* Player
    * [x] Food bar
    * [ ] Decrease health when the food bar is too low
    * [ ] Block sprint when the food is too low
    * [x] Energy bar
    * [ ] Death when health is 0

* World :
    * [ ] Keep track of chunk data (static entities) for chunk reloading
    * [ ] Serializable entities
    * [ ] World saving
