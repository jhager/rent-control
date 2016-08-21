# RENT CONTROL

This project is for RENT CONTROL a little hololens game about Apartment Management.  This project contains the code that is not hololens specific.  This can be run directly on a Windows or Mac laptop to view or play the game.   

More information can be found here: https://westert.itch.io/rent-control

The full Hololens application can be found here: https://github.com/thomaswester/hololens-pdx16gamejam

### Server

The project optionally supports a sever.  To start a server use https://github.com/typicode/json-server  

The server needs the following initial db.json

```{ "games" : [] }```

The default scene has the network game turned off.  To turn it on just enable the Network Client script on the Main Camera GameObject.

### Running in Unity

This was tested to run in Unity 5.3.2f1 for the Mac but should work within other Unity environments.   

### How to Play

Each player uses A S or D to select which piece to play.  The arrow keys can be used to flip and rotate the pieces.  P & Return will reset the game.  1 and 2 will merge the piece into the board.
