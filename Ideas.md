[MainPage](MainPage.md)

# Rough Models #
  * Steven's cube [representation model](StevenCubeModel.md).
  * Chris's cube [idea](PossibleCubeRep.md).

# Modes #
  * Time Attack (competing based on real-time elapsed)
  * Move Battle (competing based on number of moves)
  * Point Match (competing based on points, explained later)

# Networking #
  * Creation of a UofL/Speed school game network.  This is to track user scores and information.
  * Allow unlimited (if possible ?) connections for the ultimate in match play.
  * Use client/server model (not P2P) for centralized control.

# Controls #
  * Abstract input sources
    * keyboard
    * mouse
    * gamepad

# Global Game Parameters #
_Global Game parameters can apply to any mode._
  * Single-Player vs. Multi-Player
  * AI Player(s)
  * Challenge Mode
  * Backdrop
  * Picture Cubelets
  * Color Duplication (opposite sides)
  * "Cubes with guns" -- players' cubes can interact with each other
# Single-Player Modes #
  * Scenario (complete a starting configuration within a specified metric)

# Pointmatch #
_Time and move count both decrease rewards_
  * Create X shape on one side
  * Create X shape amongst the sides
  * Perform X specific rotations

# Challenge Mode #
_Events to mix up a Rubik's Cube game, changing the game experience without changing the core game._
  * Colors switch with each other (red becomes green, green becomes blue, etc)
  * Colors all become hues of one base color (red becomes dark green, blue becomes very dark green, yellow becomes light green, etc)
  * Point Of View shifts randomly (perhaps with a neat "dropped the cube" animation)
  * A cubelet becomes locked for 3 moves
  * Certain rotations are made by the computer (slow enough to follow and undo easily)

_Events to regress player progress_
  * Rigoti's revenge: cube explodes and reforms into different configuration
    * Would occur if solver takes too long or based on some other criteria
    * Perhaps could happen to good players competing against not so good players. (Handicap)

[MainPage](MainPage.md)

