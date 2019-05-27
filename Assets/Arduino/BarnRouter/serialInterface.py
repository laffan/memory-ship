import serial
from time import sleep
import os

serialUno = serial.Serial("/dev/cu.usbmodem141101", 9600, timeout=0)
serialTrinket1 = serial.Serial("/dev/cu.usbmodem1414401", 9600, timeout=0)
# emulator = SerialEmulator('./ttydevice', './ttyclient')

f = open("encoderVal.txt", "w")

print ("Serial Emulator Running...")

while True:
    data = serialUno.read(9999)
    if len(data) > 0:
      print ('Got:' + data )
      serialTrinket1.write(data)
      # socket.send(data)
      with open('encoderVal.txt', 'r+') as f:
        f.truncate()

        dataStripped = data.strip()
        dataSplit = dataStripped.split('\n')
        f.write(dataSplit[-1])

      # emulator.write( data )
      
    sleep(0.05)

serialUno.close()
f.close()



