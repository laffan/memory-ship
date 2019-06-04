#include <FastLED.h>

#define NUM_LEDS 7
CRGB leds[NUM_LEDS];
#define LED_PIN  2

String inData = "";

/* RGB Loop */
int serialInt = 0;
int speed = 4; 
int maxBrightness = 255;
int colorLoc = 0;
int r, g, b;


void setup()
{

  r = maxBrightness;
  g = 0;
  b = maxBrightness;

  Serial.begin(9600); // Initialize serial port to send and receive at 9600 baud
  FastLED.addLeds<WS2812, LED_PIN, GRB>(leds, NUM_LEDS);

}

void loop()
{

  while (Serial.available() > 0)
    {
        char recieved = Serial.read();
        inData += recieved; 

        // Process message when new line character is recieved
        if (recieved == '\n')
        {
            Serial.print("Arduino Received: ");
            Serial.print(inData);

            int inDataInt = inData.toInt();
            updateColor( inDataInt );
            
            inData = ""; // Clear recieved buffer
        }
    }
}

void updateColor( int colorLoc  ) {

    int cycle = (colorLoc <= maxBrightness) ? 1 : floor( colorLoc / maxBrightness ) + 1 ;
    int position = (colorLoc <= maxBrightness) ? colorLoc : colorLoc % maxBrightness;

  if (cycle == 1) { /* r --> rb */
    r = maxBrightness;
    g = 0;
    b = position;
  }
  else if (cycle == 2) { /* rb --> b */
    r = maxBrightness - position;
    g = 0;
    b = maxBrightness;
  }
  else if (cycle == 3) { /* b --> bg */
    r = 0;
    g = position;
    b = maxBrightness;
  }
  else if (cycle == 4) { /* bg --> g */
    r = 0;
    g = maxBrightness;
    b = maxBrightness - position;
  }
  else if (cycle == 5) { /* g --> rg */
    r = position;
    g = maxBrightness;
    b = 0;
  }
  else if (cycle == 6) { /* rg --> r */
    r = maxBrightness;
    g = maxBrightness- position;
    b = 0;
  }
  else {
    Serial.print("==================== ");
  }

  uint8_t Ur = r;
  uint8_t Ug = g;
  uint8_t Ub = b;

  for (int i = 0; i < NUM_LEDS; i++) {
    leds[i].setRGB( Ur, Ug, Ub );
  }

  FastLED.show();

}
