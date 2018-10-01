# ArduinoDaytona
Scripts and code to use an Arduino with M2 Emulator for leader lights.

This is a personal project, however it is posted here mostly so I don't lose the code,
but it also may be handy for others who wish to build something simple like this.

# Hardware
The hardware side of this project is very simple. I'm using a cheap Arduino Nano clone and a couple 
of relay boards to drive some 12V LED Strips. These strips replace the 120V "leader" lights in my cab.

I used relays as I had them laying around, however you might consider using mosfets instead, as it'll 
allow for faster switching, dimming, etc.  

# Software
The software side is a little more complicated:
 1. Arduino sketch to control a couple of relays via USB Serial
 2. A Socket Server and client to forward commands on to the Arduino. 
 3. Lua script for the M2 Emulator (http://nebula.emulatronia.com/). The script starts the server
    and calls the client when the value of the leader lamp changes each.  
    
 
Ie something like this:
```
                                                                          
                                                                          
            +---------------+                                             
            |               |                                             
            |    Arduino    |                                             
            |               |                                             
            +---------------+                                             
                    |                                                     
                    | USB Serial                                          
                    |                                                     
            +-------|-------+                                             
            |               |                                             
        +----     Server    ----+                                         
        |   |               |   |                                         
        |   +---------------+   |                                         
        |                       |                                        -
        |                       |                                         
+-------|-------+       +-------|-------+                                 
|               |       |               |                                 
|    Client     |       |     Client    |                                 
|               |       |               |                                 
+-------|-------+       +-------|-------+                                 
        |                       |                                         
        |                       |                                         
+-------|-------+       +-------|-------+                                 
|               |       |               |                                 
|     LUA       |       |      LUA      |                                 
|               |       |               |                                 
+---------------+       +---------------+                            

```

# Installation
 1. Open the Arduino sketch and modify the pins to suit your own environment. 
    In my case Player 1 is on Pin 7 and Player 2 is on Pin 13. Deploy to your Arduino.
	
 2. Modify the Lua script to set the appropriate COM Port and IP address for 
    your implementation. 
 
 3. Copy the files from ./Lua to the ./scripts folder of your m2 installation
 4. Copy the Server files to the root of your m2 installation.

Start Daytona. I recommend going to the setup menu and confirm the light comes on on the output test. 
Enjoy!

