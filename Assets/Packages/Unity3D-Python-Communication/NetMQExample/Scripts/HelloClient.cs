using UnityEngine;

public class HelloClient : MonoBehaviour
{
    private SerialReceiver _serialReceiver;

    private void Start()
    {
      _serialReceiver = new SerialReceiver();
      _serialReceiver.Start();
    }

    private void OnDestroy()
    {
      _serialReceiver.Stop();
    }
}