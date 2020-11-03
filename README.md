************************************************************************
Modification Description:

Changed the get and set of Monogame.BlockState property. Line 20 in MonogameBlock 

Changed ScoreManager.Level to 1 from -1 on BlockManager.cs Line 34

Changed hard coded initial ball position in ball.cs line 43

Changed hard coded speed and direction on launch for Ball.cs, implemented a random int for speed between 190 and 225, the random direction takes a random double * TwoPi and takes the -Sin of that for y, and the paddle's X direction for X. (-sin since monogame's Y axis is flipped)

Changed the hard coded positions to make them relative to window size on ScoreManager.cs lines 48-50

Changed speed of the paddle on line 26 of paddle.cs

Changed Console text, lives, score and level text locations

Changed amount of lives on scoremanager.cs line 59

Changed direction difference on ball/paddle collision on lines 134 and 138 paddle.cs

Added a win loss method, sprite batch, sprite font, InputHandler and a gameOver bool to blockManager. 

Added different color blocks based on level in MonogameBlock.cs lines 41-50

Added slow time and slingshot mechanics to paddleController

Took out consle debug text

Changed background color

Why I think these changes make the game more fun:
- Introduces more randomness (launch angle/speed, direction on reflect etc) for a more varied experience each game
- More visually appealing (block/background color, no debug text)
- More intense (Speed of ball and paddle were increased for faster gameplay
- More interactive (slow time and slingshot mechanics)
************************************************************************

************************************************************************
Structural Changes:
The only class I added was Audio.cs to handle the audio. Other than that, I modified every class but Levels (1,2,3)  and Game1 to incorporate the changes I needed to make. 
This is because the foundation was almost all already there to make my modifications, so I chose to keep it simple and not add extra classes.

************************************************************************

************************************************************************
State Changes:

I changed the way BlockState was getting and setting in MonogameBlock.cs. It was returning this.block.BlockState = this.blockState instead of just this.block.BlockState, 
so the blockState was never being updated, causing the bug where the ball would clip inside of invisible blocks since they were never counted as being broken.

************************************************************************

************************************************************************
Maintainability: 

There are definitely some things I would go back and clean up if I had more time. Although my OOP skills aren't on par with yours yet, 
I'm getting there, and improving after every assignment. Some methods were too large and should have been split up into multiple. 
There were probably some instances where I could have made an extra class, and some variables I shouldn't have made private. But overall 
I think the code is still moderately maintainable. 
************************************************************************

 
