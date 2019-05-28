import serial
from time import sleep
import os

# Connect to each Arduino individually
serialUno = serial.Serial("/dev/cu.usbmodem141101", 9600, timeout=0)
# serialTrinketA = serial.Serial("/dev/cu.usbmodem1414401", 9600, timeout=0)
# serialTrinketB = serial.Serial("/dev/cu.usbmodem1414301", 9600, timeout=0)
# serialTrinketC = serial.Serial("/dev/cu.usbmodem1414201", 9600, timeout=0)
# serialTrinketD = serial.Serial("/dev/cu.usbmodem1414101", 9600, timeout=0)
# serialTrinketE = serial.Serial("/dev/cu.usbmodem141301", 9600, timeout=0)
serialTrinketF = serial.Serial("/dev/cu.usbmodem141201", 9600, timeout=0)
# (+ 6 more Trinkets)


# A - 14401
# B - 14301
# C - 14201
# D - 14101
# E - 1301
# F - 1201

# Open a text file to save the encoder value in
f = open("encoderVal.txt", "w")


def validateString( output ):
  output = output.replace('\r', '') 
  return (output[0] == "@" and output[-1:] == "@" and output != "@") if True else False

lastDataVerified = ""

while True:
  # Read encoderVal from the UNO
  data = serialUno.read(9999)
  if len(data) > 0:
    # Send data to each trinket
    # Remove occasional whitespace.
    dataStripped = data.strip()
    # Split data in to array at line breaks.
    dataSplit = dataStripped.split('\n')
    # Loop through array to received strings and return
    # most recent that is valid (ie. has "@" at each end.)
    dataVerified = ""
    i = 1

    while dataVerified == "":
      try:
        if (validateString(dataSplit[len(dataSplit) - i])):
          # If validated, send to
          dataVerified = dataSplit[len(dataSplit) - i]
          # Create a backup for corrupted instances
          lastDataVerified = dataVerified

      except IndexError as error:
        print(" >>>>>> Corrupted! <<<<<<<< ")
        # If corrupt, use last verified value.
        dataVerified = lastDataVerified
        pass

      i += 1

    print("%s" % (dataVerified))

    variableString = dataVerified.replace('@', '')
    direction = variableString.split(',')[0].split('=')[1]
    value = variableString.split(',')[1].split('=')[1]

    print("direction: %s" % (direction))
    print("value: %s" % (value))
    print("---------------------")

    # serialTrinketA.write(direction + "\n")
    # serialTrinketB.write(direction + "\n")
    # serialTrinketC.write(direction + "\n")
    # serialTrinketD.write(direction + "\n")
    # serialTrinketE.write(direction + "\n")
    serialTrinketF.write(direction + "\n")

    # Save val to text file to Unity
    with open('encoderVal.txt', 'r+') as f:
      # Wipe the file so it's only the most recent value
      f.truncate()
      f.write(value)

    
  sleep(0.2)

# Close serial connections on interrupt
serialUno.close()
# serialTrinketA.close()
# serialTrinketB.close()
# serialTrinketC.close()
# serialTrinketD.close()
# serialTrinketE.close()
serialTrinketF.close()
# Empty & close file
f.truncate()
f.close()



