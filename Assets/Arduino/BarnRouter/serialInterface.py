import serial
from time import sleep
import os

# Connect to each Arduino individually
serialTrinketA = serial.Serial("/dev/cu.usbmodem1414401", 9600, timeout=0)
serialTrinketB = serial.Serial("/dev/cu.usbmodem1414301", 9600, timeout=0)
serialTrinketC = serial.Serial("/dev/cu.usbmodem1414201", 9600, timeout=0)
serialTrinketD = serial.Serial("/dev/cu.usbmodem1414101", 9600, timeout=0)
serialTrinketE = serial.Serial("/dev/cu.usbmodem141301", 9600, timeout=0)
serialTrinketF = serial.Serial("/dev/cu.usbmodem141201", 9600, timeout=0)

# Hub Addresses
# 1 - 14401
# 2 - 14301
# 3 - 14201
# 4 - 14101
# 5 - 1301
# 6 - 1201

prevEncoderVal = ""

while True:
  f = open("encoderVal.txt", "r")
  encoderVal = f.read()

  if ( encoderVal != prevEncoderVal and encoderVal != ""):
    prevEncoderVal = encoderVal
    print(encoderVal)
    serialTrinketA.write(encoderVal + "\n")
    serialTrinketB.write(encoderVal + "\n")
    serialTrinketC.write(encoderVal + "\n")
    serialTrinketD.write(encoderVal + "\n")
    serialTrinketE.write(encoderVal + "\n")
    serialTrinketF.write(encoderVal + "\n")
    
# Close serial connections on interrupt
serialTrinketA.close()
serialTrinketB.close()
serialTrinketC.close()
serialTrinketD.close()
serialTrinketE.close()
serialTrinketF.close()



