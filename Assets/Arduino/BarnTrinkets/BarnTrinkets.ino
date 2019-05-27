#include <FastLED.h>

#define NUM_LEDS 7
CRGB leds[NUM_LEDS];
#define LED_PIN  2

String inString = "";

/* RGB Loop */
int cycle = 6;
int brightness = 0;
int maxBrightness = 130;
int r;
int g;
int b;

int changeSpeed = 1;
long encoderVal = 0;


void setup()
{

  r = maxBrightness;
  g = maxBrightness;
  b = 0;

  Serial.begin(9600); // Initialize serial port to send and receive at 9600 baud
  FastLED.addLeds<WS2812, LED_PIN, GRB>(leds, NUM_LEDS);

}

void loop()
{

  // Read serial input:
  while (Serial.available() > 0)
  {
    int inChar = Serial.read();
    if (isDigit(inChar))
    {
      // convert the incoming byte to a char and add it to the string:
      inString += (char)inChar;
    }
    // if you get a newline, print the string, then the string's value:
    if (inChar == '\n')
    {
      encoderVal = inString.toInt() ;
      // clear the string for new input:
      inString = "";
      updateColor( );
    }
  
    
  } /* While serial available */

}

void updateColor(  ) {

  if ( brightness >= maxBrightness ) {
    brightness = 1;
    
    if ( cycle < 6 ) {
      cycle++;
    } else {
      cycle = 1;
    }
    
  } else {
    brightness++;
  }

  if ( cycle == 1) {
    b = b + changeSpeed;
  }
  else if ( cycle == 2) { /* rb --> b */
    r = r - changeSpeed;
  }
  else if ( cycle == 3) { /* b --> bg */
    g = g + changeSpeed;
  }
  else if ( cycle == 4) { /* bg --> g */
    b = b - changeSpeed;
  }
  else if ( cycle == 5) { /* g --> rg */
    r = r + changeSpeed;
  }
  else if ( cycle == 6) { /* g --> rg */
    g = g - changeSpeed;
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
