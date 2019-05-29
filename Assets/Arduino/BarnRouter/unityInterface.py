import time
import zmq

context = zmq.Context()
socket = context.socket(zmq.REP)
socket.bind("tcp://*:5555")

while True:
    #  Wait for next request from client
    message = socket.recv()
    if (message != "null"):
      print(message)
      with open('encoderVal.txt', 'r+') as f:
        # Wipe the file so it's only the most recent value
        f.truncate()
        f.write(message)
      
    time.sleep(.03)

    # Response (Required)
    socket.send( "Received" )

f.truncate()
f.close()
