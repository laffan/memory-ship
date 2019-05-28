#include <FastLED.h>

#define NUM_LEDS 7
CRGB leds[NUM_LEDS];
#define LED_PIN  2

String inString = "";

/* RGB Loop */
int cycle = 6;
int brightness = 0;
int maxBrightness = 255;

int r;
int g;
int b;

int changeSpeed = 2;
String inData;


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

            int direction = inData.toInt();
            updateColor( );
            
            inData = ""; // Clear recieved buffer
        }
    }
}

void updateColor(   ) {

//  changeSpeed = changeSpeed * direction;

  if ( brightness >= maxBrightness ) {
    brightness = 1;
    
    if ( cycle < 6 ) {
      cycle++;
    } else {
      cycle = 1;
    }
    
  } else {
    brightness = brightness + changeSpeed;
  }

  if ( cycle == 1) { /* r --> rb */
    r = maxBrightness;
    g = 0;
    b = b + changeSpeed;
  }
  else if ( cycle == 2) { /* rb --> b */
    r = r - changeSpeed;
    g = 0;
    b = maxBrightness;
  }
  else if ( cycle == 3) { /* b --> bg */
    r = 0;
    g = g + changeSpeed;
    b = maxBrightness;
  }
  else if ( cycle == 4) { /* bg --> g */
    r = 0;
    g = maxBrightness;
    b = b - changeSpeed;
  }
  else if ( cycle == 5) { /* g --> rg */
    r = r + changeSpeed;
    g = maxBrightness;
    b = 0;
  }
  else if ( cycle == 6) { /* rg --> r */
    r = maxBrightness;
    g = g - changeSpeed;
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
