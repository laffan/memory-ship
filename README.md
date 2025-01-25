# Memory Ship

![README_graphic](https://github.com/user-attachments/assets/cf86a659-f53d-4081-9a2a-58c4d4f2e53d)


The Memory Ship (2018) was an interactive digital installation constructed for a friend's wedding. With whimsical, abstract themes, the installation invited the viewer to navigate a three dimensional dream environment using a ship captain's wheel. Inside the environment were a series of videos from couple's childhood, while outside it were LED lanterns that reproduced hues from inside the game world. The memory ship explored spatial metaphor, blended reality and how alternative control through a memory space changes our relationship to it.

## Installation

### Unity
Unity _should_ automatically install everything you need when you open up the project for the first time.

### Python
The only nonstandard packages being used (I think) are zmq and pySerial. PySerial might need you to _uninstall_ the existing serial interface (`pip uninstall serial`) before installation (`pip install pyserial`)

### Arduino
The only special treatment here is teaching your Arduino IDE to talk to TrinketM0s (if you want to make a change) which [Adafruit has a whole guide on](https://learn.adafruit.com/adafruit-trinket-m0-circuitpython-arduino/arduino-ide-setup). 

### Usage

1. cd in to BarnRouter and start `python serialInterface.py` in one session and `python unityInterface.py` in another.

2. Run the game.

## Libraries

Uno (Rotary Encoder) → Unity
https://github.com/dwilches/Ardity

Unity → Python
https://github.com/off99555/Unity3D-Python-Communication

Python → Trinkets
https://github.com/pyserial/pyserial
