using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class SerialReceiver : RunAbleThread
{
  protected override void Run()
  {

    ForceDotNet.Force(); // this line is needed to prevent unity freeze
    using (RequestSocket client = new RequestSocket())
    {
      client.Connect("tcp://localhost:5555");

      for (int i = 0; i < 9999 && Running; i++)
      {
        Debug.Log("Sending Hello");
        client.SendFrame("Hello");

        string message = null;
        bool gotMessage = false;
        while (Running)
        {
          gotMessage = client.TryReceiveFrameString(out message); // this returns true if it's successful
          if (gotMessage) break;
        }

        if (gotMessage) {
          CameraController.serialReciever = message;
          Debug.Log(message);
        }

      }
    }

    NetMQConfig.Cleanup(); // this line is needed to prevent unity freeze 
  }
}