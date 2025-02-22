#!/usr/bin/python

import serial
from time import sleep
import os

connections = []
maxBrightness = 255
fullSpectrum = maxBrightness * 6
colorDistance = 90
prevEncoderVal = ""

# Connect to each Arduino individually
trinkets = [
    "/dev/cu.usbmodem1414401", # 1
    "/dev/cu.usbmodem1414301", # 2
    "/dev/cu.usbmodem1414201", # 3
    "/dev/cu.usbmodem1414101", # 4
    "/dev/cu.usbmodem141301",  # 5
    "/dev/cu.usbmodem141201"   # 6
]

for trinket in trinkets:
  connections.append( serial.Serial(trinket, 9600, timeout=0) )

while True:
  f = open("encoderVal.txt", "r")
  encoderVal = f.read()

  if ( encoderVal != prevEncoderVal and encoderVal != ""):
    
    prevEncoderVal = encoderVal
    # Convert encoderValue (back) to int
    encoderValInt = abs(int(encoderVal))
  i = 0
  for i in range (len(connections)):

    # Place starting points evently across spectrum
    # encoderValAdjusted = encoderValInt + (maxBrightness * (i+1))
    # Spread hues across colorDistance 
    encoderValAdjusted = encoderValInt + (colorDistance * (i+1))

    # Normalized new starting points on spectrum
    colorLoc = encoderValAdjusted % fullSpectrum if (encoderValAdjusted >= fullSpectrum) else encoderValAdjusted

    print("encoderval: %s" % (encoderValInt))


    # Update lights
    try:
      connections[i].write("%s\n" % (colorLoc))
    except:
    # except ZeroDivisionError as e:

      print("Serial Exception")
      # pass
    
# Close serial connections on interrupt

for connection in connections:
  connection.close()
